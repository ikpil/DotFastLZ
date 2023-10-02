using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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

        GenerateFile(filename, 1024 * 1024);

        // pack
        SixPack.PackFile(1, filename, fastlz1);
        SixPack.PackFile(2, filename, fastlz2);

        var sourceMd5 = ComputeMD5(filename);
        File.Delete(filename);
        Assert.That(sourceMd5, Is.EqualTo("0e3618ab09fb7e1989a05da990b2911a"));
        Assert.That(File.Exists(filename), Is.EqualTo(false));

        // checksum
        Assert.That(ComputeMD5(fastlz1), Is.EqualTo("07017027a344938392152e47e5389c34"));
        Assert.That(ComputeMD5(fastlz2), Is.EqualTo("945b4b347e9b5bd43e86e3b41be09e8b"));

        // unpack level1
        {
            int status1 = SixPack.UnpackFile(fastlz1);
            var decompress1Md5 = ComputeMD5(filename);
            File.Delete(filename);
            File.Delete(fastlz1);

            Assert.That(status1, Is.EqualTo(0));
            Assert.That(decompress1Md5, Is.EqualTo(sourceMd5));
            Assert.That(File.Exists(filename), Is.EqualTo(false));
        }

        // unpack level2
        {
            int status2 = SixPack.UnpackFile(fastlz2);
            var decompress2Md5 = ComputeMD5(filename);

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

        GenerateFile(benchmarkFileName, 1024 * 1024 * 8);

        int status1 = SixPack.BenchmarkSpeed(1, benchmarkFileName);
        int status2 = SixPack.BenchmarkSpeed(2, benchmarkFileName);

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

    public long GenerateFile(string filename, long size)
    {
        var text = "About Adler32 Checksum Calculator The Adler32 Checksum Calculator will compute an Adler32 checksum of string. " +
                   "Adler32 is a checksum algorithm that was invented by Mark Adler. " +
                   "In contrast to a cyclic redundancy check (CRC) of the same length, it trades reliability for speed.";

        var bytes = Encoding.UTF8.GetBytes(text);

        using var fs = SixPack.OpenFile(filename, FileMode.Create, FileAccess.Write);

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
}