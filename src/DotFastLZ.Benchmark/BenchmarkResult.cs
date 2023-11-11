namespace DotFastLZ.Benchmark;

public class BenchmarkResult
{
    public const int CollSize = 4;
    public string Name;
    public int Times;
    public BenchmarkSpeed Compression;
    public BenchmarkSpeed Decompression;

    public override string ToString()
    {
        var result = "";
        result += $"{Name}\n";
        result += $"  - times: {Times}\n";
        result += $"  - source bytes: {Compression.InputBytes / Times}\n";
        result += $"  - compression bytes: {Compression.OutputBytes / Times}\n";
        result += $"  - compression rate: {Compression.ComputeRate():F2}%\n";
        result += $"  - compression speed: {Compression.ComputeSpeed():F2} MB/s\n";
        result += $"  - decompression speed: {Decompression.ComputeSpeed():F2} MB/s\n";

        return result;
    }

    public double ComputeTotalSpeed()
    {
        return Compression.ComputeSpeed() + Decompression.ComputeSpeed();
    }
}