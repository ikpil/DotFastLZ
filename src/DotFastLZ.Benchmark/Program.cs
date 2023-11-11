using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.IO.Compression;
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

            var benchmakrs = new List<IBenchmark>();

            benchmakrs.Add(new BenchmarkMemCopy());

            benchmakrs.Add(new BenchmarkDotFastLZ(1));
            benchmakrs.Add(new BenchmarkDotFastLZ(2));

            benchmakrs.Add(new BenchmarkK4osLZ4(LZ4Level.L00_FAST));
            benchmakrs.Add(new BenchmarkK4osLZ4(LZ4Level.L03_HC));
            benchmakrs.Add(new BenchmarkK4osLZ4(LZ4Level.L09_HC));
            benchmakrs.Add(new BenchmarkK4osLZ4(LZ4Level.L12_MAX));

            benchmakrs.Add(new BenchmarkSystemZip(CompressionLevel.Fastest));
            benchmakrs.Add(new BenchmarkSystemZip(CompressionLevel.Optimal));

            var results = new List<BenchmarkResult>();
            foreach (var file in R.SourceFiles)
            {
                var filepath = R.Find(Path.Combine(R.Prefix, file));
                var srcBytes = R.ToBytes(filepath);
                var dstBytes = new byte[srcBytes.Length * 2];

                foreach (var benchmark in benchmakrs)
                {
                    var result = benchmark.Start(file, srcBytes.ToArray(), dstBytes);
                    results.Add(result);

                    Console.WriteLine(result.ToString());
                }
            }

            Benchmarks.Print($"Benchmark", results);
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
}