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

            var totalResults = new List<BenchmarkResult>();
            foreach (var file in R.SourceFiles)
            {
                var results = BenchmarkFile(file);
                totalResults.AddRange(results);
            }

            Print($"Benchmark", totalResults);
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

    private static List<BenchmarkResult> BenchmarkFile(string filename)
    {
        var results = new List<BenchmarkResult>();

        var filepath = R.Find(Path.Combine(R.Prefix, filename));
        var srcBytes = R.ToBytes(filepath);
        var dstBytes = new byte[srcBytes.Length * 2];

        // memcpy
        var benchmark = new BenchmarkMemCopy();
        var memoryCopy = benchmark.Start(filename, srcBytes.ToArray(), dstBytes);
        results.Add(memoryCopy);

        // // DotFastLZ fastlz
        // var fastlzLv1 = Benchmark($"DotFastLZ.Compression L1", filename, srcBytes.ToArray(), dstBytes, CompressDotFastLZL1, DecompressDotFastLZ);
        // var fastlzLv2 = Benchmark($"DotFastLZ.Compression L2", filename, srcBytes.ToArray(), dstBytes, CompressDotFastLZL2, DecompressDotFastLZ);
        // results.Add(fastlzLv1);
        // results.Add(fastlzLv2);
        //
        // // K4os LZ4
        // var k4osL01 = Benchmark($"K4os.Compression.LZ4 L00", filename,
        //     srcBytes.ToArray(), dstBytes, (s, d) => CompressK4osLZ4(s, d, LZ4Level.L00_FAST), DecompressK4osLZ4);
        // var k4osL12 = Benchmark($"K4os.Compression.LZ4 L12", filename,
        //     srcBytes.ToArray(), dstBytes, (s, d) => CompressK4osLZ4(s, d, LZ4Level.L12_MAX), DecompressK4osLZ4);
        // var k4osL03HC = Benchmark($"K4os.Compression.LZ4 L03_HC", filename,
        //     srcBytes.ToArray(), dstBytes, (s, d) => CompressK4osLZ4(s, d, LZ4Level.L03_HC), DecompressK4osLZ4);
        // var k4osL09HC = Benchmark($"K4os.Compression.LZ4 L09_HC", filename,
        //     srcBytes.ToArray(), dstBytes, (s, d) => CompressK4osLZ4(s, d, LZ4Level.L09_HC), DecompressK4osLZ4);
        // results.Add(k4osL01);
        // results.Add(k4osL12);
        // results.Add(k4osL03HC);
        // results.Add(k4osL09HC);
        //
        // var zipFastest = Benchmark($"System.IO.Compression.ZipArchive Fastest", filename,
        //     srcBytes.ToArray(), dstBytes, (s, d) => CompressZip(s, d, CompressionLevel.Fastest), DecompressZip);
        // var zipOptimal = Benchmark($"System.IO.Compression.ZipArchive Optimal", filename,
        //     srcBytes.ToArray(), dstBytes, (s, d) => CompressZip(s, d, CompressionLevel.Optimal), DecompressZip);
        // results.Add(zipFastest);
        // results.Add(zipOptimal);

        return results;
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