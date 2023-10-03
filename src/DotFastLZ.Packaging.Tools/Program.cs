﻿/*
  6PACK - file compressor using FastLZ (lightning-fast compression library)
  Copyright (C) 2007-2020 Ariya Hidayat <ariya.hidayat@gmail.com>
  Copyright (C) 2023 Choi Ikpil <ikpil@naver.com> https://github.com/ikpil/DotFastLz

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
using DotFastLZ.Compression;
using DotFastLZ.Packaging;

namespace DotFastLZ.Packaging.Tools;

public static class Program
{
    public static int Main(string[] args)
    {
        /* show help with no argument at all*/
        if (args.Length == 0)
        {
            Usage();
            return 0;
        }

        /* default compression level, not the fastest */
        int compress_level = 2;

        /* do benchmark only when explicitly specified */
        bool benchmark = false;

        bool decompress = false;

        /* no file is specified */
        string input_file = string.Empty;
        string output_file = string.Empty;

        for (int i = 0; i < args.Length; i++)
        {
            var argument = args[i].Trim();
            if (string.IsNullOrEmpty(argument))
                continue;

            /* display help on usage */
            if (argument == "-h" || argument == "--help")
            {
                Usage();
                return 0;
            }

            /* check for version information */
            if (argument == "-v" || argument == "--version")
            {
                Console.WriteLine("6pack: high-speed file compression tool");
                Console.WriteLine($"Version {SixPack.SIXPACK_VERSION_STRING} (using FastLZ {FastLZ.FASTLZ_VERSION_STRING})");
                Console.WriteLine("");
                return 0;
            }

            /* test compression speed? */
            if (argument == "-mem")
            {
                benchmark = true;
                continue;
            }

            /* compression level */
            if (argument == "-1" || argument == "--fastest")
            {
                compress_level = 1;
                continue;
            }

            if (argument == "-2")
            {
                compress_level = 2;
                continue;
            }

            if (argument == "-d")
            {
                decompress = true;
                continue;
            }

            /* unknown option */
            if (argument[0] == '-')
            {
                Console.WriteLine($"Error: unknown option {argument}\n");
                Console.WriteLine("To get help on usage:\n");
                Console.WriteLine("  6pack --help\n");
                return -1;
            }

            /* first specified file is input */
            if (string.IsNullOrEmpty(input_file))
            {
                input_file = argument;
                continue;
            }

            /* next specified file is output */
            if (string.IsNullOrEmpty(output_file))
            {
                output_file = argument;
                continue;
            }

            /* files are already specified */
            Console.WriteLine($"Error: unknown option {argument}\n");
            Console.WriteLine("To get help on usage:");
            Console.WriteLine("  6pack --help\n");
            return -1;
        }

        if (string.IsNullOrEmpty(input_file))
        {
            Console.WriteLine("Error: input file is not specified.\n");
            Console.WriteLine("To get help on usage:");
            Console.WriteLine("  6pack --help\n");
            return -1;
        }

        if (SixPack.FASTLZ_EXTENSION == Path.GetExtension(input_file))
        {
            decompress = true;
        }

        if (string.IsNullOrEmpty(output_file) && !benchmark && !decompress)
        {
            output_file = input_file + SixPack.FASTLZ_EXTENSION;
        }

        if (benchmark)
        {
            return SixPack.BenchmarkSpeed(compress_level, input_file, Console.Write);
        }

        if (decompress)
        {
            return SixPack.UnpackFile(input_file, Console.Write);
        }

        return SixPack.PackFile(compress_level, input_file, output_file, Console.Write);
    }

    private static void Usage()
    {
        Console.WriteLine($"6pack: high-speed file compression tool");
        Console.WriteLine($"Copyright (C) Ariya Hidayat, Choi Ikpil(ikpil@naver.com)");
        Console.WriteLine($" - https://github.com/ikpil/DotFastLZ");
        Console.WriteLine($"");
        Console.WriteLine($"Usage: 6pack [options] input-file output-file");
        Console.WriteLine($"");
        Console.WriteLine($"Options:");
        Console.WriteLine($"  -1    compress faster");
        Console.WriteLine($"  -2    compress better");
        Console.WriteLine($"  -v    show program version");
        Console.WriteLine($"  -d    decompression (default for {SixPack.FASTLZ_EXTENSION} extension)");
        Console.WriteLine($"  -mem  check in-memory compression speed");
        Console.WriteLine($"");
    }
}