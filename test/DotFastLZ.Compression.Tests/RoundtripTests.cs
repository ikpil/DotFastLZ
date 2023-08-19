/*
  FastLZ - Byte-aligned LZ77 compression library
  Copyright (C) 2005-2020 Ariya Hidayat <ariya.hidayat@gmail.com> https://github.com/ariya/FastLZ
  jfastlz library written by William Kinney https://code.google.com/p/jfastlz
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
using System.Diagnostics;
using System.IO;
using System.Linq;
using DotFastLZ.Compression.Tests.Fixtures;
using NUnit.Framework;

namespace DotFastLZ.Compression.Tests;

public class RoundtripTests
{
    private const string Prefix = "compression-corpus";
    private const int MAX_FILE_SIZE = 100 * 1024 * 1024;
    //private const int MAX_FILE_SIZE = 32 * 1024 * 1024;

    private string[] _names;

    private long compare(string name, byte[] a, byte[] b, long size)
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


    // /* prototype, implemented in refimpl.c */
    // void REF_Level1_decompress(const uint8_t* input, int length, uint8_t* output);
    // void REF_Level2_decompress(const uint8_t* input, int length, uint8_t* output);

    /*
      Same as test_roundtrip_level1 EXCEPT that the decompression is carried out
      using the highly-simplified, unoptimized vanilla reference decompressor.
    */
    private bool test_ref_decompressor_level1(string name, string file_name)
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
        int compressed_size = FastLZ.Compress(file_buffer, 0, (int)file_size, compressed_buffer, 0, 1);
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
        //REF_Level1_decompress(compressed_buffer, compressed_size, uncompressed_buffer);

        Console.WriteLine("Comparing. Please wait...");
        long result = compare(file_name, file_buffer, uncompressed_buffer, file_size);
        if (0 <= result)
        {
            return false;
        }

        Console.WriteLine($"{name} {file_size} -> {compressed_size} ({ratio})");
        return true;
    }


    /*
      Same as test_roundtrip_level2 EXCEPT that the decompression is carried out
      using the highly-simplified, unoptimized vanilla reference decompressor.
    */

    private bool test_ref_decompressor_level2(string name, string file_name)
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
        int compressed_size = FastLZ.Compress(file_buffer, 0, (int)file_size, compressed_buffer, 0, 2);
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

        //REF_Level2_decompress(compressed_buffer, compressed_size, uncompressed_buffer);

        Console.WriteLine("Comparing. Please wait...");
        long result = compare(file_name, file_buffer, uncompressed_buffer, file_size);
        if (0 <= result)
        {
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
    private bool test_roundtrip_level1(string name, string file_name)
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
        int compressed_size = FastLZ.Compress(file_buffer, 0, (int)file_size, compressed_buffer, 0, 1);
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

        // fastlz_decompress(compressed_buffer, compressed_size, uncompressed_buffer, file_size);

        Console.WriteLine("Comparing. Please wait...");
        long result = compare(file_name, file_buffer, uncompressed_buffer, file_size);
        if (0 <= result)
        {
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
    bool test_roundtrip_level2(string name, string file_name)
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
        int compressed_size = FastLZ.Compress(file_buffer, 0, (int)file_size, compressed_buffer, 0, 2);
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

        // fastlz_decompress(compressed_buffer, compressed_size, uncompressed_buffer, file_size);

        Console.WriteLine("Comparing. Please wait...");
        long result = compare(file_name, file_buffer, uncompressed_buffer, file_size);
        if (0 <= result)
        {
            return false;
        }

        Console.WriteLine($"{name} {file_size} -> {compressed_size} ({ratio})");

        return true;
    }


    [SetUp]
    public void Setup()
    {
        _names = new[]
        {
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
            "enwik/enwik8.txt"
        };
    }

    [Test]
    public void TestRoundtrip()
    {
        var sourceZipFiles = new[]
        {
            new SourceZip("canterburycorpus.zip", "canterbury"),
            new SourceZip("silesia.zip", "silesia"),
            new SourceZip("enwik8.zip", "enwik"),
        };

        foreach (var sourceZip in sourceZipFiles)
        {
            sourceZip.Extract(Prefix);
        }

        Console.WriteLine("Test reference decompressor for Level 1");
        foreach (var name in _names)
        {
            var filename = ResourceHelper.Find(Path.Combine(Prefix, name));
            test_ref_decompressor_level1(name, filename);
        }

        Console.WriteLine("");

        Console.WriteLine("Test reference decompressor for Level 2");
        foreach (var name in _names)
        {
            var filename = ResourceHelper.Find(Path.Combine(Prefix, name));
            test_ref_decompressor_level2(name, filename);
        }

        Console.WriteLine("");

        Console.WriteLine("Test round-trip for Level 1");
        foreach (var name in _names)
        {
            var filename = ResourceHelper.Find(Path.Combine(Prefix, name));
            test_roundtrip_level1(name, filename);
        }

        Console.WriteLine("");

        Console.WriteLine("Test round-trip for Level 2");
        foreach (var name in _names)
        {
            var filename = ResourceHelper.Find(Path.Combine(Prefix, name));
            test_roundtrip_level2(name, filename);
        }

        Console.WriteLine("");
    }
}