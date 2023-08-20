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
using System.IO;
using System.IO.Compression;

namespace DotFastLZ.Compression.Tests.Fixtures;

public static class ResourceHelper
{
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

    public static int ExtractZipFile(string zipFilePath, string extractPath)
    {
        int count = 0;
        try
        {
            if (!Directory.Exists(extractPath))
                Directory.CreateDirectory(extractPath);

            using var archive = ZipFile.OpenRead(zipFilePath);
            foreach (ZipArchiveEntry entry in archive.Entries)
            {
                string entryPath = Path.Combine(extractPath, entry.FullName);

                if (entry.FullName.EndsWith("/"))
                {
                    Directory.CreateDirectory(entryPath);
                }
                else
                {
                    entry.ExtractToFile(entryPath, true);
                    Console.WriteLine($"extract file - {entryPath}");
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
}