using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DotFastLZ.Benchmark;

public static class Benchmarks
{
    public static BenchmarkResult Start(string name, string filename, byte[] srcBytes, byte[] dstBytes, Func<byte[], byte[], long> compress, Func<byte[], long, byte[], long> decompress)
    {
        var result = new BenchmarkResult();
        result.Name = name;
        result.FileName = filename;
        result.Times = 5;
        result.SourceByteLength = srcBytes.Length;
        result.Compression.ElapsedWatch = new Stopwatch();
        result.Compression.ElapsedWatch.Start();
        for (int i = 0; i < result.Times; ++i)
        {
            long size = compress.Invoke(srcBytes, dstBytes);
            result.Compression.InputBytes += srcBytes.Length;
            result.Compression.OutputBytes += size;
        }

        result.Compression.ElapsedWatch.Stop();

        var decompInputLength = result.Compression.OutputBytes / result.Times;
        //dstBytes.AsSpan(0, (int)(compressedResult.DestBytes / compressedResult.Times)).CopyTo(srcBytes);

        result.Decompression.ElapsedWatch = new Stopwatch();
        result.Decompression.ElapsedWatch.Start();
        for (int i = 0; i < result.Times; ++i)
        {
            long size = decompress.Invoke(dstBytes, decompInputLength, srcBytes);
            result.Decompression.InputBytes += decompInputLength;
            result.Decompression.OutputBytes += size;
        }

        result.Decompression.ElapsedWatch.Stop();

        //Console.WriteLine(result.ToString());
        return result;
    }

    public static void Print(string headline, List<BenchmarkResult> results)
    {
        var rows = results
            .OrderByDescending(x => x.ComputeTotalSpeed())
            .ToList();


        // 각 열의 최대 길이를 찾기 위한 작업
        int[] widths = new int[BenchmarkResult.CollSize];
        for (int i = 0; i < rows.Count; i++)
        {
            widths[0] = Math.Max(rows[i].Name.Length, widths[0]);
            widths[1] = Math.Max(rows[i].FileName.Length, widths[1]);
            widths[2] = Math.Max(rows[i].ToSourceKbString().Length, widths[2]);
            widths[3] = Math.Max(rows[i].Compression.ToSpeedString().Length, widths[3]);
            widths[4] = Math.Max(rows[i].Decompression.ToSpeedString().Length, widths[4]);
            widths[5] = Math.Max(rows[i].Compression.ToRateString().Length, widths[5]);
        }

        var headName = "Name";
        var headFilename = "Filename";
        var headFileSize = "File kB";
        var headCompMbs = "Comp. MB/s";
        var headDecompMbs = "Decomp. MB/s";
        var headRate = "Rate";

        widths[0] = Math.Max(widths[0], headName.Length) + 1;
        widths[1] = Math.Max(widths[1], headFilename.Length) + 1;
        widths[2] = Math.Max(widths[2], headFileSize.Length) + 1;
        widths[3] = Math.Max(widths[3], headCompMbs.Length) + 1;
        widths[4] = Math.Max(widths[4], headDecompMbs.Length) + 1;
        widths[5] = Math.Max(widths[5], headRate.Length) + 1;

        Console.WriteLine();
        Console.WriteLine($"### {headline} ###");
        Console.WriteLine();


        // 표 출력
        Console.WriteLine("| " +
                          headName + new string(' ', widths[0] - headName.Length) + "| " +
                          headFilename + new string(' ', widths[1] - headFilename.Length) + "| " +
                          headFileSize + new string(' ', widths[2] - headFileSize.Length) + "| " +
                          headCompMbs + new string(' ', widths[3] - headCompMbs.Length) + "| " +
                          headDecompMbs + new string(' ', widths[4] - headDecompMbs.Length) + "| " +
                          headRate + new string(' ', widths[5] - headRate.Length) + "|");
        Console.WriteLine("|-" +
                          new string('-', widths[0]) + "|-" +
                          new string('-', widths[1]) + "|-" +
                          new string('-', widths[2]) + "|-" +
                          new string('-', widths[3]) + "|-" +
                          new string('-', widths[4]) + "|-" +
                          new string('-', widths[5]) + "|");

        foreach (var row in rows)
        {
            Console.WriteLine(
                "| " +
                row.Name.PadRight(widths[0]) + "| " +
                row.FileName.PadRight(widths[1]) + "| " +
                row.ToSourceKbString().PadRight(widths[2]) + "| " +
                row.Compression.ToSpeedString().PadRight(widths[3]) + "| " +
                row.Decompression.ToSpeedString().PadRight(widths[4]) + "| " +
                row.Compression.ToRateString().PadRight(widths[5]) + "|"
            );
        }
    }
}