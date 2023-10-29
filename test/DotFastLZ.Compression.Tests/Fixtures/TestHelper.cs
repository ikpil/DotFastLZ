/*
  FastLZ - Byte-aligned LZ77 compression library
  Copyright (C) 2005-2020 Ariya Hidayat <ariya.hidayat@gmail.com> https://github.com/ariya/FastLZ
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
using DotFastLZ.Resource;

namespace DotFastLZ.Compression.Tests.Fixtures;

public static class TestHelper
{
    private const int MAX_FILE_SIZE = 100 * 1024 * 1024;
    //private const int MAX_FILE_SIZE = 32 * 1024 * 1024;

    // /* prototype, implemented in refimpl.c */
    // void REF_Level1_decompress(const uint8_t* input, int length, uint8_t* output);
    // void REF_Level2_decompress(const uint8_t* input, int length, uint8_t* output);

    /*
      Same as test_roundtrip_level1 EXCEPT that the decompression is carried out
      using the highly-simplified, unoptimized vanilla reference decompressor.
    */
    public static bool test_ref_decompressor_level1(string name, string file_name)
    {
        Console.WriteLine($"Processing {name}...");
        if (!File.Exists(file_name))
        {
            Console.WriteLine($"Error: can not open {file_name}!");
            return false;
        }

        var fileInfo = new FileInfo(file_name);
        var file_size = fileInfo.Length;
        Console.WriteLine($"Size is {file_size} bytes.");

        if (file_size > MAX_FILE_SIZE)
        {
            Console.WriteLine($"{name} {file_size} [skipped, file too big]");
            return false;
        }

        using var fs = new FileStream(file_name, FileMode.Open, FileAccess.Read, FileShare.Read);
        byte[] file_buffer = new byte[file_size];
        int read = fs.Read(file_buffer, 0, file_buffer.Length);
        if (read != file_size)
        {
            Console.WriteLine($"Error: only read {read} bytes!");
            return false;
        }

        Console.WriteLine("Compressing. Please wait...");
        byte[] compressed_buffer = new byte[(int)(1.05 * file_size)];
        long compressed_size = FastLZ.CompressLevel(1, file_buffer, file_size, compressed_buffer);
        double ratio = (100.0 * compressed_size) / file_size;
        Console.WriteLine($"Compressing was completed: {file_size} -> {compressed_size} ({ratio})");

        Console.WriteLine("Decompressing. Please wait...");
        byte[] uncompressed_buffer = new byte[file_size];
        if (null == uncompressed_buffer)
        {
            Console.WriteLine($"{name} {file_size} -> {compressed_size}  ({ratio})  skipped, can't decompress");
            return false;
        }

        Array.Fill(uncompressed_buffer, (byte)'-');
        RefImpl.REF_Level1_decompress(compressed_buffer, compressed_size, uncompressed_buffer);

        Console.WriteLine("Comparing. Please wait...");
        long result = R.Compare(file_name, file_buffer, uncompressed_buffer, file_size);
        if (0 <= result)
        {
            Console.WriteLine($"failed to compare - {name} index({result})");
            return false;
        }

        Console.WriteLine($"{name} {file_size} -> {compressed_size} ({ratio})");
        return true;
    }


    /*
      Same as test_roundtrip_level2 EXCEPT that the decompression is carried out
      using the highly-simplified, unoptimized vanilla reference decompressor.
    */
    public static bool test_ref_decompressor_level2(string name, string file_name)
    {
        Console.WriteLine($"Processing {name}...");
        if (!File.Exists(file_name))
        {
            Console.WriteLine($"Error: can not open {file_name}!");
            return false;
        }

        var fileInfo = new FileInfo(file_name);
        var file_size = fileInfo.Length;
        Console.WriteLine($"Size is {file_size} bytes.");

        if (file_size > MAX_FILE_SIZE)
        {
            Console.WriteLine($"{name} {file_size} [skipped, file too big]");
            return false;
        }

        using var fs = new FileStream(file_name, FileMode.Open, FileAccess.Read, FileShare.Read);

        byte[] file_buffer = new byte[file_size];
        int read = fs.Read(file_buffer, 0, file_buffer.Length);
        if (read != file_size)
        {
            Console.WriteLine($"Error: only read {read} bytes!");
            return false;
        }

        Console.WriteLine("Compressing. Please wait...");
        byte[] compressed_buffer = new byte[(int)(1.05 * file_size)];
        long compressed_size = FastLZ.CompressLevel(2, file_buffer, file_size, compressed_buffer);
        double ratio = (100.0 * compressed_size) / file_size;
        Console.WriteLine($"Compressing was completed: {file_size} -> {compressed_size} ({ratio})");

        Console.WriteLine("Decompressing. Please wait...");
        byte[] uncompressed_buffer = new byte[file_size];
        if (null == uncompressed_buffer)
        {
            Console.WriteLine($"{name} {file_size} -> {compressed_size}  ({ratio})  skipped, can't decompress");
            return false;
        }

        Array.Fill(uncompressed_buffer, (byte)'-');

        /* intentionally mask out the block tag */
        compressed_buffer[0] = (byte)(compressed_buffer[0] & 31);

        RefImpl.REF_Level2_decompress(compressed_buffer, compressed_size, uncompressed_buffer);

        Console.WriteLine("Comparing. Please wait...");
        long result = R.Compare(file_name, file_buffer, uncompressed_buffer, file_size);
        if (0 <= result)
        {
            Console.WriteLine($"failed to compare - {name} index({result})");
            return false;
        }

        Console.WriteLine($"{name} {file_size} -> {compressed_size} ({ratio})");

        return true;
    }

    /*
      Read the content of the file.
      Compress it first using the Level 1 compressor.
      Decompress the output with Level 1 decompressor.
      Compare the result with the original file content.
    */
    public static bool test_roundtrip_level1(string name, string file_name)
    {
        Console.WriteLine($"Processing {name}...");
        if (!File.Exists(file_name))
        {
            Console.WriteLine($"Error: can not open {file_name}!");
            return false;
        }

        var fileInfo = new FileInfo(file_name);
        var file_size = fileInfo.Length;
        Console.WriteLine($"Size is {file_size} bytes.");

        if (file_size > MAX_FILE_SIZE)
        {
            Console.WriteLine($"{name} {file_size} [skipped, file too big]");
            return false;
        }

        using var fs = new FileStream(file_name, FileMode.Open, FileAccess.Read, FileShare.Read);
        byte[] file_buffer = new byte[file_size];
        int read = fs.Read(file_buffer, 0, file_buffer.Length);
        if (read != file_size)
        {
            Console.WriteLine($"Error: only read {read} bytes!");
            return false;
        }

        Console.WriteLine("Compressing. Please wait...");

        byte[] compressed_buffer = new byte[(int)(1.05 * file_size)];
        //int compressed_size = FastLZ.Compress(file_buffer, 0, (int)file_size, compressed_buffer, 0, 1);
        long compressed_size = FastLZ.CompressLevel1(file_buffer, 0, file_size, compressed_buffer);
        double ratio = (100.0 * compressed_size) / file_size;
        Console.WriteLine($"Compressing was completed: {file_size} -> {compressed_size} ({ratio})");

        Console.WriteLine("Decompressing. Please wait...");
        byte[] uncompressed_buffer = new byte[file_size];
        if (null == uncompressed_buffer)
        {
            Console.WriteLine($"{name} {file_size} -> {compressed_size}  ({ratio})  skipped, can't decompress");
            return false;
        }

        Array.Fill(uncompressed_buffer, (byte)'-');

        FastLZ.DecompressLevel1(compressed_buffer, 0, compressed_size, uncompressed_buffer, 0, uncompressed_buffer.Length);

        Console.WriteLine("Comparing. Please wait...");
        long result = R.Compare(file_name, file_buffer, uncompressed_buffer, file_size);
        if (0 <= result)
        {
            Console.WriteLine($"failed to compare - {name} index({result})");
            return false;
        }

        Console.WriteLine($"{name} {file_size} -> {compressed_size} ({ratio})");

        return true;
    }

    /*
      Read the content of the file.
      Compress it first using the Level 2 compressor.
      Decompress the output with Level 2 decompressor.
      Compare the result with the original file content.
    */
    public static bool test_roundtrip_level2(string name, string file_name)
    {
        Console.WriteLine($"Processing {name}...");
        if (!File.Exists(file_name))
        {
            Console.WriteLine($"Error: can not open {file_name}!");
            return false;
        }

        var fileInfo = new FileInfo(file_name);
        var file_size = fileInfo.Length;
        Console.WriteLine($"Size is {file_size} bytes.");

        if (file_size > MAX_FILE_SIZE)
        {
            Console.WriteLine($"{name} {file_size} [skipped, file too big]");
            return false;
        }

        using var fs = new FileStream(file_name, FileMode.Open, FileAccess.Read, FileShare.Read);
        byte[] file_buffer = new byte[file_size];
        int read = fs.Read(file_buffer, 0, file_buffer.Length);
        if (read != file_size)
        {
            Console.WriteLine($"Error: only read {read} bytes!");
            return false;
        }

        Console.WriteLine("Compressing. Please wait...");

        byte[] compressed_buffer = new byte[(int)(1.05 * file_size)];
        //int compressed_size = FastLZ.Compress(file_buffer, 0, (int)file_size, compressed_buffer, 0, 2);
        long compressed_size = FastLZ.CompressLevel2(file_buffer, 0, file_size, compressed_buffer);
        double ratio = (100.0 * compressed_size) / file_size;
        Console.WriteLine($"Compressing was completed: {file_size} -> {compressed_size} ({ratio})");

        Console.WriteLine("Decompressing. Please wait...");
        byte[] uncompressed_buffer = new byte[file_size];
        if (null == uncompressed_buffer)
        {
            Console.WriteLine($"{name} {file_size} -> {compressed_size}  ({ratio})  skipped, can't decompress");
            return false;
        }

        Array.Fill(uncompressed_buffer, (byte)'-');

        //FastLZ.Decompress(compressed_buffer, 0, compressed_buffer.Length, uncompressed_buffer, 0, uncompressed_buffer.Length);
        FastLZ.DecompressLevel2(compressed_buffer, 0, compressed_size, uncompressed_buffer, 0, uncompressed_buffer.Length);

        Console.WriteLine("Comparing. Please wait...");
        long result = R.Compare(file_name, file_buffer, uncompressed_buffer, file_size);
        if (0 <= result)
        {
            Console.WriteLine($"failed to compare - {name} index({result})");
            return false;
        }

        Console.WriteLine($"{name} {file_size} -> {compressed_size} ({ratio})");

        return true;
    }
}