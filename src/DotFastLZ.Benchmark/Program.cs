using System;
using DotFastLZ.Resource;

namespace DotFastLZ.Benchmark;

public static class Program
{
    public static void Main(string[] args)
    {
        try
        {
            R.ExtractAll();
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

    public static BenchmarkResult Benchmark(string file, int count)
    {
        return null;
    }
}