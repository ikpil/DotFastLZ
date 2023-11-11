using System;
using System.IO;
using System.IO.Compression;

namespace DotFastLZ.Benchmark;

public class BenchmarkSystemZip : IBenchmark
{
    public string Name => _name;

    private string _name;
    private CompressionLevel _level;

    public BenchmarkSystemZip(CompressionLevel level)
    {
        _level = level;
        _name = $"System.Io.Zip {_level.ToString()}";
    }

    public BenchmarkResult Start(string filename, byte[] srcBytes, byte[] dstBytes)
    {
        return Benchmarks.Start(Name, filename, srcBytes, dstBytes, (s, d) => CompressZip(s, d, _level), DecompressZip);
    }

    private static long CompressZip(byte[] srcBytes, byte[] dstBytes, CompressionLevel level)
    {
        using var ms = new MemoryStream();
        using (var zip = new ZipArchive(ms, ZipArchiveMode.Create))
        {
            var entry = zip.CreateEntry("test", level);
            using var s = entry.Open();
            s.Write(srcBytes, 0, srcBytes.Length);
        }

        var ssss = ms.ToArray().AsSpan();
        ssss.CopyTo(dstBytes);
        return ssss.Length;
    }

    private static long DecompressZip(byte[] srcBytes, long size, byte[] dstBytes)
    {
        using var readStream = new MemoryStream(srcBytes, 0, (int)size);
        using var zip = new ZipArchive(readStream, ZipArchiveMode.Read);
        var entry = zip.Entries[0];
        using var entryStream = entry.Open();
        using MemoryStream writeStream = new MemoryStream(dstBytes);
        entryStream.CopyTo(writeStream);
        return writeStream.Position;
    }
}