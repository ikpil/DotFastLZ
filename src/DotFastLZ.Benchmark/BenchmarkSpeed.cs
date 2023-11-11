using System;
using System.Diagnostics;

namespace DotFastLZ.Benchmark;

public struct BenchmarkSpeed
{
    public Stopwatch ElapsedWatch;
    public long InputBytes;
    public long OutputBytes;

    public double ComputeRate()
    {
        return OutputBytes / (double)InputBytes * 100;
    }

    public string ToRateString()
    {
        return $"{ComputeRate():F2}";
    }

    public double ComputeSpeed()
    {
        return CalculateCompressionSpeed(OutputBytes, ElapsedWatch.Elapsed);
    }

    public static double CalculateCompressionSpeed(long size, TimeSpan timeSpan)
    {
        // MB/s
        return (double)size / (1024 * 1024) / timeSpan.TotalSeconds;
    }

    public string ToSpeedString()
    {
        return $"{ComputeSpeed():F2}";
    }
}