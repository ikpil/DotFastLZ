namespace DotFastLZ.Benchmark;

public class BenchmarkResult
{
    public const int CollSize = 6;

    public string Name;
    public string FileName;
    public long SourceByteLength;

    public int Times;
    public BenchmarkSpeed Compression;
    public BenchmarkSpeed Decompression;

    public override string ToString()
    {
        var result = "";
        result += $"{Name}\n";
        result += $"  - times: {Times}\n";
        result += $"  - filename : {FileName}\n";
        result += $"  - source bytes: {ToSourceKbString()} kB/s\n";
        result += $"  - compression bytes: {Compression.OutputBytes / Times}\n";
        result += $"  - compression rate: {Compression.ComputeRate():F2}%\n";
        result += $"  - compression speed: {Compression.ComputeSpeed():F2} MB/s\n";
        result += $"  - decompression speed: {Decompression.ComputeSpeed():F2} MB/s\n";

        return result;
    }

    public string ToSourceKbString()
    {
        double mb = (double)SourceByteLength / 1024;
        return $"{mb:F2}";
    }

    public double ComputeTotalSpeed()
    {
        return Compression.ComputeSpeed() + Decompression.ComputeSpeed();
    }
}