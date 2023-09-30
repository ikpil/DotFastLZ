/*
  6PACK - file compressor using FastLZ (lightning-fast compression library)
  Copyright (C) 2007-2020 Ariya Hidayat <ariya.hidayat@gmail.com>
  Copyright (C) 2023 Choi Ikpil <ikpil@naver.com>

  Permission is hereby granted, free of charge, to any person obtaining a copy
  of this software and associated documentation files (the "Software"), to deal
  in the Software without restriction, including without limitation the rights
  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
  copies of the Software, and to permit persons to whom the Software is
  furnished to do so, subject to the following conditions:

  The above copyright notice and this permission notice shall be included in
  all copies or substantial portions of the Software.

  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
  THE SOFTWARE.
*/

using System;
using System.IO;
using System.Linq;
using System.Text;
using DotFastLZ.Compression;

namespace DotFastLZ.SixPack;

public static class Program
{
    private const int SIXPACK_VERSION_MAJOR = 0;
    private const int SIXPACK_VERSION_MINOR = 1;
    private const int SIXPACK_VERSION_REVISION = 0;
    private const string SIXPACK_VERSION_STRING = "snapshot 20070615";

    /* magic identifier for 6pack file */
    private static readonly byte[] sixpack_magic = { 137, (byte)'6', (byte)'P', (byte)'K', 13, 10, 26, 10 };
    private const int BLOCK_SIZE = (2 * 64 * 1024);

    /* for Adler-32 checksum algorithm, see RFC 1950 Section 8.2 */
    private const int ADLER32_BASE = 65521;

    private static ulong update_adler32(ulong checksum, byte[] buf, int len)
    {
        int ptr = 0;
        ulong s1 = checksum & 0xffff;
        ulong s2 = (checksum >> 16) & 0xffff;

        while (len > 0)
        {
            var k = len < 5552 ? len : 5552;
            len -= k;

            while (k >= 8)
            {
                s1 += buf[ptr++];
                s2 += s1;
                s1 += buf[ptr++];
                s2 += s1;
                s1 += buf[ptr++];
                s2 += s1;
                s1 += buf[ptr++];
                s2 += s1;
                s1 += buf[ptr++];
                s2 += s1;
                s1 += buf[ptr++];
                s2 += s1;
                s1 += buf[ptr++];
                s2 += s1;
                s1 += buf[ptr++];
                s2 += s1;
                k -= 8;
            }

            while (k-- > 0)
            {
                s1 += buf[ptr++];
                s2 += s1;
            }

            s1 = s1 % ADLER32_BASE;
            s2 = s2 % ADLER32_BASE;
        }

        return (s2 << 16) + s1;
    }

    private static void usage()
    {
        Console.WriteLine("6pack: high-speed file compression tool");
        Console.WriteLine("Copyright (C) Ariya Hidayat");
        Console.WriteLine("Copyright (C) Choi Ikpil");
        Console.WriteLine("");
        Console.WriteLine("Usage: 6pack [options]  input-file  output-file");
        Console.WriteLine("");
        Console.WriteLine("Options:");
        Console.WriteLine("  -1    compress faster");
        Console.WriteLine("  -2    compress better");
        Console.WriteLine("  -v    show program version");
        Console.WriteLine("  -mem  check in-memory compression speed");
        Console.WriteLine("");
    }

    /* return non-zero if magic sequence is detected */
    /* warning: reset the read pointer to the beginning of the file */
    public static bool detect_magic(FileStream f)
    {
        byte[] buffer = new byte[8];
        int bytes_read;
        int c;

        f.Seek(0, SeekOrigin.Begin);
        bytes_read = f.Read(buffer, 0, 8);
        f.Seek(0, SeekOrigin.Begin);
        if (bytes_read < 8)
        {
            return false;
        }

        for (c = 0; c < 8; c++)
        {
            if (buffer[c] != sixpack_magic[c])
            {
                return false;
            }
        }

        return true;
    }


    public static void write_magic(FileStream f)
    {
        f.Write(sixpack_magic);
    }


    public static void write_chunk_header(FileStream f, int id, int options, long size, ulong checksum, long extra)
    {
        byte[] buffer = new byte[16];

        buffer[0] = (byte)(id & 255);
        buffer[1] = (byte)(id >> 8);
        buffer[2] = (byte)(options & 255);
        buffer[3] = (byte)(options >> 8);
        buffer[4] = (byte)(size & 255);
        buffer[5] = (byte)((size >> 8) & 255);
        buffer[6] = (byte)((size >> 16) & 255);
        buffer[7] = (byte)((size >> 24) & 255);
        buffer[8] = (byte)(checksum & 255);
        buffer[9] = (byte)((checksum >> 8) & 255);
        buffer[10] = (byte)((checksum >> 16) & 255);
        buffer[11] = (byte)((checksum >> 24) & 255);
        buffer[12] = (byte)(extra & 255);
        buffer[13] = (byte)((extra >> 8) & 255);
        buffer[14] = (byte)((extra >> 16) & 255);
        buffer[15] = (byte)((extra >> 24) & 255);

        f.Write(buffer);
    }

