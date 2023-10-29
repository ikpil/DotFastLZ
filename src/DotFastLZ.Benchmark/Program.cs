using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using DotFastLZ.Compression;
using DotFastLZ.Resource;
using K4os.Compression.LZ4;

namespace DotFastLZ.Benchmark;

public static class Program
{
    public static void Main(string[] args)
    {
        var results = new List<BenchmarkResult>();
        try
        {
            R.ExtractAll();
            foreach (var file in R.SourceFiles)
            {
                var filepath = R.Find(Path.Combine(R.Prefix, file));
                var srcBytes = R.ToBytes(filepath);
                var dstBytes = new byte[srcBytes.Length * 2];

                // DotFastLZ fastlz
                var fastlzLv1 = Benchmark($"DotFastLZ.Compression compression L1 - {file}", srcBytes, dstBytes, CompressFastLZLv1);
                var fastlzLv2 = Benchmark($"DotFastLZ.Compression compression L2 - {file}", srcBytes, dstBytes, CompressFastLZLv2);
                results.Add(fastlzLv1);
                results.Add(fastlzLv2);

                // K4os LZ4
                var k4osL01 = Benchmark($"K4os.Compression.LZ4 compression L00 - {file}", srcBytes, dstBytes, (s, d) => CompressK4osLZ4(s, d, LZ4Level.L00_FAST));
                var k4osL12 = Benchmark($"K4os.Compression.LZ4 compression L12 - {file}", srcBytes, dstBytes, (s, d) => CompressK4osLZ4(s, d, LZ4Level.L12_MAX));
                var k4osL03HC = Benchmark($"K4os.Compression.LZ4 compression L03_HC - {file}", srcBytes, dstBytes, (s, d) => CompressK4osLZ4(s, d, LZ4Level.L03_HC));
                var k4osL09HC = Benchmark($"K4os.Compression.LZ4 compression L09_HC - {file}", srcBytes, dstBytes, (s, d) => CompressK4osLZ4(s, d, LZ4Level.L09_HC));
                results.Add(k4osL01);
                results.Add(k4osL12);
                results.Add(k4osL03HC);
                results.Add(k4osL09HC);
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

        Print(results);
    }


    public static BenchmarkResult Benchmark(string name, byte[] srcBytes, byte[] dstBytes, Func<byte[], byte[], long> bench)
    {
        var result = new BenchmarkResult();
        result.Name = name;
        result.Watch = new Stopwatch();
        result.Watch.Start();
        for (int i = 0; i < 5; ++i)
        {
            long size = bench.Invoke(srcBytes, dstBytes);
            result.SourceBytes += srcBytes.Length;
            result.DestBytes += size;
            result.Times += 1;
        }

        result.Watch.Stop();
        Console.WriteLine(result.ToString());
        return result;
    }

    private static long CompressFastLZLv1(byte[] srcBytes, byte[] dstBytes)
    {
        return FastLZ.CompressLevel1(srcBytes, 0, srcBytes.Length, dstBytes);
    }

    private static long CompressFastLZLv2(byte[] srcBytes, byte[] dstBytes)
    {
        return FastLZ.CompressLevel2(srcBytes, 0, srcBytes.Length, dstBytes);
    }

    private static long CompressK4osLZ4(byte[] srcBytes, byte[] dstBytes, LZ4Level level)
    {
        var writer = new FixedArrayBufferWriter<byte>(dstBytes);
        LZ4Pickler.Pickle(srcBytes, writer, level);
        return writer.WrittenCount;
    }

    private static void Print(List<BenchmarkResult> results)
    {
        var sorted = results.OrderByDescending(x => x.ComputeSpeed()).ToList();

        // 각 열의 최대 길이를 찾기 위한 작업
        int[] widths = new int[3];
        for (int i = 0; i < sorted.Count; i++)
        {
            widths[0] = Math.Max(sorted[i].Name.Length, widths[0]);
            widths[1] = Math.Max(sorted[i].ToRateString().Length, widths[1]);
            widths[2] = Math.Max(sorted[i].ToSpeedString().Length, widths[2]);
        }

        widths[0] += 1;
        widths[1] += 1;
        widths[2] += 1;

        // 표 출력
        Console.WriteLine("Name" + new string(' ', widths[0] - 4) + "| " +
                          "Rate" + new string(' ', widths[1] - 4) + "| " +
                          "MB/s");
        Console.WriteLine(new string('-', widths[0]) + "| " + new string('-', widths[1]) + "| " + new string('-', widths[2]));

        foreach (var row in sorted)
        {
            Console.WriteLine(
                row.Name.PadRight(widths[0]) + "| " +
                row.ToRateString().PadRight(widths[1]) + "| " +
                row.ToSpeedString().PadRight(widths[2])
            );
        }
    }
}