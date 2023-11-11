using System;
using K4os.Compression.LZ4;

namespace DotFastLZ.Benchmark;

public class BenchmarkK4osLZ4 : IBenchmark
{
    public string Name => _name;

    private string _name;
    private LZ4Level _level;

    public BenchmarkK4osLZ4(LZ4Level level)
    {
        _level = level;
        _name = $"K4os.LZ L{_level.ToString()}";

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
    }


    public BenchmarkResult Start(string filename, byte[] srcBytes, byte[] dstBytes)
    {
        return Benchmarks.Start(Name, filename, srcBytes, dstBytes,
            (s, d) => CompressK4osLZ4(s, d, _level), DecompressK4osLZ4);
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
}