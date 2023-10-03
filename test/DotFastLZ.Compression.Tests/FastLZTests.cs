using System;
using System.Text;
using NUnit.Framework;

namespace DotFastLZ.Compression.Tests;

public class FastLZTests
{
    public static byte[] GetInputSource()
    {
        // input source
        string text = "This is an example original text in English. ";

        // extend the text to meet a minimum length of 2048 characters
        while (text.Length < 2048)
        {
            text += "Lorem ipsum dolor sit amet, consectetur adipiscing elit. \n";
        }

        return Encoding.UTF8.GetBytes(text);
    }

    [Test]
    public void TestUsage()
    {
        for (int level = 1; level <= 2; ++level)
        {
            // compress
            var input = GetInputSource();
            var estimateSize = FastLZ.EstimateCompressedSize(input.Length);
            var comBuf = new byte[estimateSize];
            var comBufSize = FastLZ.CompressLevel(level, input, input.Length, comBuf);

            // decompress
            byte[] decBuf = new byte[input.Length];
            var decBufSize = FastLZ.Decompress(comBuf, comBufSize, decBuf, decBuf.Length);

            // compare
            var compareSize = FastLZ.MemCompare(input, 0, decBuf, 0, decBufSize);

            // check
            Assert.That(decBufSize, Is.EqualTo(input.Length), "decompress size error");
            Assert.That(compareSize, Is.EqualTo(input.Length), "decompress compare error");
        }
    }
}