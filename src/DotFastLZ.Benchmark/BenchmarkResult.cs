using System;
using System.Diagnostics;

namespace DotFastLZ.Benchmark;

public class BenchmarkResult
{
    public string Name;
    public int Times;
    public Stopwatch Watch;
    public long SourceBytes;
    public long DestBytes;

    public static double CalculateCompressionSpeed(long size, TimeSpan timeSpan)
    {
        // MB/s
        return (double)size / (1024 * 1024) / timeSpan.TotalSeconds;
    }

    public double ComputeRate()
    {
        return DestBytes / (double)SourceBytes * 100;
    }

    public string ToRateString()
    {
        return $"{ComputeRate():F2}";
    }

    public double ComputeSpeed()
    {
        return CalculateCompressionSpeed(DestBytes, Watch.Elapsed);
    }

    public string ToSpeedString()
    {
        return $"{ComputeSpeed():F2}";
    }

    public override string ToString()
    {
        var result = "";
        result += $"{Name}\n";
        result += $"  - times: {Times}\n";
        result += $"  - source bytes: {SourceBytes / Times}\n";
        result += $"  - compression bytes: {DestBytes / Times}\n";
        result += $"  - compression rate: {ComputeRate():F2}%\n";
        result += $"  - compression speed: {ComputeSpeed():F2} MB/s\n";

        return result;
    }
}