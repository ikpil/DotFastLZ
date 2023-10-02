using System;
using System.Text;
using NUnit.Framework;

namespace DotFastLZ.Compression.Tests;

public class FastLZTests
{
    [Test]
    public void TestUsage()
    {
        // input source
        string text = "This is an example original text in English. ";

        // 최소 길이 2048자를 만족하도록 텍스트를 확장
        while (text.Length < 2048)
        {
            text += "Lorem ipsum dolor sit amet, consectetur adipiscing elit. \n";
        }

        var input = Encoding.UTF8.GetBytes(text);

        for (int level = 1; level <= 2; ++level)
        {
            // compress
            var estimateSize = FastLZ.EstimateCompressedSize(input.Length);
            var compressedBuffer = new byte[estimateSize];
            var compressedSize = FastLZ.CompressLevel(level, input, input.Length, compressedBuffer);

            // decompress
            byte[] decompressedBuffer = null;
            var decompressedSize = 0L;
            do
            {
                // guess
                long guessSize = null == decompressedBuffer 
                    ? compressedSize * 3 
                    : decompressedBuffer.Length * 3;
                
                decompressedBuffer = new byte[guessSize];
                decompressedSize = FastLZ.Decompress(compressedBuffer, compressedSize, decompressedBuffer, decompressedBuffer.Length);
                // ..
            } while (0 == decompressedSize && decompressedBuffer.Length < input.Length);

            // compare
            var compareSize = FastLZ.MemCompare(input, 0, decompressedBuffer, 0, decompressedSize);

            // check
            Assert.That(decompressedSize, Is.EqualTo(input.Length), "decompress size error");
            Assert.That(compareSize, Is.EqualTo(input.Length), "decompress compare error");
        }
    }
}