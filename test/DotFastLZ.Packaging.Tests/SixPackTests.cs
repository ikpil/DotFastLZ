using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    public void TestPackFile()
    {
    }

    [Test]
    public void TestUnpackFile()
    {
    }

    [Test]
    public void TestBenchmarkSpeed()
    {
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
        Assert.That(SixPack.OpenFile("aaaaa.dll", FileMode.Open), Is.Null);
        Assert.That(SixPack.OpenFile("aaaaa.dll", FileMode.Create, FileAccess.Write), Is.Not.Null);
    }
}