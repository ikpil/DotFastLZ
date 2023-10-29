/*
  Copyright (C) 2023 Choi Ikpil <ikpil@naver.com> https://github.com/ikpil/DotFastLZ

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
using System.Collections.Immutable;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;

namespace DotFastLZ.Resource;

public static class R
{
    public const string Prefix = "compression-corpus";

    // ready zip files
    public static readonly ImmutableArray<SourceZip> SourceZipFiles = ImmutableArray.Create(
        new SourceZip("canterburycorpus.zip", "canterbury"),
        new SourceZip("silesia.zip", "silesia"),
        new SourceZip("enwik8.zip", "enwik")
    );

    public static readonly ImmutableArray<string> TestFiles = ImmutableArray.Create(
        "canterbury/alice29.txt",
        "canterbury/asyoulik.txt",
        "canterbury/cp.html",
        "canterbury/fields.c",
        "canterbury/grammar.lsp",
        "canterbury/kennedy.xls",
        "canterbury/lcet10.txt",
        "canterbury/plrabn12.txt",
        "canterbury/ptt5",
        "canterbury/sum",
        "canterbury/xargs.1",
        "silesia/dickens",
        "silesia/mozilla",
        "silesia/mr",
        "silesia/nci",
        "silesia/ooffice",
        "silesia/osdb",
        "silesia/reymont",
        "silesia/samba",
        "silesia/sao",
        "silesia/webster",
        "silesia/x-ray",
        "silesia/xml",
        "enwik/enwik8"
    );


    public static byte[] ToBytes(string filename)
    {
        var filepath = Find(filename);
        using var fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
        byte[] buffer = new byte[fs.Length];
        fs.Read(buffer, 0, buffer.Length);

        return buffer;
    }

    public static string Find(string pathName)
    {
        string path = Path.Combine("resources", pathName);
        for (int i = 0; i < 10; ++i)
        {
            if (File.Exists(path) || Directory.Exists(path))
            {
                return Path.GetFullPath(path);
            }

            path = Path.Combine("..", path);
        }

        return Path.GetFullPath(pathName);
    }

    public static int ExtractZipFile(string zipFilePath, string destDir)
    {
        int count = 0;
        try
        {
            if (string.IsNullOrEmpty(destDir))
                destDir = string.Empty;

            if (!Directory.Exists(destDir))
            {
                Directory.CreateDirectory(destDir);
            }

            using var archive = ZipFile.OpenRead(zipFilePath);
            foreach (ZipArchiveEntry entry in archive.Entries)
            {
                // https://cwe.mitre.org/data/definitions/22.html
                string fullDestDirPath = Path.GetFullPath(destDir + Path.DirectorySeparatorChar);
                string destFileName = Path.GetFullPath(Path.Combine(destDir, entry.FullName));
                if (!destFileName.StartsWith(fullDestDirPath))
                {
                    throw new InvalidOperationException($"Entry is outside the target dir: {destFileName}");
                }

                if (entry.FullName.EndsWith("/"))
                {
                    Directory.CreateDirectory(destFileName);
                }
                else
                {
                    entry.ExtractToFile(destFileName, true);
                    Console.WriteLine($"extract file - {destFileName}");
                }

                count += 1;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return count;
    }

    public static long Compare(string name, byte[] a, byte[] b, long size)
    {
        for (long i = 0; i < size; ++i)
        {
            if (a[i] != b[i])
            {
                Console.WriteLine($"Error on {name}!");
                Console.WriteLine($"Different at index {i}: expecting {a[i]},actual {b[i]}");
                return i;
            }
        }

        return -1;
    }

    public static long GenerateFile(string filename, long size)
    {
        var text = "About Adler32 Checksum Calculator The Adler32 Checksum Calculator will compute an Adler32 checksum of string. " +
                   "Adler32 is a checksum algorithm that was invented by Mark Adler. " +
                   "In contrast to a cyclic redundancy check (CRC) of the same length, it trades reliability for speed.";

        var bytes = Encoding.UTF8.GetBytes(text);

        using var fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.Read);

        var count = size / bytes.Length;
        for (int i = 0; i < count; ++i)
        {
            fs.Write(bytes);
        }

        count = size % bytes.Length;
        if (0 < count)
        {
            fs.Write(bytes, 0, (int)count);
        }

        return size;
    }

    public static string ComputeMD5(string filePath)
    {
        using var md5 = MD5.Create();
        using var stream = File.OpenRead(filePath);
        byte[] hashBytes = md5.ComputeHash(stream);
        StringBuilder sb = new StringBuilder();

        foreach (byte b in hashBytes)
        {
            sb.Append(b.ToString("x2"));
        }

        return sb.ToString();
    }

    public static void ExtractAll()
    {
        // extract source files
        foreach (var sourceZip in SourceZipFiles)
        {
            sourceZip.Extract(Prefix);
        }
    }

    public static void DeleteAll()
    {
        var path = Find(Prefix);
        if (!string.IsNullOrEmpty(path))
        {
            Directory.Delete(path, true);
        }
    }
}