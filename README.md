[![License](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)
[![.NET](https://github.com/ikpil/DotFastLZ/actions/workflows/dotnet.yml/badge.svg)](https://github.com/ikpil/DotFastLZ/actions/workflows/dotnet.yml)
[![CodeQL](https://github.com/ikpil/DotFastLZ/actions/workflows/github-code-scanning/codeql/badge.svg)](https://github.com/ikpil/DotFastLZ/actions/workflows/github-code-scanning/codeql)
[![NuGet Version and Downloads count](https://buildstats.info/nuget/DotFastLZ.Compression)](https://www.nuget.org/packages/DotFastLZ.Compression)
![Repo Size](https://img.shields.io/github/repo-size/ikpil/DotFastLZ.svg?colorB=lightgray)
![Languages](https://img.shields.io/github/languages/top/ikpil/DotFastLZ)

## Introduction ##

- DotFastLZ is a C# port of FastLZ [ariya/FastLZ](https://github.com/ariya/FastLZ)
- DotFastLZ can be used in C# projects and Unity3D, and it's great for compressing small, repetitive data.

## Usage: DotFastLZ.Compression ##
```csharp
for (int level = 1; level <= 2; ++level)
{
    // compress
    var input = GetInputSource();
    var estimateSize = FastLZ.EstimateCompressedSize(input.Length);
    var comBuf = new byte[estimateSize];
    var comBufSize = FastLZ.CompressLevel(level, input, input.Length, comBuf);

    // decompress
    byte[] decBuf = new byte[input.Length];
    var decBufSize = FastLZ.Decompress(comBuf, comBufSize, decBuf, decBuf.Length);

    // compare
    var compareSize = FastLZ.MemCompare(input, 0, decBuf, 0, decBufSize);

    // check
    Assert.That(decBufSize, Is.EqualTo(input.Length), "decompress size error");
    Assert.That(compareSize, Is.EqualTo(input.Length), "decompress compare error");
}
```

## Usage: DotFastLZ.Compression.Packaging ##
```csharp
const string targetFileName = "soruce.txt";
string packagingFileName = targetFileName + ".fastlz";

// pack/unpack
SixPack.PackFile(2, targetFileName, packagingFileName, Console.Write);
SixPack.UnpackFile(packagingFileName, Console.Write);
```

## Usage: DotFastLZ.Packaging.Tools ##
```shell
$ 6pack --help

6pack: high-speed file compression tool
Copyright (C) Ariya Hidayat, Choi Ikpil(ikpil@naver.com)
 - https://github.com/ikpil/DotFastLZ

Usage: 6pack [options] input-file output-file

Options:
  -1    compress faster
  -2    compress better
  -v    show program version
  -d    decompression (default for .fastlz extension)
  -mem  check in-memory compression speed
```

# Benchmarks
- CPU : AMD Ryzen 5 5625U

## Benchmark : compression canterbury/alice29.txt

| Name                                     | MB/s    | Rate    |
|------------------------------------------|---------|---------|
| memcpy                                   | 3629.71 | 100.00  |
| K4os.Compression.LZ4 L00                 | 39.44   | 58.32   |
| DotFastLZ.Compression L2                 | 38.42   | 55.68   |
| DotFastLZ.Compression L1                 | 36.20   | 56.19   |
| System.IO.Compression.ZipArchive Fastest | 32.00   | 40.86   |
| System.IO.Compression.ZipArchive Optimal | 11.46   | 36.18   |
| K4os.Compression.LZ4 L03_HC              | 6.46    | 44.64   |
| K4os.Compression.LZ4 L09_HC              | 2.77    | 41.87   |
| K4os.Compression.LZ4 L12                 | 2.27    | 41.40   |

## Benchmark : decompression canterbury/alice29.txt

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 10330.72 | 100.00   |
| K4os.Compression.LZ4 L09_HC              | 679.44   | 41.87    |
| K4os.Compression.LZ4 L12                 | 652.48   | 41.40    |
| K4os.Compression.LZ4 L03_HC              | 436.48   | 44.64    |
| System.IO.Compression.ZipArchive Optimal | 130.36   | 36.18    |
| System.IO.Compression.ZipArchive Fastest | 124.41   | 40.86    |
| K4os.Compression.LZ4 L00                 | 122.05   | 58.32    |
| DotFastLZ.Compression L1                 | 81.92    | 56.19    |
| DotFastLZ.Compression L2                 | 80.30    | 55.68    |

## Benchmark : compression canterbury/asyoulik.txt

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 12132.11 | 100.00   |
| K4os.Compression.LZ4 L00                 | 163.17   | 63.64    |
| DotFastLZ.Compression L1                 | 45.38    | 59.54    |
| DotFastLZ.Compression L2                 | 42.79    | 58.91    |
| System.IO.Compression.ZipArchive Fastest | 34.58    | 43.37    |
| System.IO.Compression.ZipArchive Optimal | 12.17    | 39.76    |
| K4os.Compression.LZ4 L09_HC              | 9.58     | 47.06    |
| K4os.Compression.LZ4 L03_HC              | 7.73     | 49.55    |
| K4os.Compression.LZ4 L12                 | 2.72     | 46.58    |

## Benchmark : decompression canterbury/asyoulik.txt

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 52359.65 | 100.00   |
| K4os.Compression.LZ4 L00                 | 994.86   | 63.64    |
| K4os.Compression.LZ4 L09_HC              | 824.29   | 47.06    |
| K4os.Compression.LZ4 L03_HC              | 692.28   | 49.55    |
| K4os.Compression.LZ4 L12                 | 649.22   | 46.58    |
| System.IO.Compression.ZipArchive Optimal | 136.30   | 39.76    |
| System.IO.Compression.ZipArchive Fastest | 133.34   | 43.37    |
| DotFastLZ.Compression L2                 | 107.15   | 58.91    |
| DotFastLZ.Compression L1                 | 105.92   | 59.54    |

## Benchmark : compression canterbury/cp.html

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 37843.95 | 100.00   |
| K4os.Compression.LZ4 L00                 | 118.10   | 48.40    |
| DotFastLZ.Compression L1                 | 45.18    | 49.32    |
| DotFastLZ.Compression L2                 | 44.50    | 47.77    |
| System.IO.Compression.ZipArchive Fastest | 36.94    | 35.82    |
| K4os.Compression.LZ4 L09_HC              | 28.91    | 42.03    |
| System.IO.Compression.ZipArchive Optimal | 18.62    | 33.04    |
| K4os.Compression.LZ4 L03_HC              | 13.76    | 42.47    |
| K4os.Compression.LZ4 L12                 | 11.20    | 41.83    |

## Benchmark : decompression canterbury/cp.html

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 39105.42 | 100.00   |
| K4os.Compression.LZ4 L00                 | 556.68   | 48.40    |
| K4os.Compression.LZ4 L09_HC              | 531.35   | 42.03    |
| K4os.Compression.LZ4 L03_HC              | 268.57   | 42.47    |
| System.IO.Compression.ZipArchive Optimal | 140.61   | 33.04    |
| K4os.Compression.LZ4 L12                 | 137.49   | 41.83    |
| System.IO.Compression.ZipArchive Fastest | 134.99   | 35.82    |
| DotFastLZ.Compression L1                 | 104.01   | 49.32    |
| DotFastLZ.Compression L2                 | 93.52    | 47.77    |

## Benchmark : compression canterbury/fields.c

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 33229.59 | 100.00   |
| K4os.Compression.LZ4 L00                 | 133.63   | 46.80    |
| DotFastLZ.Compression L1                 | 36.80    | 42.46    |
| System.IO.Compression.ZipArchive Fastest | 35.20    | 33.19    |
| K4os.Compression.LZ4 L03_HC              | 35.13    | 38.27    |
| DotFastLZ.Compression L2                 | 33.45    | 42.38    |
| K4os.Compression.LZ4 L09_HC              | 32.07    | 37.95    |
| System.IO.Compression.ZipArchive Optimal | 17.28    | 28.98    |
| K4os.Compression.LZ4 L12                 | 9.13     | 37.71    |

## Benchmark : decompression canterbury/fields.c

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 48333.95 | 100.00   |
| K4os.Compression.LZ4 L09_HC              | 367.49   | 37.95    |
| K4os.Compression.LZ4 L00                 | 365.90   | 46.80    |
| K4os.Compression.LZ4 L03_HC              | 363.33   | 38.27    |
| System.IO.Compression.ZipArchive Optimal | 128.17   | 28.98    |
| K4os.Compression.LZ4 L12                 | 123.24   | 37.71    |
| System.IO.Compression.ZipArchive Fastest | 115.95   | 33.19    |
| DotFastLZ.Compression L2                 | 86.52    | 42.38    |
| DotFastLZ.Compression L1                 | 66.79    | 42.46    |

## Benchmark : compression canterbury/grammar.lsp

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 17743.11 | 100.00   |
| K4os.Compression.LZ4 L03_HC              | 53.17    | 46.63    |
| K4os.Compression.LZ4 L09_HC              | 44.80    | 46.41    |
| DotFastLZ.Compression L2                 | 30.26    | 47.89    |
| System.IO.Compression.ZipArchive Fastest | 29.64    | 37.62    |
| DotFastLZ.Compression L1                 | 29.27    | 47.89    |
| System.IO.Compression.ZipArchive Optimal | 21.07    | 35.82    |
| K4os.Compression.LZ4 L12                 | 15.27    | 46.25    |
| K4os.Compression.LZ4 L00                 | 2.73     | 51.46    |

## Benchmark : decompression canterbury/grammar.lsp

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 44357.78 | 100.00   |
| K4os.Compression.LZ4 L09_HC              | 452.47   | 46.41    |
| K4os.Compression.LZ4 L03_HC              | 449.63   | 46.63    |
| K4os.Compression.LZ4 L12                 | 418.69   | 46.25    |
| K4os.Compression.LZ4 L00                 | 375.78   | 51.46    |
| System.IO.Compression.ZipArchive Optimal | 111.32   | 35.82    |
| DotFastLZ.Compression L1                 | 93.07    | 47.89    |
| System.IO.Compression.ZipArchive Fastest | 92.59    | 37.62    |
| DotFastLZ.Compression L2                 | 91.47    | 47.89    |

## Benchmark : compression canterbury/kennedy.xls

| Name                                     | MB/s    | Rate    |
|------------------------------------------|---------|---------|
| memcpy                                   | 8826.54 | 100.00  |
| K4os.Compression.LZ4 L00                 | 220.90  | 36.39   |
| DotFastLZ.Compression L1                 | 58.05   | 39.37   |
| DotFastLZ.Compression L2                 | 56.19   | 40.08   |
| System.IO.Compression.ZipArchive Fastest | 47.76   | 23.17   |
| K4os.Compression.LZ4 L03_HC              | 36.33   | 31.96   |
| System.IO.Compression.ZipArchive Optimal | 17.00   | 21.38   |
| K4os.Compression.LZ4 L09_HC              | 4.19    | 31.48   |
| K4os.Compression.LZ4 L12                 | 0.34    | 31.48   |

## Benchmark : decompression canterbury/kennedy.xls

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 15291.82 | 100.00   |
| K4os.Compression.LZ4 L00                 | 928.81   | 36.39    |
| K4os.Compression.LZ4 L09_HC              | 894.19   | 31.48    |
| K4os.Compression.LZ4 L12                 | 804.00   | 31.48    |
| K4os.Compression.LZ4 L03_HC              | 751.07   | 31.96    |
| System.IO.Compression.ZipArchive Optimal | 194.53   | 21.38    |
| System.IO.Compression.ZipArchive Fastest | 181.59   | 23.17    |
| DotFastLZ.Compression L1                 | 114.34   | 39.37    |
| DotFastLZ.Compression L2                 | 113.25   | 40.08    |

## Benchmark : compression canterbury/lcet10.txt

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 11065.37 | 100.00   |
| K4os.Compression.LZ4 L00                 | 113.94   | 54.65    |
| DotFastLZ.Compression L1                 | 81.99    | 54.67    |
| DotFastLZ.Compression L2                 | 81.43    | 53.70    |
| System.IO.Compression.ZipArchive Fastest | 38.68    | 38.97    |
| K4os.Compression.LZ4 L03_HC              | 29.42    | 41.21    |
| System.IO.Compression.ZipArchive Optimal | 11.31    | 34.27    |
| K4os.Compression.LZ4 L09_HC              | 10.30    | 38.85    |
| K4os.Compression.LZ4 L12                 | 5.84     | 38.46    |

## Benchmark : decompression canterbury/lcet10.txt

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 43296.21 | 100.00   |
| K4os.Compression.LZ4 L00                 | 1383.52  | 54.65    |
| K4os.Compression.LZ4 L09_HC              | 957.10   | 38.85    |
| K4os.Compression.LZ4 L03_HC              | 942.79   | 41.21    |
| K4os.Compression.LZ4 L12                 | 939.07   | 38.46    |
| DotFastLZ.Compression L1                 | 194.56   | 54.67    |
| DotFastLZ.Compression L2                 | 190.37   | 53.70    |
| System.IO.Compression.ZipArchive Optimal | 137.87   | 34.27    |
| System.IO.Compression.ZipArchive Fastest | 135.36   | 38.97    |

## Benchmark : compression canterbury/plrabn12.txt

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 35901.44 | 100.00   |
| K4os.Compression.LZ4 L00                 | 183.01   | 67.57    |
| DotFastLZ.Compression L1                 | 85.53    | 62.37    |
| DotFastLZ.Compression L2                 | 84.39    | 61.85    |
| System.IO.Compression.ZipArchive Fastest | 38.73    | 44.95    |
| K4os.Compression.LZ4 L03_HC              | 31.21    | 50.64    |
| System.IO.Compression.ZipArchive Optimal | 11.24    | 41.15    |
| K4os.Compression.LZ4 L09_HC              | 9.53     | 47.20    |
| K4os.Compression.LZ4 L12                 | 6.01     | 46.62    |

## Benchmark : decompression canterbury/plrabn12.txt

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 44271.53 | 100.00   |
| K4os.Compression.LZ4 L00                 | 1667.26  | 67.57    |
| K4os.Compression.LZ4 L09_HC              | 1008.86  | 47.20    |
| K4os.Compression.LZ4 L12                 | 968.68   | 46.62    |
| K4os.Compression.LZ4 L03_HC              | 883.95   | 50.64    |
| DotFastLZ.Compression L1                 | 212.48   | 62.37    |
| DotFastLZ.Compression L2                 | 203.22   | 61.85    |
| System.IO.Compression.ZipArchive Fastest | 141.90   | 44.95    |
| System.IO.Compression.ZipArchive Optimal | 137.39   | 41.15    |

## Benchmark : compression canterbury/ptt5

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 18413.88 | 100.00   |
| K4os.Compression.LZ4 L00                 | 165.35   | 16.93    |
| DotFastLZ.Compression L1                 | 70.37    | 15.84    |
| DotFastLZ.Compression L2                 | 68.22    | 15.77    |
| System.IO.Compression.ZipArchive Fastest | 36.33    | 12.57    |
| K4os.Compression.LZ4 L03_HC              | 24.99    | 13.55    |
| System.IO.Compression.ZipArchive Optimal | 10.94    | 10.66    |
| K4os.Compression.LZ4 L09_HC              | 4.33     | 13.01    |
| K4os.Compression.LZ4 L12                 | 1.01     | 12.89    |

## Benchmark : decompression canterbury/ptt5

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 37591.47 | 100.00   |
| K4os.Compression.LZ4 L09_HC              | 736.51   | 13.01    |
| K4os.Compression.LZ4 L12                 | 712.87   | 12.89    |
| K4os.Compression.LZ4 L03_HC              | 685.92   | 13.55    |
| K4os.Compression.LZ4 L00                 | 635.66   | 16.93    |
| DotFastLZ.Compression L1                 | 118.24   | 15.84    |
| DotFastLZ.Compression L2                 | 117.38   | 15.77    |
| System.IO.Compression.ZipArchive Fastest | 109.87   | 12.57    |
| System.IO.Compression.ZipArchive Optimal | 83.18    | 10.66    |

## Benchmark : compression canterbury/sum

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 52097.87 | 100.00   |
| K4os.Compression.LZ4 L00                 | 199.68   | 49.20    |
| DotFastLZ.Compression L2                 | 85.78    | 49.40    |
| DotFastLZ.Compression L1                 | 85.34    | 53.66    |
| System.IO.Compression.ZipArchive Fastest | 39.52    | 38.01    |
| K4os.Compression.LZ4 L03_HC              | 39.21    | 43.44    |
| System.IO.Compression.ZipArchive Optimal | 18.47    | 34.17    |
| K4os.Compression.LZ4 L09_HC              | 18.28    | 42.72    |
| K4os.Compression.LZ4 L12                 | 5.34     | 42.60    |

## Benchmark : decompression canterbury/sum

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 55255.31 | 100.00   |
| K4os.Compression.LZ4 L09_HC              | 1166.11  | 42.72    |
| K4os.Compression.LZ4 L12                 | 1078.98  | 42.60    |
| K4os.Compression.LZ4 L03_HC              | 1023.29  | 43.44    |
| K4os.Compression.LZ4 L00                 | 1012.61  | 49.20    |
| DotFastLZ.Compression L1                 | 233.69   | 53.66    |
| DotFastLZ.Compression L2                 | 192.57   | 49.40    |
| System.IO.Compression.ZipArchive Optimal | 135.32   | 34.17    |
| System.IO.Compression.ZipArchive Fastest | 134.96   | 38.01    |

## Benchmark : compression canterbury/xargs.1

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 50389.77 | 100.00   |
| K4os.Compression.LZ4 L00                 | 204.33   | 62.95    |
| DotFastLZ.Compression L1                 | 80.15    | 58.46    |
| DotFastLZ.Compression L2                 | 73.60    | 58.46    |
| K4os.Compression.LZ4 L03_HC              | 65.65    | 57.23    |
| K4os.Compression.LZ4 L09_HC              | 62.23    | 57.09    |
| System.IO.Compression.ZipArchive Fastest | 39.18    | 45.56    |
| System.IO.Compression.ZipArchive Optimal | 25.49    | 43.70    |
| K4os.Compression.LZ4 L12                 | 20.68    | 56.87    |

## Benchmark : decompression canterbury/xargs.1

| Name                                     | MB/s      | Rate      |
|------------------------------------------|-----------|-----------|
| memcpy                                   | 100779.53 | 100.00    |
| K4os.Compression.LZ4 L09_HC              | 1456.47   | 57.09     |
| K4os.Compression.LZ4 L03_HC              | 1424.04   | 57.23     |
| K4os.Compression.LZ4 L12                 | 1364.66   | 56.87     |
| K4os.Compression.LZ4 L00                 | 1256.30   | 62.95     |
| DotFastLZ.Compression L1                 | 313.37    | 58.46     |
| DotFastLZ.Compression L2                 | 298.29    | 58.46     |
| System.IO.Compression.ZipArchive Optimal | 143.91    | 43.70     |
| System.IO.Compression.ZipArchive Fastest | 119.12    | 45.56     |

## Benchmark : compression silesia/dickens

| Name                                     | MB/s    | Rate    |
|------------------------------------------|---------|---------|
| memcpy                                   | 8166.65 | 100.00  |
| K4os.Compression.LZ4 L00                 | 175.58  | 63.07   |
| DotFastLZ.Compression L2                 | 82.05   | 58.84   |
| DotFastLZ.Compression L1                 | 80.64   | 59.46   |
| System.IO.Compression.ZipArchive Fastest | 36.82   | 42.78   |
| K4os.Compression.LZ4 L03_HC              | 30.26   | 46.87   |
| System.IO.Compression.ZipArchive Optimal | 10.77   | 38.53   |
| K4os.Compression.LZ4 L09_HC              | 9.30    | 43.49   |
| K4os.Compression.LZ4 L12                 | 5.58    | 42.93   |

## Benchmark : decompression silesia/dickens

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 16898.36 | 100.00   |
| K4os.Compression.LZ4 L00                 | 1530.79  | 63.07    |
| K4os.Compression.LZ4 L09_HC              | 977.98   | 43.49    |
| K4os.Compression.LZ4 L03_HC              | 970.42   | 46.87    |
| K4os.Compression.LZ4 L12                 | 925.76   | 42.93    |
| DotFastLZ.Compression L1                 | 203.86   | 59.46    |
| DotFastLZ.Compression L2                 | 196.23   | 58.84    |
| System.IO.Compression.ZipArchive Fastest | 132.70   | 42.78    |
| System.IO.Compression.ZipArchive Optimal | 132.47   | 38.53    |

## Benchmark : compression silesia/mozilla

| Name                                     | MB/s    | Rate    |
|------------------------------------------|---------|---------|
| memcpy                                   | 7729.67 | 100.00  |
| K4os.Compression.LZ4 L00                 | 220.78  | 51.61   |
| DotFastLZ.Compression L2                 | 92.43   | 51.66   |
| DotFastLZ.Compression L1                 | 87.04   | 52.69   |
| System.IO.Compression.ZipArchive Fastest | 38.51   | 40.31   |
| K4os.Compression.LZ4 L03_HC              | 37.34   | 44.15   |
| System.IO.Compression.ZipArchive Optimal | 17.65   | 37.18   |
| K4os.Compression.LZ4 L09_HC              | 16.32   | 43.11   |
| K4os.Compression.LZ4 L12                 | 2.91    | 42.98   |

## Benchmark : decompression silesia/mozilla

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 14459.50 | 100.00   |
| K4os.Compression.LZ4 L00                 | 1359.25  | 51.61    |
| K4os.Compression.LZ4 L12                 | 1239.70  | 42.98    |
| K4os.Compression.LZ4 L09_HC              | 1184.09  | 43.11    |
| K4os.Compression.LZ4 L03_HC              | 1148.76  | 44.15    |
| DotFastLZ.Compression L1                 | 272.23   | 52.69    |
| DotFastLZ.Compression L2                 | 247.03   | 51.66    |
| System.IO.Compression.ZipArchive Fastest | 151.00   | 40.31    |
| System.IO.Compression.ZipArchive Optimal | 143.47   | 37.18    |

## Benchmark : compression silesia/mr

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 10592.97 | 100.00   |
| K4os.Compression.LZ4 L00                 | 225.82   | 54.57    |
| DotFastLZ.Compression L1                 | 95.44    | 50.89    |
| DotFastLZ.Compression L2                 | 93.54    | 50.59    |
| System.IO.Compression.ZipArchive Fastest | 38.32    | 37.98    |
| K4os.Compression.LZ4 L03_HC              | 38.11    | 46.59    |
| System.IO.Compression.ZipArchive Optimal | 15.37    | 36.36    |
| K4os.Compression.LZ4 L09_HC              | 8.29     | 42.58    |
| K4os.Compression.LZ4 L12                 | 3.92     | 42.02    |

## Benchmark : decompression silesia/mr

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 24557.52 | 100.00   |
| K4os.Compression.LZ4 L00                 | 1583.26  | 54.57    |
| K4os.Compression.LZ4 L09_HC              | 1156.54  | 42.58    |
| K4os.Compression.LZ4 L12                 | 1127.68  | 42.02    |
| K4os.Compression.LZ4 L03_HC              | 1112.97  | 46.59    |
| DotFastLZ.Compression L1                 | 261.63   | 50.89    |
| DotFastLZ.Compression L2                 | 247.10   | 50.59    |
| System.IO.Compression.ZipArchive Fastest | 143.16   | 37.98    |
| System.IO.Compression.ZipArchive Optimal | 141.63   | 36.36    |

## Benchmark : compression silesia/nci

| Name                                     | MB/s    | Rate    |
|------------------------------------------|---------|---------|
| memcpy                                   | 8085.63 | 100.00  |
| K4os.Compression.LZ4 L00                 | 164.96  | 16.49   |
| DotFastLZ.Compression L2                 | 81.45   | 19.60   |
| DotFastLZ.Compression L1                 | 80.06   | 20.64   |
| System.IO.Compression.ZipArchive Fastest | 39.09   | 12.38   |
| K4os.Compression.LZ4 L03_HC              | 23.34   | 12.67   |
| System.IO.Compression.ZipArchive Optimal | 8.75    | 9.61    |
| K4os.Compression.LZ4 L09_HC              | 3.67    | 10.95   |
| K4os.Compression.LZ4 L12                 | 1.98    | 10.78   |

## Benchmark : decompression silesia/nci

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 14985.46 | 100.00   |
| K4os.Compression.LZ4 L00                 | 640.30   | 16.49    |
| K4os.Compression.LZ4 L03_HC              | 498.71   | 12.67    |
| K4os.Compression.LZ4 L09_HC              | 468.88   | 10.95    |
| K4os.Compression.LZ4 L12                 | 462.38   | 10.78    |
| DotFastLZ.Compression L1                 | 173.91   | 20.64    |
| DotFastLZ.Compression L2                 | 163.85   | 19.60    |
| System.IO.Compression.ZipArchive Fastest | 123.80   | 12.38    |
| System.IO.Compression.ZipArchive Optimal | 117.33   | 9.61     |

## Benchmark : compression silesia/ooffice

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 21109.55 | 100.00   |
| K4os.Compression.LZ4 L00                 | 212.71   | 70.53    |
| DotFastLZ.Compression L2                 | 86.72    | 68.78    |
| DotFastLZ.Compression L1                 | 79.68    | 69.48    |
| K4os.Compression.LZ4 L03_HC              | 36.35    | 58.64    |
| System.IO.Compression.ZipArchive Fastest | 34.89    | 54.16    |
| K4os.Compression.LZ4 L09_HC              | 20.64    | 57.60    |
| System.IO.Compression.ZipArchive Optimal | 18.14    | 50.36    |
| K4os.Compression.LZ4 L12                 | 9.61     | 57.46    |

## Benchmark : decompression silesia/ooffice

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 16275.14 | 100.00   |
| K4os.Compression.LZ4 L00                 | 1742.63  | 70.53    |
| K4os.Compression.LZ4 L09_HC              | 1347.40  | 57.60    |
| K4os.Compression.LZ4 L03_HC              | 1341.54  | 58.64    |
| K4os.Compression.LZ4 L12                 | 1339.24  | 57.46    |
| DotFastLZ.Compression L1                 | 257.04   | 69.48    |
| DotFastLZ.Compression L2                 | 248.47   | 68.78    |
| System.IO.Compression.ZipArchive Fastest | 138.78   | 54.16    |
| System.IO.Compression.ZipArchive Optimal | 125.94   | 50.36    |

## Benchmark : compression silesia/osdb

| Name                                     | MB/s    | Rate    |
|------------------------------------------|---------|---------|
| memcpy                                   | 8909.44 | 100.00  |
| K4os.Compression.LZ4 L00                 | 184.56  | 52.12   |
| DotFastLZ.Compression L2                 | 99.28   | 52.65   |
| DotFastLZ.Compression L1                 | 87.72   | 65.83   |
| System.IO.Compression.ZipArchive Fastest | 42.17   | 38.86   |
| K4os.Compression.LZ4 L03_HC              | 38.11   | 40.11   |
| K4os.Compression.LZ4 L09_HC              | 21.35   | 39.44   |
| System.IO.Compression.ZipArchive Optimal | 20.83   | 36.57   |
| K4os.Compression.LZ4 L12                 | 10.58   | 39.13   |

## Benchmark : decompression silesia/osdb

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 21584.44 | 100.00   |
| K4os.Compression.LZ4 L00                 | 1407.10  | 52.12    |
| K4os.Compression.LZ4 L12                 | 1261.77  | 39.13    |
| K4os.Compression.LZ4 L03_HC              | 1223.00  | 40.11    |
| K4os.Compression.LZ4 L09_HC              | 1210.72  | 39.44    |
| DotFastLZ.Compression L1                 | 422.25   | 65.83    |
| DotFastLZ.Compression L2                 | 308.80   | 52.65    |
| System.IO.Compression.ZipArchive Fastest | 180.81   | 38.86    |
| System.IO.Compression.ZipArchive Optimal | 175.08   | 36.57    |

## Benchmark : compression silesia/reymont

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 10818.91 | 100.00   |
| K4os.Compression.LZ4 L00                 | 169.84   | 48.01    |
| DotFastLZ.Compression L2                 | 82.73    | 47.60    |
| DotFastLZ.Compression L1                 | 80.93    | 49.85    |
| System.IO.Compression.ZipArchive Fastest | 38.19    | 34.90    |
| K4os.Compression.LZ4 L03_HC              | 29.05    | 36.64    |
| System.IO.Compression.ZipArchive Optimal | 8.52     | 28.44    |
| K4os.Compression.LZ4 L09_HC              | 4.90     | 31.86    |
| K4os.Compression.LZ4 L12                 | 3.17     | 31.13    |

## Benchmark : decompression silesia/reymont

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 22565.67 | 100.00   |
| K4os.Compression.LZ4 L00                 | 1035.53  | 48.01    |
| K4os.Compression.LZ4 L03_HC              | 855.48   | 36.64    |
| K4os.Compression.LZ4 L12                 | 829.33   | 31.13    |
| K4os.Compression.LZ4 L09_HC              | 824.30   | 31.86    |
| DotFastLZ.Compression L1                 | 196.06   | 49.85    |
| DotFastLZ.Compression L2                 | 181.80   | 47.60    |
| System.IO.Compression.ZipArchive Fastest | 130.37   | 34.90    |
| System.IO.Compression.ZipArchive Optimal | 122.33   | 28.44    |

## Benchmark : compression silesia/samba

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 12310.15 | 100.00   |
| K4os.Compression.LZ4 L00                 | 196.64   | 35.72    |
| DotFastLZ.Compression L2                 | 86.58    | 35.45    |
| DotFastLZ.Compression L1                 | 82.55    | 38.61    |
| System.IO.Compression.ZipArchive Fastest | 38.97    | 28.64    |
| K4os.Compression.LZ4 L03_HC              | 32.62    | 29.20    |
| System.IO.Compression.ZipArchive Optimal | 14.80    | 25.45    |
| K4os.Compression.LZ4 L09_HC              | 12.93    | 28.42    |
| K4os.Compression.LZ4 L12                 | 3.35     | 28.21    |

## Benchmark : decompression silesia/samba

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 13819.53 | 100.00   |
| K4os.Compression.LZ4 L00                 | 1078.85  | 35.72    |
| K4os.Compression.LZ4 L03_HC              | 889.90   | 29.20    |
| K4os.Compression.LZ4 L12                 | 874.50   | 28.21    |
| K4os.Compression.LZ4 L09_HC              | 873.93   | 28.42    |
| DotFastLZ.Compression L1                 | 221.31   | 38.61    |
| DotFastLZ.Compression L2                 | 208.04   | 35.45    |
| System.IO.Compression.ZipArchive Fastest | 146.02   | 28.64    |
| System.IO.Compression.ZipArchive Optimal | 142.50   | 25.45    |

## Benchmark : compression silesia/sao

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 21980.65 | 100.00   |
| K4os.Compression.LZ4 L00                 | 194.55   | 93.63    |
| DotFastLZ.Compression L2                 | 107.80   | 88.07    |
| DotFastLZ.Compression L1                 | 87.30    | 88.08    |
| System.IO.Compression.ZipArchive Fastest | 39.89    | 76.70    |
| K4os.Compression.LZ4 L03_HC              | 37.65    | 80.96    |
| K4os.Compression.LZ4 L09_HC              | 22.03    | 79.09    |
| System.IO.Compression.ZipArchive Optimal | 20.39    | 73.80    |
| K4os.Compression.LZ4 L12                 | 12.74    | 78.17    |

## Benchmark : decompression silesia/sao

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 24563.12 | 100.00   |
| K4os.Compression.LZ4 L00                 | 2577.54  | 93.63    |
| K4os.Compression.LZ4 L12                 | 1972.28  | 78.17    |
| K4os.Compression.LZ4 L09_HC              | 1721.95  | 79.09    |
| K4os.Compression.LZ4 L03_HC              | 1673.91  | 80.96    |
| DotFastLZ.Compression L1                 | 464.44   | 88.08    |
| DotFastLZ.Compression L2                 | 344.20   | 88.07    |
| System.IO.Compression.ZipArchive Fastest | 226.87   | 76.70    |
| System.IO.Compression.ZipArchive Optimal | 202.64   | 73.80    |

## Benchmark : compression silesia/webster

| Name                                     | MB/s    | Rate    |
|------------------------------------------|---------|---------|
| memcpy                                   | 7072.95 | 100.00  |
| K4os.Compression.LZ4 L00                 | 162.50  | 48.58   |
| DotFastLZ.Compression L2                 | 79.41   | 47.89   |
| DotFastLZ.Compression L1                 | 78.99   | 48.91   |
| System.IO.Compression.ZipArchive Fastest | 36.72   | 34.62   |
| K4os.Compression.LZ4 L03_HC              | 29.53   | 35.55   |
| System.IO.Compression.ZipArchive Optimal | 12.27   | 29.70   |
| K4os.Compression.LZ4 L09_HC              | 10.34   | 33.77   |
| K4os.Compression.LZ4 L12                 | 5.07    | 33.34   |

## Benchmark : decompression silesia/webster

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 13210.90 | 100.00   |
| K4os.Compression.LZ4 L00                 | 1183.61  | 48.58    |
| K4os.Compression.LZ4 L03_HC              | 788.40   | 35.55    |
| K4os.Compression.LZ4 L12                 | 776.94   | 33.34    |
| K4os.Compression.LZ4 L09_HC              | 774.73   | 33.77    |
| DotFastLZ.Compression L1                 | 190.09   | 48.91    |
| DotFastLZ.Compression L2                 | 168.24   | 47.89    |
| System.IO.Compression.ZipArchive Fastest | 127.78   | 34.62    |
| System.IO.Compression.ZipArchive Optimal | 127.44   | 29.70    |

## Benchmark : compression silesia/x-ray

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 16671.48 | 100.00   |
| K4os.Compression.LZ4 L00                 | 638.72   | 99.01    |
| DotFastLZ.Compression L2                 | 107.69   | 96.70    |
| DotFastLZ.Compression L1                 | 96.91    | 96.74    |
| K4os.Compression.LZ4 L03_HC              | 37.78    | 84.99    |
| System.IO.Compression.ZipArchive Fastest | 36.81    | 74.87    |
| K4os.Compression.LZ4 L09_HC              | 35.04    | 84.67    |
| System.IO.Compression.ZipArchive Optimal | 26.23    | 70.77    |
| K4os.Compression.LZ4 L12                 | 23.77    | 84.64    |

## Benchmark : decompression silesia/x-ray

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 16400.15 | 100.00   |
| K4os.Compression.LZ4 L00                 | 6394.87  | 99.01    |
| K4os.Compression.LZ4 L03_HC              | 1765.76  | 84.99    |
| K4os.Compression.LZ4 L12                 | 1742.77  | 84.64    |
| K4os.Compression.LZ4 L09_HC              | 1741.64  | 84.67    |
| DotFastLZ.Compression L1                 | 419.18   | 96.74    |
| DotFastLZ.Compression L2                 | 395.99   | 96.70    |
| System.IO.Compression.ZipArchive Fastest | 199.12   | 74.87    |
| System.IO.Compression.ZipArchive Optimal | 179.78   | 70.77    |

## Benchmark : compression silesia/xml

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 10670.36 | 100.00   |
| K4os.Compression.LZ4 L00                 | 169.98   | 22.96    |
| DotFastLZ.Compression L2                 | 79.59    | 23.90    |
| DotFastLZ.Compression L1                 | 78.89    | 25.97    |
| System.IO.Compression.ZipArchive Fastest | 38.86    | 17.00    |
| K4os.Compression.LZ4 L03_HC              | 26.47    | 15.95    |
| System.IO.Compression.ZipArchive Optimal | 9.95     | 12.85    |
| K4os.Compression.LZ4 L09_HC              | 8.53     | 14.41    |
| K4os.Compression.LZ4 L12                 | 3.58     | 14.22    |

## Benchmark : decompression silesia/xml

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 16942.49 | 100.00   |
| K4os.Compression.LZ4 L00                 | 711.54   | 22.96    |
| K4os.Compression.LZ4 L03_HC              | 593.79   | 15.95    |
| K4os.Compression.LZ4 L09_HC              | 591.58   | 14.41    |
| K4os.Compression.LZ4 L12                 | 578.97   | 14.22    |
| DotFastLZ.Compression L1                 | 177.07   | 25.97    |
| DotFastLZ.Compression L2                 | 166.60   | 23.90    |
| System.IO.Compression.ZipArchive Fastest | 125.15   | 17.00    |
| System.IO.Compression.ZipArchive Optimal | 122.70   | 12.85    |

## Benchmark : compression enwik/enwik8

| Name                                     | MB/s    | Rate    |
|------------------------------------------|---------|---------|
| memcpy                                   | 8519.09 | 100.00  |
| K4os.Compression.LZ4 L00                 | 178.98  | 57.26   |
| DotFastLZ.Compression L2                 | 80.50   | 54.52   |
| DotFastLZ.Compression L1                 | 79.42   | 55.58   |
| System.IO.Compression.ZipArchive Fastest | 36.05   | 40.87   |
| K4os.Compression.LZ4 L03_HC              | 32.12   | 43.83   |
| K4os.Compression.LZ4 L09_HC              | 13.43   | 42.20   |
| System.IO.Compression.ZipArchive Optimal | 13.36   | 36.95   |
| K4os.Compression.LZ4 L12                 | 6.84    | 41.91   |

## Benchmark : decompression enwik/enwik8

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 13171.28 | 100.00   |
| K4os.Compression.LZ4 L00                 | 1451.09  | 57.26    |
| K4os.Compression.LZ4 L03_HC              | 972.60   | 43.83    |
| K4os.Compression.LZ4 L09_HC              | 953.98   | 42.20    |
| K4os.Compression.LZ4 L12                 | 937.86   | 41.91    |
| DotFastLZ.Compression L1                 | 203.07   | 55.58    |
| DotFastLZ.Compression L2                 | 193.43   | 54.52    |
| System.IO.Compression.ZipArchive Fastest | 128.98   | 40.87    |
| System.IO.Compression.ZipArchive Optimal | 128.40   | 36.95    |


## Overview

FastLZ (MIT license) is an ANSI C/C90 implementation of [Lempel-Ziv 77 algorithm](https://en.wikipedia.org/wiki/LZ77_and_LZ78#LZ77) (LZ77) of lossless data compression. It is suitable to compress series of text/paragraphs, sequences of raw pixel data, or any other blocks of data with lots of repetition. It is not intended to be used on images, videos, and other formats of data typically already in an optimal compressed form.

The focus for FastLZ is a very fast compression and decompression, doing that at the cost of the compression ratio. As an illustration, the comparison with zlib when compressing [enwik8](http://www.mattmahoney.net/dc/textdata.html) (also in [more details](https://github.com/inikep/lzbench)):

|         | Ratio | Compression | Decompression |
|---------|-------|-------------|---------------|
| FastLZ  | 54.2% | 159 MB/s    | 305 MB/s      |
| zlib -1 | 42.3% | 50 MB/s     | 184 MB/s      |
| zlib -9 | 36.5% | 11 MB/s     | 185 MB/s      |

FastLZ is used by many software products, from a number of games (such as [Death Stranding](https://en.wikipedia.org/wiki/Death_Stranding)) to various open-source projects ([Godot Engine](https://godotengine.org/), [Facebook HHVM](https://hhvm.com/), [Apache Traffic Server](https://trafficserver.apache.org/), [Calligra Office](https://www.calligra.org/), [OSv](http://osv.io/), [Netty](https://netty.io/), etc). It even serves as the basis for other compression projects like [BLOSC](https://blosc.org/).

For other implementations of byte-aligned LZ77, take a look at [LZ4](https://lz4.github.io/lz4/), [Snappy](http://google.github.io/snappy/), [Density](https://github.com/centaurean/density), [LZO](http://www.oberhumer.com/opensource/lzo/), [LZF](http://oldhome.schmorp.de/marc/liblzf.html), [LZJB](https://en.wikipedia.org/wiki/LZJB), [LZRW](http://www.ross.net/compression/lzrw1.html), etc.

## Block Format

Let us assume that FastLZ compresses an array of bytes, called the _uncompressed block_, into another array of bytes, called the _compressed block_. To understand what will be stored in the compressed block, it is illustrative to demonstrate how FastLZ will _decompress_ the block to retrieve the original uncompressed block.

The first 3-bit of the block, i.e. the 3 most-significant bits of the first byte, is the **block tag**. Currently the block tag determines the compression level used to produce the compressed block.

|Block tag|Compression level|
|---------|-----------------|
|   0     |    Level 1      |
|   1     |    Level 2      |

The content of the block will vary depending on the compression level.

### Block Format for Level 1

FastLZ Level 1 implements LZ77 compression algorithm with 8 KB sliding window and up to 264 bytes of match length.

The compressed block consists of one or more **instructions**.
Each instruction starts with a 1-byte opcode, 2-byte opcode, or 3-byte opcode.

| Instruction type | Opcode[0]                                        | Opcode[1]           | Opcode[2]           |
|------------------|--------------------------------------------------|---------------------|---------------------|
| Literal run      | `000`, L&#x2085;-L&#x2080;                       | -                   | -                   |
| Short match      | M&#x2082;-M&#x2080;, R&#x2081;&#x2082;-R&#x2088; | R&#x2087;-R&#x2080; | -                   |
| Long match       | `111`, R&#x2081;&#x2082;-R&#x2088;               | M&#x2087;-M&#x2080; | R&#x2087;-R&#x2080; |

Note that the _very first_ instruction in a compressed block is always a literal run.

#### Literal run instruction

For the literal run instruction, there is one or more bytes following the code. This is called the literal run.

The 5 least-significant bits of `opcode[0]`, _L_, determines the **number of literals** following the opcode. The value of 0 indicates a 1-byte literal run, 1 indicates a 2-byte literal run, and so on. The minimum literal run is 1 and the maximum literal run is 32.

The decompressor copies (_L + 1_) bytes of literal run, starting from the first one right after opcode.

_Example_: If the compressed block is a 4-byte array of `[0x02, 0x41, 0x42, 0x43]`, then the opcode is `0x02` and that means a literal run of 3 bytes. The decompressor will then copy the subsequent 3 bytes, `[0x41, 0x42, 0x43]`, to the output buffer. The output buffer now represents the (original) uncompressed block, `[0x41, 0x42, 0x43]`.

#### Short match instruction

The 3 most-significant bits of `opcode[0]`, _M_, determines the **match length**. The value of 1 indicates a 3-byte match, 2 indicates a 4-byte match and so on. The minimum match length is 3 and the maximum match length is 8.

The 5 least-significant bits of `opcode[0]` combined with the 8 bits of the `opcode[1]`, _R_, determines the **reference offset**. Since the offset is encoded in 13 bits, the minimum is 0 and the maximum is 8191.

The following C code retrieves the match length and reference offset:

```c
M = opcode[0] >> 5;
R = 256 * (opcode[0] << 5) + opcode[1];
```

The decompressor copies _(M+2)_ bytes, starting from the location offsetted by _R_ in the output buffer. Note that _R_ is a *back reference*, i.e. the value of 0 corresponds the last byte in the output buffer, 1 is the second to last byte, and so forth.

_Example 1_: If the compressed block is a 7-byte array of `[0x03, 0x41, 0x42, 0x43, 0x44, 0x20, 0x02]`, then there are two instructions in the there. The first instruction is the literal run of 4 bytes (due to _L = 3_). Thus, the decompressor copies 4 bytes to the output buffer, resulting in `[0x41, 0x42, 0x43, 0x44]`. The second instruction is the short match of 3 bytes (from _M = 1_, i.e `0x20 >> 5`) and the offset of 2. Therefore, the compressor goes back 2 bytes from the last position, copies 3 bytes (`[0x42, 0x43, 0x44]`), and appends them to the output buffer. The output buffer now represents the complete uncompressed data, `[0x41, 0x42, 0x43, 0x44, 0x42, 0x43, 0x44]`.

_Example 2_: If the compressed block is a 4-byte array of `[0x00, 0x61, 0x40, 0x00]`, then there are two instructions in there. The first instruction is the literal run of just 1 byte (_L = 0_). Thus, the decompressor copies the byte (`0x61`) to the output buffer. The output buffer now becomes `[0x61]`. The second instruction is the short match of 4 bytes (from _M = 2_, i.e. `0x40 >> 5`) and the offset of 0. Therefore, the decompressor copies 4 bytes starting using the back reference of 0 (i.e. the position of `0x61`). The output buffer now represents the complete uncompressed data, `[0x61, 0x61, 0x61, 0x61, 0x61]`.

#### Long match instruction

The value of `opcode[1]`, _M_, determines the **match length**. The value of 0 indicates a 9-byte match, 1 indicates a 10-byte match and so on. The minimum match length is 9 and the maximum match length is 264.

The 5 least-significant bits of `opcode[0]` combined with the 8 bits of `opcode[2]`, _R_, determines the **reference offset**. Since the offset is encoded in 13 bits, the minimum is 0 and the maximum is 8191.

The following C code retrieves the match length and reference offset:

```c
M = opcode[1];
R = 256 * (opcode[0] << 5) + opcode[2];
```
The decompressor copies _(M+9)_ bytes, starting from the location offsetted by _R_ in the output buffer. Note that _R_ is a *back reference*, i.e. the value of 0 corresponds to the last byte in the output buffer, 1 is for the second to last byte, and so forth.

_Example_:  If the compressed block is a 4-byte array of `[0x01, 0x44, 0x45, 0xE0, 0x01, 0x01]`, then there are two instructions in there. The first instruction is the literal run with the length of 2 (due to _L = 1_). Thus, the decompressor copies the 2-byte literal run (`[0x44, 0x45]`) to the output buffer. The second instruction is the long match with the match length of 10 (from _M = 1_) and the offset of 1. Therefore, the decompressor copies 10 bytes starting using the back reference of 1 (i.e. the position of `0x44`). The output buffer now represents the complete uncompressed data, `[0x44, 0x45, 0x44, 0x45, 0x44, 0x45, 0x44, 0x45, 0x44, 0x45, 0x44, 0x45]`.

#### Decompressor Reference Implementation

The following 40-line C function implements a fully-functional decompressor for the above block format. Note that it is intended to be educational, e.g. no bound check is implemented, and therefore it is absolutely **unsafe** for production.

```c
void fastlz_level1_decompress(const uint8_t* input, int length, uint8_t* output) {
  int src = 0;
  int dest = 0;
  while (src < length) {
    int type = input[src] >> 5;
    if (type == 0) {
      /* literal run */
      int run = 1 + input[src];
      src = src + 1;
      while (run > 0) {
        output[dest] = input[src];
        src = src + 1;
        dest = dest + 1;
        run = run - 1;
      }
    } else if (type < 7) {
      /* short match */
      int ofs = 256 * (input[src] & 31) + input[src + 1];
      int len = 2 + (input[src] >> 5);
      src = src + 2;
      int ref = dest - ofs - 1;
      while (len > 0) {
        output[dest] = output[ref];
        ref = ref + 1;
        dest = dest + 1;
        len = len - 1;
      }
    } else {
      /* long match */
      int ofs = 256 * (input[src] & 31) + input[src + 2];
      int len = 9 + input[src + 1];
      src = src + 3;
      int ref = dest - ofs - 1;
      while (len > 0) {
        output[dest] = output[ref];
        ref = ref + 1;
        dest = dest + 1;
        len = len - 1;
      }
    }
  }
}
```