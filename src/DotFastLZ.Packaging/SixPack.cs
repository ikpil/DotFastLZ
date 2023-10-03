using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using DotFastLZ.Compression;

namespace DotFastLZ.Packaging
{
    public static class SixPack
    {
        public const int SIXPACK_VERSION_MAJOR = 0;
        public const int SIXPACK_VERSION_MINOR = 1;
        public const int SIXPACK_VERSION_REVISION = 0;

        public const string SIXPACK_VERSION_STRING = "0.1.0";

        //public const string SIXPACK_VERSION_STRING = "snapshot 20070615";
        public const string FASTLZ_EXTENSION = ".fastlz";

        /* magic identifier for 6pack file */
        public static readonly ReadOnlyMemory<byte> SIXPACK_MAGIC = new ReadOnlyMemory<byte>(new byte[] { 137, (byte)'6', (byte)'P', (byte)'K', 13, 10, 26, 10 });
        public const int BLOCK_SIZE = (2 * 64 * 1024);

        /* for Adler-32 checksum algorithm, see RFC 1950 Section 8.2 */
        public const int ADLER32_BASE = 65521;


        public static long GetTickCount64()
        {
            return unchecked((long)(Stopwatch.GetTimestamp() * ((double)TimeSpan.TicksPerSecond / Stopwatch.Frequency)));
        }

        // update_adler32
        public static ulong Adler32(ulong checksum, byte[] buf, long len)
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


        /* return non-zero if magic sequence is detected */
        /* warning: reset the read pointer to the beginning of the file */
        // detect_magic
        public static bool DetectMagicByFileStream(FileStream f)
        {
            byte[] buffer = new byte[8];

            f.Seek(0, SeekOrigin.Begin);
            var bytesRead = f.Read(buffer, 0, 8);
            f.Seek(0, SeekOrigin.Begin);

            return DetectMagic(buffer, 0);
        }

        public static bool DetectMagic(byte[] buffer, int offset)
        {
            if (0 > offset || buffer.Length - offset < 8)
            {
                return false;
            }

            for (int c = 0; c < 8; c++)
            {
                if (buffer[offset + c] != SIXPACK_MAGIC.Span[c])
                {
                    return false;
                }
            }

            return true;
        }

        // write_magic
        private static void WriteMagic(FileStream f)
        {
            f.Write(SIXPACK_MAGIC.Span);
        }

        // write_chunk_header
        private static void WriteChunkHeader(FileStream f, int id, int options, long size, ulong checksum, long extra)
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

        // read_chunk_header
        private static void ReadChunkHeader(FileStream f, out int id, out int options, out long size, out ulong checksum, out long extra)
        {
            byte[] buffer = new byte[16];
            f.Read(buffer, 0, 16);

            id = FastLZ.ReadUInt16(buffer, 0) & 0xffff;
            options = FastLZ.ReadUInt16(buffer, 2) & 0xffff;
            size = FastLZ.ReadUInt32(buffer, 4) & 0xffffffff;
            checksum = FastLZ.ReadUInt32(buffer, 8) & 0xffffffff;
            extra = FastLZ.ReadUInt32(buffer, 12) & 0xffffffff;
        }


