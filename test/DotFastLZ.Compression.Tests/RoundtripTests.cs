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
using DotFastLZ.Compression.Tests.Fixtures;
using NUnit.Framework;

namespace DotFastLZ.Compression.Tests;

public class RoundtripTests
{
    private const string Prefix = "compression-corpus";

    private string[] _names = new[]
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
        "enwik/enwik8"
    };

    [SetUp]
    public void OnSetUp()
    {
        // ready zip files
        var sourceZipFiles = new[]
        {
            new SourceZip("canterburycorpus.zip", "canterbury"),
            new SourceZip("silesia.zip", "silesia"),
            new SourceZip("enwik8.zip", "enwik"),
        };

        // extract source files
        foreach (var sourceZip in sourceZipFiles)
        {
            sourceZip.Extract(Prefix);
        }
    }

    [Test]
    public void TestRoundtrip()
    {
        Console.WriteLine("Test reference decompressor for Level 1");
        foreach (var name in _names)
        {
            var filename = ResourceHelper.Find(Path.Combine(Prefix, name));
            bool result = TestHelper.test_ref_decompressor_level1(name, filename);
            Assert.That(result, Is.EqualTo(true), $"test_ref_decompressor_level1({name}, {filename})");
        }

        Console.WriteLine("");

        Console.WriteLine("Test reference decompressor for Level 2");
        foreach (var name in _names)
        {
            var filename = ResourceHelper.Find(Path.Combine(Prefix, name));
            bool result = TestHelper.test_ref_decompressor_level2(name, filename);
            Assert.That(result, Is.EqualTo(true), $"test_ref_decompressor_level2({name}, {filename})");
        }

        Console.WriteLine("");

        Console.WriteLine("Test round-trip for Level 1");
        foreach (var name in _names)
        {
            var filename = ResourceHelper.Find(Path.Combine(Prefix, name));
            bool result = TestHelper.test_roundtrip_level1(name, filename);
            Assert.That(result, Is.EqualTo(true), $"test_roundtrip_level1({name}, {filename})");
        }

        Console.WriteLine("");

        Console.WriteLine("Test round-trip for Level 2");
        foreach (var name in _names)
        {
            var filename = ResourceHelper.Find(Path.Combine(Prefix, name));
            var result = TestHelper.test_roundtrip_level2(name, filename);
            Assert.That(result, Is.EqualTo(true), $"test_roundtrip_level2({name}, {filename})");
        }

        Console.WriteLine("");
    }
}