    public static int pack_file_compressed(string input_file, int method, int level, FileStream f)
    {
        ulong checksum;
        byte[] buffer = new byte[BLOCK_SIZE];
        byte[] result = new byte[BLOCK_SIZE * 2]; /* FIXME twice is too large */
        byte[] progress = new byte[20];
        int chunk_size;

        /* sanity check */
        FileStream temp;
        try
        {
            temp = new FileStream(input_file, FileMode.Open, FileAccess.Read, FileShare.Read);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: could not open {input_file}");
            Console.WriteLine(e.Message);
            return -1;
        }

        using var ifs = temp;

        /* find size of the file */
        ifs.Seek(0, SeekOrigin.End);
        long fsize = ifs.Position;
        ifs.Seek(0, SeekOrigin.Begin);

        /* already a 6pack archive? */
        if (detect_magic(ifs))
        {
            Console.WriteLine($"Error: file {input_file} is already a 6pack archive!");
            return -1;
        }

        /* truncate directory prefix, e.g. "foo/bar/FILE.txt" becomes "FILE.txt" */
        string shown_name_string = input_file
            .Split(new char[] { '/', '\\', Path.PathSeparator, Path.DirectorySeparatorChar, }, StringSplitOptions.RemoveEmptyEntries)
            .Last();
        byte[] shown_name = Encoding.UTF8.GetBytes(shown_name_string);

        /* chunk for File Entry */
        buffer[0] = (byte)(fsize & 255);
        buffer[1] = (byte)((fsize >> 8) & 255);
        buffer[2] = (byte)((fsize >> 16) & 255);
        buffer[3] = (byte)((fsize >> 24) & 255);
        buffer[4] = (byte)((fsize >> 32) & 255);
        buffer[5] = (byte)((fsize >> 40) & 255);
        buffer[6] = (byte)((fsize >> 48) & 255);
        buffer[7] = (byte)((fsize >> 56) & 255);

        buffer[8] = (byte)(shown_name.Length + 1 & 255);
        buffer[9] = (byte)(shown_name.Length + 1 >> 8);
        checksum = 1L;
        checksum = update_adler32(checksum, buffer, 10);
        checksum = update_adler32(checksum, shown_name, shown_name.Length + 1);
        write_chunk_header(f, 1, 0, 10 + shown_name.Length + 1, checksum, 0);
        f.Write(buffer, 10, 1);
        f.Write(shown_name, shown_name.Length + 1, 1);
        long total_compressed = 16 + 10 + shown_name.Length + 1;

        /* for progress status */
        Array.Fill(progress, (byte)' ');
        int c = 0;
        if (shown_name.Length < 16)
            for (c = 0; c < shown_name.Length; c++)
                progress[c] = shown_name[c];
        else
        {
            for (c = 0; c < 13; c++)
                progress[c] = shown_name[c];

            progress[13] = (byte)'.';
            progress[14] = (byte)'.';
            progress[15] = (byte)' ';
        }

        progress[16] = (byte)'[';
        progress[17] = 0;
        Console.WriteLine("%s", progress);
        for (c = 0; c < 50; c++) Console.WriteLine(".");
        Console.WriteLine("]\r");
        Console.WriteLine("%s", progress);

        /* read file and place ifs archive */
        long total_read = 0;
        long percent = 0;
        for (;;)
        {
            int compress_method = method;
            int last_percent = (int)percent;
            int bytes_read = ifs.Read(buffer, 0, BLOCK_SIZE);
            if (bytes_read == 0)
                break;
            total_read += bytes_read;

            /* for progress */
            if (fsize < (1 << 24))
                percent = total_read * 100 / fsize;
            else
                percent = total_read / 256 * 100 / (fsize >> 8);
            percent >>= 1;
            while (last_percent < (int)percent)
            {
                Console.Write("#");
                last_percent++;
            }

            /* too small, don't bother to compress */
            if (bytes_read < 32) compress_method = 0;

            /* write to output */
            switch (compress_method)
            {
                /* FastLZ */
                case 1:
                    chunk_size = FastLZ.Compress(buffer, 0, bytes_read, result, 0, level);
                    checksum = update_adler32(1L, result, chunk_size);
                    write_chunk_header(f, 17, 1, chunk_size, checksum, bytes_read);
                    f.Write(result, 0, chunk_size);
                    total_compressed += 16;
                    total_compressed += chunk_size;
                    break;

                /* uncompressed, also fallback method */
                case 0:
                default:
                    checksum = 1L;
                    checksum = update_adler32(checksum, buffer, bytes_read);
                    write_chunk_header(f, 17, 0, bytes_read, checksum, bytes_read);
                    f.Write(buffer, 0, bytes_read);
                    total_compressed += 16;
                    total_compressed += bytes_read;
                    break;
            }
        }

        if (total_read != fsize)
        {
            Console.WriteLine("");
            Console.WriteLine($"Error: reading {input_file} failed!");
            return -1;
        }
        else
        {
            Console.WriteLine("] ");
            if (total_compressed < fsize)
            {
                if (fsize < (1 << 20))
                    percent = total_compressed * 1000 / fsize;
                else
                    percent = total_compressed / 256 * 1000 / (fsize >> 8);
                percent = 1000 - percent;
                Console.WriteLine($"{(int)percent / 10:D2}.{(int)percent % 10:D1}%% saved");
            }

            Console.WriteLine("");
        }

        return 0;
    }

