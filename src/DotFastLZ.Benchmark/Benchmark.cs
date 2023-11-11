using System;
using System.Diagnostics;

namespace DotFastLZ.Benchmark;

public static class Benchmarks
{
    public static BenchmarkResult Start(string name, string filename, byte[] srcBytes, byte[] dstBytes, Func<byte[], byte[], long> compress, Func<byte[], long, byte[], long> decompress)
    {
        var result = new BenchmarkResult();
        result.Name = name;
        result.FileName = filename;
        result.SourceByteLength = srcBytes.Length;
        result.Compression.ElapsedWatch = new Stopwatch();
        result.Compression.ElapsedWatch.Start();
        for (int i = 0; i < 5; ++i)
        {
            long size = compress.Invoke(srcBytes, dstBytes);
            result.Compression.InputBytes += result.SourceByteLength;
            result.Compression.OutputBytes += size;
            result.Times += 1;
        }

        result.Compression.ElapsedWatch.Stop();

        var decompInputLength = result.Compression.OutputBytes / result.Times;
        //dstBytes.AsSpan(0, (int)(compressedResult.DestBytes / compressedResult.Times)).CopyTo(srcBytes);

        result.Decompression.ElapsedWatch = new Stopwatch();
        result.Decompression.ElapsedWatch.Start();
        for (int i = 0; i < 5; ++i)
        {
            long size = decompress.Invoke(dstBytes, decompInputLength, srcBytes);
            result.Decompression.InputBytes += decompInputLength;
            result.Decompression.OutputBytes += size;
            result.Times += 1;
        }

        result.Decompression.ElapsedWatch.Stop();

        //Console.WriteLine(result.ToString());
        return result;
    }
}

public class Benchmark
{
    public virtual string Name => "unknown";

    public virtual BenchmarkResult Start(string filename, byte[] srcBytes, byte[] dstBytes)
    {
        return null;
    }
}

public class BenchmarkMemCopy : Benchmark
{
    public override string Name => "memcpy";

    //var memoryCopy = Benchmark($"memcpy", filename, srcBytes.ToArray(), dstBytes, CompressMemcpy, DecompressMemcpy);

    public BenchmarkMemCopy()
    {
    }

    public override BenchmarkResult Start(string filename, byte[] srcBytes, byte[] dstBytes)
    {
        return Benchmarks.Start(Name, filename, srcBytes, dstBytes, CompressMemcpy, DecompressMemcpy);
    }


    public static long CompressMemcpy(byte[] srcBytes, byte[] dstBytes)
    {
        srcBytes.CopyTo(new Span<byte>(dstBytes));
        return srcBytes.Length;
    }

    public static long DecompressMemcpy(byte[] srcBytes, long size, byte[] dstBytes)
    {
        srcBytes.AsSpan(0, (int)size).CopyTo(new Span<byte>(dstBytes));
        return size;
    }
}