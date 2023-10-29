using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using DotFastLZ.Compression.Tests.Fixtures;
using DotFastLZ.Resource;
using NUnit.Framework;

namespace DotFastLZ.Packaging.Tests;

public class SixPackTests
{
    [Test]
    public void TestAdler32()
    {
        var text = "hello. I am ikpil";
        var input = Encoding.UTF8.GetBytes(text);
        var adler32Sum = SixPack.Adler32(1L, input, input.Length);
        var hash = adler32Sum.ToString("x");
        Assert.That(hash, Is.EqualTo("33b405d3"));
    }

    [Test]
    public void TestDetectMagic()
    {
        var source = SixPack.SIXPACK_MAGIC
            .ToArray()
            .Select(x => x)
            .ToList();

        var copied = new List<byte>();
        copied.Add(1); // add head(1)
        copied.AddRange(source);
        copied.Add(2); // add tail(2);

        Assert.That(SixPack.DetectMagic(copied.ToArray(), 0), Is.EqualTo(false));
        Assert.That(SixPack.DetectMagic(copied.ToArray(), 1), Is.EqualTo(true));
        Assert.That(SixPack.DetectMagic(copied.ToArray(), 2), Is.EqualTo(false));
        Assert.That(SixPack.DetectMagic(copied.ToArray(), 3), Is.EqualTo(false));

        Assert.That(SixPack.DetectMagic(copied.ToArray(), copied.Count + 1), Is.EqualTo(false));
    }

    [Test]
    public void TestPackAndUnpack()
    {
        const string filename = "6pack-packing-test.txt";
        string fastlz1 = filename + ".fastlz1";
        string fastlz2 = filename + ".fastlz2";

        File.Delete(filename);
        File.Delete(fastlz1);
        File.Delete(fastlz2);

        R.GenerateFile(filename, 1024 * 1024);

        // pack
        SixPack.PackFile(1, filename, fastlz1, Console.Write);
        SixPack.PackFile(2, filename, fastlz2, Console.Write);

        var sourceMd5 = R.ComputeMD5(filename);
        File.Delete(filename);
        Assert.That(sourceMd5, Is.EqualTo("90e4a4b78ebf7f88b02b0054ab0d6daa"));
        Assert.That(File.Exists(filename), Is.EqualTo(false));

        // checksum
        Assert.That(R.ComputeMD5(fastlz1), Is.EqualTo("6ca821bdf187f12bf23552133dfa99a1"));
        Assert.That(R.ComputeMD5(fastlz2), Is.EqualTo("c70d787ea842eba36b7d1479b94c6740"));

        // unpack level1
        {
            int status1 = SixPack.UnpackFile(fastlz1, Console.Write);
            var decompress1Md5 = R.ComputeMD5(filename);
            File.Delete(filename);
            File.Delete(fastlz1);

            Assert.That(status1, Is.EqualTo(0));
            Assert.That(decompress1Md5, Is.EqualTo(sourceMd5));
            Assert.That(File.Exists(filename), Is.EqualTo(false));
        }

        // unpack level2
        {
            int status2 = SixPack.UnpackFile(fastlz2, Console.Write);
            var decompress2Md5 = R.ComputeMD5(filename);

            File.Delete(filename);
            File.Delete(fastlz2);

            Assert.That(status2, Is.EqualTo(0));
            Assert.That(decompress2Md5, Is.EqualTo(sourceMd5));
            Assert.That(File.Exists(filename), Is.EqualTo(false));
        }
    }

    [Test]
    public void TestBenchmarkSpeed()
    {
        const string benchmarkFileName = "benchmark.txt";
        File.Delete(benchmarkFileName);

        R.GenerateFile(benchmarkFileName, 1024 * 1024 * 8);

        int status1 = SixPack.BenchmarkSpeed(1, benchmarkFileName, Console.Write);
        int status2 = SixPack.BenchmarkSpeed(2, benchmarkFileName, Console.Write);

        Assert.That(status1, Is.EqualTo(0));
        Assert.That(status2, Is.EqualTo(0));
    }

    [Test]
    public void TestGetFileName()
    {
        Assert.That(SixPack.GetFileName("c:/abc.exe"), Is.EqualTo("abc.exe"));
        Assert.That(SixPack.GetFileName("c:\\abc.exe"), Is.EqualTo("abc.exe"));
        Assert.That(SixPack.GetFileName("c://abc .exe"), Is.EqualTo("abc .exe"));
        Assert.That(SixPack.GetFileName("abcd/edfg/ abc .exe"), Is.EqualTo(" abc .exe"));
        Assert.That(SixPack.GetFileName("ad//aabb// abc .exe"), Is.EqualTo(" abc .exe"));
        Assert.That(SixPack.GetFileName("aa\\\\addda\\\\ abc .exe"), Is.EqualTo(" abc .exe"));
        Assert.That(SixPack.GetFileName("c////dab// abc .exe"), Is.EqualTo(" abc .exe"));
        Assert.That(SixPack.GetFileName("c/ abc .exe"), Is.EqualTo(" abc .exe"));
    }

    [Test]
    public void TestOpenFile()
    {
        const string filename = "open-file-test.txt";
        File.Delete(filename);

        Assert.That(SixPack.OpenFile(filename, FileMode.Open), Is.Null);
        Assert.That(SixPack.OpenFile(filename, FileMode.Create, FileAccess.Write), Is.Not.Null);
    }
}