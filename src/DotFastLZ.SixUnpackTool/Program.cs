/*
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
using System.Collections.Generic;
using DotFastLZ.Compression;
using DotFastLZ.Package;

namespace DotFastLZ.SixUnpack;

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

        var archiveFiles = new List<string>();
        for (int i = 0; i < args.Length; i++)
        {
            var argument = args[i].Trim();
            if (string.IsNullOrEmpty(argument))
                continue;

            /* check for help on usage */
            if (argument == "-h" || argument == "--help")
            {
                Usage();
                return 0;
            }

            /* check for version information */
            if (argument == "-v" || argument == "--version")
            {
                Console.WriteLine("6unpack: high-speed file compression tool");
                Console.WriteLine($"Version {SixPack.SIXPACK_VERSION_STRING} (using FastLZ {FastLZ.FASTLZ_VERSION_REVISION})");
                Console.WriteLine("");
                return 0;
            }
            
            archiveFiles.Add(argument);
        }

        for (int i = 0; i < archiveFiles.Count; ++i)
        {
            var archiveFile = archiveFiles[i];
            unpack_file(archiveFile);
        }

        return 0;
    }

    private static int unpack_file(string s)
    {
        return 0;
    }

    private static void Usage()
    {
        Console.WriteLine("6unpack: uncompress 6pack archive");
        Console.WriteLine("Copyright (C) Ariya Hidayat, Choi Ikpil(ikpil@naver.com)");
        Console.WriteLine(" - https://github.com/ikpil/DotFastLZ");
        Console.WriteLine("");
        Console.WriteLine("Usage: 6unpack archive-file");
        Console.WriteLine("");
    }
}