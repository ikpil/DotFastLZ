using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using DotFastLZ.Compression;
using DotFastLZ.Resource;
using K4os.Compression.LZ4;

namespace DotFastLZ.Benchmark;

public static class Program
{
    public static void Main(string[] args)
    {
        try
        {
            R.ExtractAll();
            foreach (var file in R.SourceFiles)
            {
                var results = BenchmarkFile(file);
                Print($"Benchmark : compression {file}", results);
                //break;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            R.DeleteAll();
        }
    }

    private static List<BenchmarkResult> BenchmarkFile(string file)
    {
        var results = new List<BenchmarkResult>();

        var filepath = R.Find(Path.Combine(R.Prefix, file));
        var srcBytes = R.ToBytes(filepath);
        var dstBytes = new byte[srcBytes.Length * 2];

        // memcpy
        var memoryCopy = Benchmark($"memcpy", srcBytes.ToArray(), dstBytes, CompressMemcpy, DecompressMemcpy);
        results.Add(memoryCopy);

        // DotFastLZ fastlz
        var fastlzLv1 = Benchmark($"DotFastLZ.Compression L1", srcBytes.ToArray(), dstBytes, CompressDotFastLZL1, DecompressDotFastLZ);
        var fastlzLv2 = Benchmark($"DotFastLZ.Compression L2", srcBytes.ToArray(), dstBytes, CompressDotFastLZL2, DecompressDotFastLZ);
        results.Add(fastlzLv1);
        results.Add(fastlzLv2);

        // K4os LZ4
        var k4osL01 = Benchmark($"K4os.Compression.LZ4 L00", srcBytes.ToArray(), dstBytes,
            (s, d) => CompressK4osLZ4(s, d, LZ4Level.L00_FAST), DecompressK4osLZ4);
        var k4osL12 = Benchmark($"K4os.Compression.LZ4 L12", srcBytes.ToArray(), dstBytes,
            (s, d) => CompressK4osLZ4(s, d, LZ4Level.L12_MAX), DecompressK4osLZ4);
        var k4osL03HC = Benchmark($"K4os.Compression.LZ4 L03_HC", srcBytes.ToArray(), dstBytes,
            (s, d) => CompressK4osLZ4(s, d, LZ4Level.L03_HC), DecompressK4osLZ4);
        var k4osL09HC = Benchmark($"K4os.Compression.LZ4 L09_HC", srcBytes.ToArray(), dstBytes,
            (s, d) => CompressK4osLZ4(s, d, LZ4Level.L09_HC), DecompressK4osLZ4);
        results.Add(k4osL01);
        results.Add(k4osL12);
        results.Add(k4osL03HC);
        results.Add(k4osL09HC);

        var zipFastest = Benchmark($"System.IO.Compression.ZipArchive Fastest", srcBytes.ToArray(), dstBytes,
            (s, d) => CompressZip(s, d, CompressionLevel.Fastest), DecompressZip);
        var zipOptimal = Benchmark($"System.IO.Compression.ZipArchive Optimal", srcBytes.ToArray(), dstBytes,
            (s, d) => CompressZip(s, d, CompressionLevel.Optimal), DecompressZip);
        results.Add(zipFastest);
        results.Add(zipOptimal);

        return results;
    }

    private static BenchmarkResult Benchmark(string name, byte[] srcBytes, byte[] dstBytes, Func<byte[], byte[], long> compress, Func<byte[], long, byte[], long> decompress)
    {
        long compressionSourceByteLength = srcBytes.Length;

        var result = new BenchmarkResult();
        result.Name = name;
        result.Compression.ElapsedWatch = new Stopwatch();
        result.Compression.ElapsedWatch.Start();
        for (int i = 0; i < 5; ++i)
        {
            long size = compress.Invoke(srcBytes, dstBytes);
            result.Compression.InputBytes += compressionSourceByteLength;
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


    private static long CompressMemcpy(byte[] srcBytes, byte[] dstBytes)
    {
        srcBytes.CopyTo(new Span<byte>(dstBytes));
        return srcBytes.Length;
    }

    private static long DecompressMemcpy(byte[] srcBytes, long size, byte[] dstBytes)
    {
        srcBytes.AsSpan(0, (int)size).CopyTo(new Span<byte>(dstBytes));
        return size;
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

    private static long CompressDotFastLZL1(byte[] srcBytes, byte[] dstBytes)
    {
        return FastLZ.CompressLevel1(srcBytes, 0, srcBytes.Length, dstBytes);
    }

    private static long CompressDotFastLZL2(byte[] srcBytes, byte[] dstBytes)
    {
        return FastLZ.CompressLevel2(srcBytes, 0, srcBytes.Length, dstBytes);
    }

    private static long DecompressDotFastLZ(byte[] srcBytes, long size, byte[] dstBytes)
    {
        return FastLZ.Decompress(srcBytes, size, dstBytes, dstBytes.Length);
    }

    private static long CompressK4osLZ4(byte[] srcBytes, byte[] dstBytes, LZ4Level level)
    {
        var writer = new FixedArrayBufferWriter<byte>(dstBytes);
        LZ4Pickler.Pickle(srcBytes, writer, level);
        return writer.WrittenCount;
    }

    private static long DecompressK4osLZ4(byte[] srcBytes, long size, byte[] dstBytes)
    {
        var writer = new FixedArrayBufferWriter<byte>(dstBytes);
        LZ4Pickler.Unpickle(srcBytes.AsSpan(0, (int)size), writer);
        return writer.WrittenCount;
    }

    private static void Print(string headline, List<BenchmarkResult> results)
    {
        var rows = results
            .OrderByDescending(x => x.ComputeTotalSpeed())
            .ToList();


        // 각 열의 최대 길이를 찾기 위한 작업
        int[] widths = new int[BenchmarkResult.CollSize];
        for (int i = 0; i < rows.Count; i++)
        {
            widths[0] = Math.Max(rows[i].Name.Length, widths[0]);
            widths[1] = Math.Max(rows[i].Compression.ToSpeedString().Length, widths[1]);
            widths[2] = Math.Max(rows[i].Decompression.ToSpeedString().Length, widths[2]);
            widths[3] = Math.Max(rows[i].Compression.ToRateString().Length, widths[3]);
        }

        var headName = "Name";
        var headCompMbs = "Comp. MB/s";
        var headDecompMbs = "Decomp. MB/s";
        var headRate = "Rate";
        
        widths[0] = Math.Max(widths[0], headName.Length) + 1;
        widths[1] = Math.Max(widths[1], headCompMbs.Length) + 1;
        widths[2] = Math.Max(widths[2], headDecompMbs.Length) + 1;
        widths[3] = Math.Max(widths[3], headRate.Length) + 1;

        Console.WriteLine();
        Console.WriteLine($"### {headline} ###");
        Console.WriteLine();



        // 표 출력
        Console.WriteLine("| " +
                          headName + new string(' ', widths[0] - headName.Length) + "| " +
                          headCompMbs + new string(' ', widths[1] - headCompMbs.Length) + "| " +
                          headDecompMbs + new string(' ', widths[2] - headDecompMbs.Length) + "| " +
                          headRate + new string(' ', widths[3] - headRate.Length) + "|");
        Console.WriteLine("|-" +
                          new string('-', widths[0]) + "|-" +
                          new string('-', widths[1]) + "|-" +
                          new string('-', widths[2]) + "|-" +
                          new string('-', widths[3]) + "|");

        foreach (var row in rows)
        {
            Console.WriteLine(
                "| " +
                row.Name.PadRight(widths[0]) + "| " +
                row.Compression.ToSpeedString().PadRight(widths[1]) + "| " +
                row.Decompression.ToSpeedString().PadRight(widths[2]) + "| " +
                row.Compression.ToRateString().PadRight(widths[3]) + "|"
            );
        }
    }
}