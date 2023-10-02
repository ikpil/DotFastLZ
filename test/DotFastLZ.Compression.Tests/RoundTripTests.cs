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

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class RoundTripTests
{
    [OneTimeSetUp]
    public void OnSetUp()
    {
        ResourceHelper.ExtractAll();
    }

    [OneTimeTearDown]
    public void OnTearDown()
    {
        // remove
        ResourceHelper.RemoveAll();
    }

    [Test]
    public void TestRefDecompressorLevel1()
    {
        Console.WriteLine("Test reference decompressor for Level 1");
        foreach (var name in ResourceHelper.TestFiles)
        {
            var filename = ResourceHelper.Find(Path.Combine(ResourceHelper.Prefix, name));
            bool result = TestHelper.test_ref_decompressor_level1(name, filename);
            Assert.That(result, Is.EqualTo(true), $"test_ref_decompressor_level1({name}, {filename})");
        }
    }

    [Test]
    public void TestRefDecompressorLevel2()
    {
        Console.WriteLine("Test reference decompressor for Level 2");
        foreach (var name in ResourceHelper.TestFiles)
        {
            var filename = ResourceHelper.Find(Path.Combine(ResourceHelper.Prefix, name));
            bool result = TestHelper.test_ref_decompressor_level2(name, filename);
            Assert.That(result, Is.EqualTo(true), $"test_ref_decompressor_level2({name}, {filename})");
        }
    }


    [Test]
    public void TestRoundtripLevel1()
    {
        Console.WriteLine("Test round-trip for Level 1");
        foreach (var name in ResourceHelper.TestFiles)
        {
            var filename = ResourceHelper.Find(Path.Combine(ResourceHelper.Prefix, name));
            bool result = TestHelper.test_roundtrip_level1(name, filename);
            Assert.That(result, Is.EqualTo(true), $"test_roundtrip_level1({name}, {filename})");
        }
    }

    [Test]
    public void TestRoundtripLevel2()
    {
        Console.WriteLine("Test round-trip for Level 2");
        foreach (var name in ResourceHelper.TestFiles)
        {
            var filename = ResourceHelper.Find(Path.Combine(ResourceHelper.Prefix, name));
            var result = TestHelper.test_roundtrip_level2(name, filename);
            Assert.That(result, Is.EqualTo(true), $"test_roundtrip_level2({name}, {filename})");
        }
    }
}