    public static FileStream OpenFile(string filePath, FileMode mode, FileAccess access = FileAccess.Read, FileShare share = FileShare.Read)
    {
        try
        {
            return new FileStream(filePath, mode, access, share);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }


    public static int pack_file(int compress_level, string input_file, string output_file)
    {
        int result;

        var testFs = OpenFile(output_file, FileMode.Open);
        if (null != testFs)
        {
            using var a = testFs;
            Console.WriteLine($"Error: file {output_file} already exists. Aborted.");
            return -1;
        }

        var tempFs = OpenFile(output_file, FileMode.CreateNew, FileAccess.Write, FileShare.ReadWrite);
        if (null == tempFs)
        {
            Console.WriteLine($"Error: could not create {output_file}. Aborted.");
            return -1;
        }

        using var f = tempFs;

        write_magic(f);
        result = pack_file_compressed(input_file, 1, compress_level, f);

        return result;
    }


    public static int Main(string[] args)
    {
        usage();
        return 0;
    }
}


//
// #ifdef SIXPACK_BENCHMARK_WIN32
// int benchmark_speed(int compress_level, const char* input_file);
//
// int benchmark_speed(int compress_level, const char* input_file) {
//   FILE* ifs;
//   unsigned long fsize;
//   unsigned long maxout;
//   const char* shown_name;
//   unsigned char* buffer;
//   unsigned char* result;
//   size_t bytes_read;
//
//   /* sanity check */
//   ifs = fopen(input_file, "rb");
//   if (!ifs) {
//     printf("Error: could not open %s\n", input_file);
//     return -1;
//   }
//
//   /* find size of the file */
//   fseek(ifs, 0, SEEK_END);
//   fsize = ftell(ifs);
//   fseek(ifs, 0, SEEK_SET);
//
//   /* already a 6pack archive? */
//   if (detect_magic(ifs)) {
//     printf("Error: no benchmark for 6pack archive!\n");
//     fclose(ifs);
//     return -1;
//   }
//
//   /* truncate directory prefix, e.g. "foo/bar/FILE.txt" becomes "FILE.txt" */
//   shown_name = input_file + strlen(input_file) - 1;
//   while (shown_name > input_file)
//     if (*(shown_name - 1) == PATH_SEPARATOR)
//       break;
//     else
//       shown_name--;
//
//   maxout = 1.05 * fsize;
//   maxout = (maxout < 66) ? 66 : maxout;
//   buffer = (unsigned char*)malloc(fsize);
//   result = (unsigned char*)malloc(maxout);
//   if (!buffer || !result) {
//     printf("Error: not enough memory!\n");
//     free(buffer);
//     free(result);
//     fclose(ifs);
//     return -1;
//   }
//
//   printf("Reading source file....\n");
//   bytes_read = fread(buffer, 1, fsize, ifs);
//   if (bytes_read != fsize) {
//     printf("Error reading file %s!\n", shown_name);
//     printf("Read %d bytes, expecting %d bytes\n", bytes_read, fsize);
//     free(buffer);
//     free(result);
//     fclose(ifs);
//     return -1;
//   }
//
//   /* shamelessly copied from QuickLZ 1.20 test program */
//   {
//     unsigned int j, y;
//     size_t i, u = 0;
//     double mbs, fastest;
//     unsigned long compressed_size;
//
//     printf("Setting HIGH_PRIORITY_CLASS...\n");
//     SetPriorityClass(GetCurrentProcess(), HIGH_PRIORITY_CLASS);
//
//     printf("Benchmarking FastLZ Level %d, please wait...\n", compress_level);
//
//     i = bytes_read;
//     fastest = 0.0;
//     for (j = 0; j < 3; j++) {
//       y = 0;
//       mbs = GetTickCount();
//       while (GetTickCount() == mbs)
//         ;
//       mbs = GetTickCount();
//       while (GetTickCount() - mbs < 3000) /* 1% accuracy with 18.2 timer */
//       {
//         u = fastlz_compress_level(compress_level, buffer, bytes_read, result);
//         y++;
//       }
//
//       mbs = ((double)i * (double)y) / ((double)(GetTickCount() - mbs) / 1000.) / 1000000.;
//       /*printf(" %.1f Mbyte/s  ", mbs);*/
//       if (fastest < mbs) fastest = mbs;
//     }
//
//     printf("\nCompressed %d bytes into %d bytes (%.1f%%) at %.1f Mbyte/s.\n", (unsigned int)i, (unsigned int)u,
//            (double)u / (double)i * 100., fastest);
//
// #if 1
//     fastest = 0.0;
//     compressed_size = u;
//     for (j = 0; j < 3; j++) {
//       y = 0;
//       mbs = GetTickCount();
//       while (GetTickCount() == mbs)
//         ;
//       mbs = GetTickCount();
//       while (GetTickCount() - mbs < 3000) /* 1% accuracy with 18.2 timer */
//       {
//         u = fastlz_decompress(result, compressed_size, buffer, bytes_read);
//         y++;
//       }
//
//       mbs = ((double)i * (double)y) / ((double)(GetTickCount() - mbs) / 1000.) / 1000000.;
//       /*printf(" %.1f Mbyte/s  ", mbs);*/
//       if (fastest < mbs) fastest = mbs;
//     }
//
//     printf("\nDecompressed at %.1f Mbyte/s.\n\n(1 MB = 1000000 byte)\n", fastest);
// #endif
//   }
//
//   fclose(ifs);
//   return 0;
// }
// #endif /* SIXPACK_BENCHMARK_WIN32 */
//
// int main(int argc, char** argv) {
//   int i;
//   int compress_level;
//   int benchmark;
//   char* input_file;
//   char* output_file;
//
//   /* show help with no argument at all*/
//   if (argc == 1) {
//     usage();
//     return 0;
//   }
//
//   /* default compression level, not the fastest */
//   compress_level = 2;
//
//   /* do benchmark only when explicitly specified */
//   benchmark = 0;
//
//   /* no file is specified */
//   input_file = 0;
//   output_file = 0;
//
//   for (i = 1; i <= argc; i++) {
//     char* argument = argv[i];
//
//     if (!argument) continue;
//
//     /* display help on usage */
//     if (!strcmp(argument, "-h") || !strcmp(argument, "--help")) {
//       usage();
//       return 0;
//     }
//
//     /* check for version information */
//     if (!strcmp(argument, "-v") || !strcmp(argument, "--version")) {
//       printf("6pack: high-speed file compression tool\n");
//       printf("Version %s (using FastLZ %s)\n", SIXPACK_VERSION_STRING, FASTLZ_VERSION_STRING);
//       printf("Copyright (C) Ariya Hidayat\n");
//       printf("\n");
//       return 0;
//     }
//
//     /* test compression speed? */
//     if (!strcmp(argument, "-mem")) {
//       benchmark = 1;
//       continue;
//     }
//
//     /* compression level */
//     if (!strcmp(argument, "-1") || !strcmp(argument, "--fastest")) {
//       compress_level = 1;
//       continue;
//     }
//     if (!strcmp(argument, "-2")) {
//       compress_level = 2;
//       continue;
//     }
//
//     /* unknown option */
//     if (argument[0] == '-') {
//       printf("Error: unknown option %s\n\n", argument);
//       printf("To get help on usage:\n");
//       printf("  6pack --help\n\n");
//       return -1;
//     }
//
//     /* first specified file is input */
//     if (!input_file) {
//       input_file = argument;
//       continue;
//     }
//
//     /* next specified file is output */
//     if (!output_file) {
//       output_file = argument;
//       continue;
//     }
//
//     /* files are already specified */
//     printf("Error: unknown option %s\n\n", argument);
//     printf("To get help on usage:\n");
//     printf("  6pack --help\n\n");
//     return -1;
//   }
//
//   if (!input_file) {
//     printf("Error: input file is not specified.\n\n");
//     printf("To get help on usage:\n");
//     printf("  6pack --help\n\n");
//     return -1;
//   }
//
//   if (!output_file && !benchmark) {
//     printf("Error: output file is not specified.\n\n");
//     printf("To get help on usage:\n");
//     printf("  6pack --help\n\n");
//     return -1;
//   }
//
// #ifdef SIXPACK_BENCHMARK_WIN32
//   if (benchmark)
//     return benchmark_speed(compress_level, input_file);
//   else
// #endif
//     return pack_file(compress_level, input_file, output_file);
//
//   /* unreachable */
//   return 0;
// }