        // pack_file_compressed
        private static int PackFileCompressed(string input_file, int method, int level, FileStream ofs, Action<string> logger)
        {
            ulong checksum;
            byte[] result = new byte[BLOCK_SIZE * 2]; /* FIXME twice is too large */

            /* sanity check */
            FileStream temp = OpenFile(input_file, FileMode.Open);
            if (null == temp)
            {
                logger?.Invoke($"Error: could not open {input_file}\n");
                return -1;
            }

            using var ifs = temp;

            /* find size of the file */
            ifs.Seek(0, SeekOrigin.End);
            long fsize = ifs.Position;
            ifs.Seek(0, SeekOrigin.Begin);

            /* already a 6pack archive? */
            if (DetectMagicByFileStream(ifs))
            {
                logger?.Invoke($"Error: file {input_file} is already a 6pack archive!\n");
                return -1;
            }

            /* truncate directory prefix, e.g. "foo/bar/FILE.txt" becomes "FILE.txt" */
            string fileName = GetFileName(input_file);
            byte[] utf8_shown_name = Encoding.UTF8.GetBytes(fileName);
            byte[] shown_name = new byte[utf8_shown_name.Length + 1]; // for cstyle
            Array.Fill(shown_name, (byte)0);
            Array.Copy(utf8_shown_name, shown_name, utf8_shown_name.Length);

            /* chunk for File Entry */
            byte[] buffer = new byte[BLOCK_SIZE];
            buffer[0] = (byte)(fsize & 255);
            buffer[1] = (byte)((fsize >> 8) & 255);
            buffer[2] = (byte)((fsize >> 16) & 255);
            buffer[3] = (byte)((fsize >> 24) & 255);
            buffer[4] = (byte)((fsize >> 32) & 255);
            buffer[5] = (byte)((fsize >> 40) & 255);
            buffer[6] = (byte)((fsize >> 48) & 255);
            buffer[7] = (byte)((fsize >> 56) & 255);
            buffer[8] = (byte)(shown_name.Length & 255); // filename length for lowest bit
            buffer[9] = (byte)(shown_name.Length >> 8); // filename length for highest bit

            checksum = 1L;
            checksum = Adler32(checksum, buffer, 10);
            checksum = Adler32(checksum, shown_name, shown_name.Length);
            WriteChunkHeader(ofs, 1, 0, 10 + shown_name.Length, checksum, 0);
            ofs.Write(buffer, 0, 10);
            ofs.Write(shown_name, 0, shown_name.Length);
            long total_compressed = 16 + 10 + shown_name.Length;

            /* for progress status */
            string progress;
            if (16 < fileName.Length)
            {
                progress = fileName.Substring(0, 13);
                progress += ".. ";
            }
            else
            {
                progress = fileName.PadRight(16, ' ');
            }


            logger?.Invoke($"{progress} [");
            for (int c = 0; c < 50; c++)
            {
                logger?.Invoke(".");
            }

            logger?.Invoke("]\r");
            logger?.Invoke($"{progress} [");

            /* read file and place ifs archive */
            long total_read = 0;
            long percent = 0;
            var beginTick = GetTickCount64();
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
                {
                    percent = total_read * 100 / fsize;
                }
                else
                {
                    percent = total_read / 256 * 100 / (fsize >> 8);
                }

                percent /= 2;
                while (last_percent < (int)percent)
                {
                    logger?.Invoke("#");
                    last_percent++;
                }

                /* too small, don't bother to compress */
                if (bytes_read < 32)
                {
                    compress_method = 0;
                }

                /* write to output */
                switch (compress_method)
                {
                    /* FastLZ */
                    case 1:
                    {
                        long chunkSize = FastLZ.CompressLevel(level, buffer, bytes_read, result);
                        checksum = Adler32(1L, result, chunkSize);
                        WriteChunkHeader(ofs, 17, 1, chunkSize, checksum, bytes_read);
                        ofs.Write(result, 0, (int)chunkSize);
                        total_compressed += 16;
                        total_compressed += chunkSize;
                    }
                        break;

                    /* uncompressed, also fallback method */
                    case 0:
                    default:
                    {
                        checksum = 1L;
                        checksum = Adler32(checksum, buffer, bytes_read);
                        WriteChunkHeader(ofs, 17, 0, bytes_read, checksum, bytes_read);
                        ofs.Write(buffer, 0, bytes_read);
                        total_compressed += 16;
                        total_compressed += bytes_read;
                    }
                        break;
                }
            }

            if (total_read != fsize)
            {
                logger?.Invoke("\n");
                logger?.Invoke($"Error: reading {input_file} failed!\n");
                return -1;
            }
            else
            {
                logger?.Invoke("] ");
                if (total_compressed < fsize)
                {
                    if (fsize < (1 << 20))
                    {
                        percent = total_compressed * 1000 / fsize;
                    }
                    else
                    {
                        percent = total_compressed / 256 * 1000 / (fsize >> 8);
                    }

                    percent = 1000 - percent;

                    var elapsedTicks = (GetTickCount64() - beginTick);
                    var elapsedMs = elapsedTicks / TimeSpan.TicksPerMillisecond;
                    var elapsedMicro = elapsedTicks / (TimeSpan.TicksPerMillisecond / 1000);
                    logger?.Invoke($"{(int)percent / 10:D2}.{(int)percent % 10:D1}% saved - {elapsedMs} ms, {elapsedMicro} micro");
                }

                logger?.Invoke("\n");
            }

            return 0;
        }


        // pack_file
        public static int PackFile(int compress_level, string input_file, string output_file, Action<string> logger = null)
        {
            int result;

            var fs = OpenFile(output_file, FileMode.Open);
            if (null != fs)
            {
                fs.Dispose();
                logger?.Invoke($"Error: file {output_file} already exists. Aborted.\n");
                return -1;
            }

            fs = OpenFile(output_file, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            if (null == fs)
            {
                logger?.Invoke($"Error: could not create {output_file}. Aborted.\n");
                return -1;
            }

            using var ofs = fs;

            WriteMagic(ofs);
            result = PackFileCompressed(input_file, 1, compress_level, ofs, logger);
            return result;
        }

        // unpack_file
        public static int UnpackFile(string input_file, Action<string> logger = null)
        {
            ulong checksum;

            /* sanity check */
            var tempFs = OpenFile(input_file, FileMode.Open);
            if (null == tempFs)
            {
                logger?.Invoke($"Error: could not open {input_file}\n");
                return -1;
            }

            using var ifs = tempFs;

            /* find size of the file */
            ifs.Seek(0, SeekOrigin.End);
            long fsize = ifs.Position;
            ifs.Seek(0, SeekOrigin.Begin);

            /* not a 6pack archive? */
            if (!DetectMagicByFileStream(ifs))
            {
                logger?.Invoke($"Error: file {input_file} is not a 6pack archive!\n");
                return -1;
            }

            logger?.Invoke($"Archive: {input_file}");

            /* position of first chunk */
            ifs.Seek(8, SeekOrigin.Begin);

            /* initialize */
            string output_file = string.Empty;
            FileStream ofs = null;
            long total_extracted = 0;
            long decompressed_size = 0;
            long percent = 0;

            byte[] buffer = new byte[BLOCK_SIZE];
            byte[] compressed_buffer = null;
            byte[] decompressed_buffer = null;
            long compressed_bufsize = 0;
            long decompressed_bufsize = 0;

            /* main loop */
            for (;;)
            {
                /* end of file? */
                long pos = ifs.Position;
                if (pos >= fsize)
                {
                    break;
                }

                ReadChunkHeader(
                    ifs,
                    out var chunk_id,
                    out var chunk_options,
                    out var chunk_size,
                    out var chunk_checksum,
                    out var chunk_extra
                );

                if (chunk_id == 1 && chunk_size > 10 && chunk_size < BLOCK_SIZE)
                {
                    /* close current file, if any */
                    logger?.Invoke("\n");
                    if (null != ofs)
                    {
                        ofs.Close();
                        ofs = null;
                    }

                    /* file entry */
                    ifs.Read(buffer, 0, (int)chunk_size);
                    checksum = Adler32(1L, buffer, chunk_size);
                    if (checksum != chunk_checksum)
                    {
                        logger?.Invoke("\nError: checksum mismatch!\n");
                        logger?.Invoke($"Got {checksum:X8} Expecting {chunk_checksum:X8}\n");
                        return -1;
                    }


                    decompressed_size = FastLZ.ReadUInt32(buffer, 0);
                    total_extracted = 0;
                    percent = 0;

                    /* get file to extract */
                    int name_length = FastLZ.ReadUInt16(buffer, 8);
                    output_file = Encoding.UTF8.GetString(buffer, 10, name_length - 1);
                    output_file = output_file.Trim();

                    /* check if already exists */
                    ofs = OpenFile(output_file, FileMode.Open);
                    if (null != ofs)
                    {
                        ofs.Close();
                        ofs = null;
                        logger?.Invoke($"File {output_file} already exists. Skipped.\n");
                    }
                    else
                    {
                        /* create the file */
                        ofs = OpenFile(output_file, FileMode.CreateNew, FileAccess.Write, FileShare.Write);
                        if (null == ofs)
                        {
                            logger?.Invoke($"Can't create file {output_file} Skipped.\n");
                        }
                        else
                        {
                            /* for progress status */
                            logger?.Invoke("\n");
                            string progress;
                            if (16 < output_file.Length)
                            {
                                progress = output_file.Substring(0, 13);
                                progress += ".. ";
                            }
                            else
                            {
                                progress = output_file.PadRight(16, ' ');
                            }


                            logger?.Invoke($"{progress} [");
                            for (int c = 0; c < 50; c++)
                            {
                                logger?.Invoke(".");
                            }

                            logger?.Invoke("]\r");
                            logger?.Invoke($"{progress} [");
                        }
                    }
                }

                if ((chunk_id == 17) && null != ofs && !string.IsNullOrEmpty(output_file) && 0 < decompressed_size)
                {
                    long remaining;

                    /* uncompressed */
                    switch (chunk_options)
                    {
                        /* stored, simply copy to output */
                        case 0:
                        {
                            /* read one block at at time, write and update checksum */
                            total_extracted += chunk_size;
                            remaining = chunk_size;
                            checksum = 1L;
                            for (;;)
                            {
                                long r = (BLOCK_SIZE < remaining) ? BLOCK_SIZE : remaining;
                                long bytes_read = ifs.Read(buffer, 0, (int)r);
                                if (0 >= bytes_read)
                                {
                                    break;
                                }

                                ofs.Write(buffer, 0, (int)bytes_read);
                                checksum = Adler32(checksum, buffer, bytes_read);
                                remaining -= bytes_read;
                            }

                            /* verify everything is written correctly */
                            if (checksum != chunk_checksum)
                            {
                                logger?.Invoke("\nError: checksum mismatch. Aborted.\n");
                                logger?.Invoke($"Got {checksum:X8} Expecting {chunk_checksum:X8}\n");
                            }
                        }
                            break;

                        /* compressed using FastLZ */
                        case 1:
                        {
                            /* enlarge input buffer if necessary */
                            if (chunk_size > compressed_bufsize)
                            {
                                compressed_bufsize = chunk_size;
                                compressed_buffer = new byte[compressed_bufsize];
                            }

                            /* enlarge output buffer if necessary */
                            if (chunk_extra > decompressed_bufsize)
                            {
                                decompressed_bufsize = chunk_extra;
                                decompressed_buffer = new byte[decompressed_bufsize];
                            }

                            /* read and check checksum */
                            ifs.Read(compressed_buffer, 0, (int)chunk_size);
                            checksum = Adler32(1L, compressed_buffer, chunk_size);
                            total_extracted += chunk_extra;

                            /* verify that the chunk data is correct */
                            if (checksum != chunk_checksum)
                            {
                                logger?.Invoke("\nError: checksum mismatch. Skipped.\n");
                                logger?.Invoke($"Got {checksum:X8} Expecting {chunk_checksum:X8}\n");
                            }
                            else
                            {
                                /* decompress and verify */
                                remaining = FastLZ.Decompress(compressed_buffer, chunk_size, decompressed_buffer, chunk_extra);
                                if (remaining != chunk_extra)
                                {
                                    logger?.Invoke("\nError: decompression failed. Skipped.\n");
                                }
                                else
                                {
                                    ofs.Write(decompressed_buffer, 0, (int)chunk_extra);
                                }
                            }
                        }
                            break;

                        default:
                            logger?.Invoke($"\nError: unknown compression method ({chunk_options})\n");
                            break;
                    }

                    /* for progress, if everything is fine */
                    //if (null != f)
                    {
                        int last_percent = (int)percent;
                        if (decompressed_size < (1 << 24))
                        {
                            percent = total_extracted * 100 / decompressed_size;
                        }
                        else
                        {
                            percent = total_extracted / 256 * 100 / (decompressed_size >> 8);
                        }

                        percent >>= 1;
                        while (last_percent < (int)percent)
                        {
                            logger?.Invoke("#");
                            last_percent++;
                        }

                        if (total_extracted == decompressed_size)
                        {
                            logger?.Invoke($"]\n");
                        }
                    }
                }

                /* position of next chunk */
                ifs.Seek(pos + 16 + chunk_size, SeekOrigin.Begin);
            }

            logger?.Invoke("\n");
            logger?.Invoke("\n");

            /* close working files */
            if (null != ofs)
            {
                ofs.Close();
            }

            /* so far so good */
            return 0;
        }

        // benchmark_speed
        public static int BenchmarkSpeed(int compress_level, string input_file, Action<string> logger = null)
        {
            /* sanity check */
            var fs = OpenFile(input_file, FileMode.Open);
            if (null == fs)
            {
                logger?.Invoke($"Error: could not open {input_file}\n");
                return -1;
            }

            using var ifs = fs;

            /* find size of the file */
            ifs.Seek(0, SeekOrigin.End);
            var fsize = ifs.Position;
            ifs.Seek(0, SeekOrigin.Begin);

            /* already a 6pack archive? */
            if (DetectMagicByFileStream(ifs))
            {
                logger?.Invoke("Error: no benchmark for 6pack archive!\n");
                return -1;
            }

            /* truncate directory prefix, e.g. "foo/bar/FILE.txt" becomes "FILE.txt" */
            var shown_name = GetFileName(input_file);

            long maxout = (long)(1.05d * fsize);
            maxout = (maxout < 66) ? 66 : maxout;
            byte[] buffer = new byte[fsize];
            byte[] result = new byte[maxout];

            /* for benchmark */
            // if (null == buffer || null == result)
            // {
            //     logger?.Invoke("Error: not enough memory!\n\n");
            //     return -1;
            // }

            logger?.Invoke("Reading source file....\n");
            int bytes_read = ifs.Read(buffer, 0, (int)fsize);
            if (bytes_read != fsize)
            {
                logger?.Invoke($"Error reading file {shown_name}!\n");
                logger?.Invoke($"Read {bytes_read} bytes, expecting {fsize} bytes\n");
                return -1;
            }

            /* shamelessly copied from QuickLZ 1.20 test program */
            {

                // multi platform error
                // logger?.Invoke("Setting HIGH_PRIORITY_CLASS...\n\n");
                // {
                //      Process currentProcess = Process.GetCurrentProcess();
                //      currentProcess.PriorityClass = ProcessPriorityClass.High;
                // }

                logger?.Invoke($"Benchmarking FastLZ Level {compress_level}, please wait...\n");

                long u = 0;
                long fastest = 0;
                long curTicks;
                for (int j = 0; j < 3; j++)
                {
                    int y = 0;
                    curTicks = GetTickCount64();
                    while (GetTickCount64() == curTicks)
                    {
                    }

                    curTicks = GetTickCount64();
                    while (GetTickCount64() - curTicks < 3000) /* 1% accuracy with 18.2 timer */
                    {
                        u = FastLZ.CompressLevel(compress_level, buffer, bytes_read, result);
                        y++;
                    }


                    long mbs = (bytes_read * y) / ((GetTickCount64() - curTicks) / TimeSpan.TicksPerMillisecond);
                    if (fastest < mbs)
                    {
                        fastest = mbs;
                    }
                }

                logger?.Invoke($"Compressed {bytes_read} bytes into {u} bytes ({(u * 100.0d / bytes_read):F1}%) at {fastest / (double)1000:F1} Mbyte/s.\n");

                fastest = 0;
                long compressed_size = u;
                for (int j = 0; j < 3; j++)
                {
                    int y = 0;
                    curTicks = GetTickCount64();
                    while (GetTickCount64() == curTicks)
                    {
                    }

                    curTicks = GetTickCount64();
                    while (GetTickCount64() - curTicks < 3000) /* 1% accuracy with 18.2 timer */
                    {
                        u = FastLZ.Decompress(result, compressed_size, buffer, bytes_read);
                        y++;
                    }

                    long mbs = (bytes_read * y) / ((GetTickCount64() - curTicks) / TimeSpan.TicksPerMillisecond);
                    if (fastest < mbs)
                    {
                        fastest = mbs;
                    }
                }

                logger?.Invoke($"\nDecompressed at {fastest / (double)1000:F1} Mbyte/s.\n\n(1 MB = 1000000 byte)\n");
            }

            return 0;
        }

        public static string GetFileName(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return string.Empty;
            }

            /* truncate directory prefix, e.g. "foo/bar/FILE.txt" becomes "FILE.txt" */
            return path
                .Split(new char[] { '/', '\\', Path.DirectorySeparatorChar, }, StringSplitOptions.RemoveEmptyEntries)
                .Last();
        }

        public static FileStream OpenFile(string filePath, FileMode mode, FileAccess access = FileAccess.Read, FileShare share = FileShare.Read)
        {
            try
            {
                return new FileStream(filePath, mode, access, share);
            }
            catch (Exception /* e */)
            {
                return null;
            }
        }
    }
}