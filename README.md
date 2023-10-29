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

## Benchmark
CPU : AMD Ryzen 5 5625U

| Name                                                              | Rate  | MB/s   |
|-------------------------------------------------------------------|-------|--------|
| K4os.Compression.LZ4 compression L00 - silesia/x-ray              | 99.01 | 680.70 |
| K4os.Compression.LZ4 compression L00 - silesia/sao                | 93.63 | 247.47 |
| K4os.Compression.LZ4 compression L00 - silesia/mr                 | 54.57 | 235.90 |
| K4os.Compression.LZ4 compression L00 - silesia/mozilla            | 51.61 | 227.21 |
| K4os.Compression.LZ4 compression L00 - silesia/ooffice            | 70.53 | 216.67 |
| K4os.Compression.LZ4 compression L00 - silesia/samba              | 35.72 | 203.95 |
| K4os.Compression.LZ4 compression L00 - canterbury/xargs.1         | 62.95 | 202.69 |
| K4os.Compression.LZ4 compression L00 - canterbury/sum             | 49.20 | 202.66 |
| K4os.Compression.LZ4 compression L00 - canterbury/kennedy.xls     | 36.39 | 200.49 |
| K4os.Compression.LZ4 compression L00 - silesia/osdb               | 52.12 | 189.70 |
| K4os.Compression.LZ4 compression L00 - enwik/enwik8               | 57.26 | 186.09 |
| K4os.Compression.LZ4 compression L00 - silesia/dickens            | 63.07 | 183.56 |
| K4os.Compression.LZ4 compression L00 - canterbury/plrabn12.txt    | 67.57 | 181.00 |
| K4os.Compression.LZ4 compression L00 - silesia/reymont            | 48.01 | 174.78 |
| K4os.Compression.LZ4 compression L00 - silesia/webster            | 48.58 | 173.96 |
| K4os.Compression.LZ4 compression L00 - silesia/xml                | 22.96 | 172.04 |
| K4os.Compression.LZ4 compression L00 - silesia/nci                | 16.49 | 166.65 |
| K4os.Compression.LZ4 compression L00 - canterbury/ptt5            | 16.93 | 162.88 |
| K4os.Compression.LZ4 compression L00 - canterbury/asyoulik.txt    | 63.64 | 140.25 |
| DotFastLZ.Compression compression L2 - canterbury/kennedy.xls     | 40.08 | 115.64 |
| DotFastLZ.Compression compression L1 - canterbury/kennedy.xls     | 39.37 | 114.79 |
| K4os.Compression.LZ4 compression L00 - canterbury/lcet10.txt      | 54.65 | 111.52 |
| DotFastLZ.Compression compression L2 - silesia/sao                | 88.07 | 109.26 |
| DotFastLZ.Compression compression L2 - silesia/x-ray              | 96.70 | 107.14 |
| DotFastLZ.Compression compression L2 - silesia/osdb               | 52.65 | 98.09  |
| DotFastLZ.Compression compression L1 - silesia/x-ray              | 96.74 | 96.93  |
| DotFastLZ.Compression compression L1 - silesia/mr                 | 50.89 | 96.64  |
| DotFastLZ.Compression compression L2 - silesia/mozilla            | 51.66 | 93.46  |
| DotFastLZ.Compression compression L2 - silesia/mr                 | 50.59 | 91.63  |
| K4os.Compression.LZ4 compression L00 - canterbury/cp.html         | 48.40 | 90.26  |
| DotFastLZ.Compression compression L1 - silesia/mozilla            | 52.69 | 90.25  |
| DotFastLZ.Compression compression L1 - silesia/osdb               | 65.83 | 89.04  |
| DotFastLZ.Compression compression L2 - silesia/samba              | 35.45 | 87.08  |
| DotFastLZ.Compression compression L2 - canterbury/sum             | 49.40 | 86.64  |
| DotFastLZ.Compression compression L2 - silesia/ooffice            | 68.78 | 85.58  |
| DotFastLZ.Compression compression L1 - silesia/samba              | 38.61 | 85.04  |
| DotFastLZ.Compression compression L1 - silesia/nci                | 20.64 | 84.64  |
| DotFastLZ.Compression compression L1 - silesia/dickens            | 59.46 | 83.57  |
| DotFastLZ.Compression compression L2 - silesia/dickens            | 58.84 | 83.14  |
| DotFastLZ.Compression compression L1 - silesia/reymont            | 49.85 | 83.10  |
| DotFastLZ.Compression compression L1 - silesia/sao                | 88.08 | 82.99  |
| DotFastLZ.Compression compression L1 - silesia/xml                | 25.97 | 82.79  |
| DotFastLZ.Compression compression L2 - silesia/nci                | 19.60 | 82.71  |
| DotFastLZ.Compression compression L2 - silesia/reymont            | 47.60 | 82.70  |
| DotFastLZ.Compression compression L2 - canterbury/plrabn12.txt    | 61.85 | 82.58  |
| DotFastLZ.Compression compression L1 - enwik/enwik8               | 55.58 | 81.72  |
| DotFastLZ.Compression compression L1 - silesia/ooffice            | 69.48 | 81.44  |
| DotFastLZ.Compression compression L1 - silesia/webster            | 48.91 | 81.00  |
| DotFastLZ.Compression compression L2 - canterbury/lcet10.txt      | 53.70 | 80.78  |
| DotFastLZ.Compression compression L2 - silesia/webster            | 47.89 | 80.43  |
| DotFastLZ.Compression compression L2 - enwik/enwik8               | 54.52 | 79.77  |
| DotFastLZ.Compression compression L2 - silesia/xml                | 23.90 | 78.78  |
| DotFastLZ.Compression compression L1 - canterbury/sum             | 53.66 | 78.48  |
| DotFastLZ.Compression compression L2 - canterbury/xargs.1         | 58.46 | 77.26  |
| K4os.Compression.LZ4 compression L00 - canterbury/fields.c        | 46.80 | 76.82  |
| DotFastLZ.Compression compression L1 - canterbury/ptt5            | 15.84 | 72.88  |
| DotFastLZ.Compression compression L1 - canterbury/plrabn12.txt    | 62.37 | 70.88  |
| DotFastLZ.Compression compression L1 - canterbury/lcet10.txt      | 54.67 | 68.25  |
| DotFastLZ.Compression compression L2 - canterbury/ptt5            | 15.77 | 67.82  |
| DotFastLZ.Compression compression L2 - canterbury/cp.html         | 47.77 | 60.61  |
| DotFastLZ.Compression compression L1 - canterbury/xargs.1         | 58.46 | 58.91  |
| K4os.Compression.LZ4 compression L09_HC - canterbury/xargs.1      | 57.09 | 58.85  |
| K4os.Compression.LZ4 compression L03_HC - canterbury/xargs.1      | 57.23 | 54.74  |
| DotFastLZ.Compression compression L1 - canterbury/fields.c        | 42.46 | 51.63  |
| K4os.Compression.LZ4 compression L03_HC - canterbury/grammar.lsp  | 46.63 | 46.24  |
| DotFastLZ.Compression compression L1 - canterbury/cp.html         | 49.32 | 46.14  |
| DotFastLZ.Compression compression L2 - canterbury/fields.c        | 42.38 | 45.13  |
| K4os.Compression.LZ4 compression L03_HC - canterbury/sum          | 43.44 | 44.80  |
| K4os.Compression.LZ4 compression L09_HC - canterbury/grammar.lsp  | 46.41 | 43.43  |
| DotFastLZ.Compression compression L1 - canterbury/asyoulik.txt    | 59.54 | 42.33  |
| K4os.Compression.LZ4 compression L00 - canterbury/alice29.txt     | 58.32 | 39.19  |
| K4os.Compression.LZ4 compression L03_HC - canterbury/kennedy.xls  | 31.96 | 38.89  |
| K4os.Compression.LZ4 compression L03_HC - canterbury/fields.c     | 38.27 | 38.65  |
| DotFastLZ.Compression compression L2 - canterbury/grammar.lsp     | 47.89 | 38.36  |
| K4os.Compression.LZ4 compression L03_HC - silesia/mr              | 46.59 | 38.02  |
| K4os.Compression.LZ4 compression L03_HC - silesia/osdb            | 40.11 | 36.99  |
| K4os.Compression.LZ4 compression L03_HC - silesia/x-ray           | 84.99 | 36.39  |
| K4os.Compression.LZ4 compression L03_HC - silesia/mozilla         | 44.15 | 36.38  |
| DotFastLZ.Compression compression L2 - canterbury/asyoulik.txt    | 58.91 | 35.81  |
| K4os.Compression.LZ4 compression L03_HC - silesia/sao             | 80.96 | 35.57  |
| K4os.Compression.LZ4 compression L03_HC - silesia/ooffice         | 58.64 | 35.01  |
| DotFastLZ.Compression compression L1 - canterbury/alice29.txt     | 56.19 | 33.97  |
| K4os.Compression.LZ4 compression L09_HC - silesia/x-ray           | 84.67 | 33.10  |
| DotFastLZ.Compression compression L1 - canterbury/grammar.lsp     | 47.89 | 32.94  |
| K4os.Compression.LZ4 compression L03_HC - silesia/samba           | 29.20 | 32.02  |
| K4os.Compression.LZ4 compression L03_HC - enwik/enwik8            | 43.83 | 31.95  |
| K4os.Compression.LZ4 compression L03_HC - canterbury/asyoulik.txt | 49.55 | 30.91  |
| K4os.Compression.LZ4 compression L03_HC - canterbury/lcet10.txt   | 41.21 | 30.87  |
| K4os.Compression.LZ4 compression L03_HC - silesia/dickens         | 46.87 | 29.96  |
| K4os.Compression.LZ4 compression L03_HC - canterbury/plrabn12.txt | 50.64 | 29.39  |
| K4os.Compression.LZ4 compression L03_HC - silesia/reymont         | 36.64 | 28.57  |
| K4os.Compression.LZ4 compression L03_HC - silesia/webster         | 35.55 | 28.27  |
| DotFastLZ.Compression compression L2 - canterbury/alice29.txt     | 55.68 | 27.39  |
| K4os.Compression.LZ4 compression L03_HC - silesia/xml             | 15.95 | 26.24  |
| K4os.Compression.LZ4 compression L09_HC - canterbury/cp.html      | 42.03 | 25.89  |
| K4os.Compression.LZ4 compression L03_HC - canterbury/ptt5         | 13.55 | 25.33  |
| K4os.Compression.LZ4 compression L03_HC - silesia/nci             | 12.67 | 23.70  |
| K4os.Compression.LZ4 compression L09_HC - canterbury/fields.c     | 37.95 | 22.58  |
| K4os.Compression.LZ4 compression L12 - silesia/x-ray              | 84.64 | 22.51  |
| K4os.Compression.LZ4 compression L09_HC - silesia/sao             | 79.09 | 20.84  |
| K4os.Compression.LZ4 compression L12 - canterbury/xargs.1         | 56.87 | 20.53  |
| K4os.Compression.LZ4 compression L09_HC - silesia/osdb            | 39.44 | 20.43  |
| K4os.Compression.LZ4 compression L09_HC - silesia/ooffice         | 57.60 | 19.82  |
| K4os.Compression.LZ4 compression L09_HC - canterbury/sum          | 42.72 | 17.61  |
| K4os.Compression.LZ4 compression L09_HC - silesia/mozilla         | 43.11 | 15.98  |
| K4os.Compression.LZ4 compression L12 - canterbury/grammar.lsp     | 46.25 | 14.82  |
| K4os.Compression.LZ4 compression L03_HC - canterbury/cp.html      | 42.47 | 14.00  |
| K4os.Compression.LZ4 compression L12 - silesia/sao                | 78.17 | 13.91  |
| K4os.Compression.LZ4 compression L09_HC - enwik/enwik8            | 42.20 | 13.60  |
| K4os.Compression.LZ4 compression L09_HC - silesia/samba           | 28.42 | 12.30  |
| K4os.Compression.LZ4 compression L09_HC - canterbury/asyoulik.txt | 47.06 | 11.98  |
| K4os.Compression.LZ4 compression L12 - canterbury/cp.html         | 41.83 | 11.70  |
| K4os.Compression.LZ4 compression L12 - silesia/osdb               | 39.13 | 10.29  |
| K4os.Compression.LZ4 compression L09_HC - canterbury/lcet10.txt   | 38.85 | 10.15  |
| K4os.Compression.LZ4 compression L09_HC - silesia/webster         | 33.77 | 10.07  |
| K4os.Compression.LZ4 compression L12 - canterbury/fields.c        | 37.71 | 9.61   |
| K4os.Compression.LZ4 compression L12 - silesia/ooffice            | 57.46 | 9.39   |
| K4os.Compression.LZ4 compression L09_HC - canterbury/plrabn12.txt | 47.20 | 9.15   |
| K4os.Compression.LZ4 compression L09_HC - silesia/dickens         | 43.49 | 9.08   |
| K4os.Compression.LZ4 compression L09_HC - silesia/xml             | 14.41 | 8.40   |
| K4os.Compression.LZ4 compression L09_HC - silesia/mr              | 42.58 | 8.23   |
| K4os.Compression.LZ4 compression L12 - enwik/enwik8               | 41.91 | 6.96   |
| K4os.Compression.LZ4 compression L12 - canterbury/plrabn12.txt    | 46.62 | 5.93   |
| K4os.Compression.LZ4 compression L12 - canterbury/lcet10.txt      | 38.46 | 5.75   |
| K4os.Compression.LZ4 compression L12 - silesia/dickens            | 42.93 | 5.55   |
| K4os.Compression.LZ4 compression L12 - canterbury/sum             | 42.60 | 5.28   |
| K4os.Compression.LZ4 compression L03_HC - canterbury/alice29.txt  | 44.64 | 5.10   |
| K4os.Compression.LZ4 compression L12 - silesia/webster            | 33.34 | 5.04   |
| K4os.Compression.LZ4 compression L12 - canterbury/asyoulik.txt    | 46.58 | 4.89   |
| K4os.Compression.LZ4 compression L09_HC - silesia/reymont         | 31.86 | 4.81   |
| K4os.Compression.LZ4 compression L09_HC - canterbury/kennedy.xls  | 31.48 | 4.70   |
| K4os.Compression.LZ4 compression L09_HC - canterbury/ptt5         | 13.01 | 4.60   |
| K4os.Compression.LZ4 compression L12 - silesia/mr                 | 42.02 | 3.88   |
| K4os.Compression.LZ4 compression L09_HC - silesia/nci             | 10.95 | 3.83   |
| K4os.Compression.LZ4 compression L12 - silesia/samba              | 28.21 | 3.59   |
| K4os.Compression.LZ4 compression L12 - silesia/xml                | 14.22 | 3.48   |
| K4os.Compression.LZ4 compression L12 - silesia/reymont            | 31.13 | 3.11   |
| K4os.Compression.LZ4 compression L12 - silesia/mozilla            | 42.98 | 2.89   |
| K4os.Compression.LZ4 compression L00 - canterbury/grammar.lsp     | 51.46 | 2.67   |
| K4os.Compression.LZ4 compression L09_HC - canterbury/alice29.txt  | 41.87 | 2.47   |
| K4os.Compression.LZ4 compression L12 - silesia/nci                | 10.78 | 1.92   |
| K4os.Compression.LZ4 compression L12 - canterbury/alice29.txt     | 41.40 | 1.74   |
| K4os.Compression.LZ4 compression L12 - canterbury/ptt5            | 12.89 | 1.01   |
| K4os.Compression.LZ4 compression L12 - canterbury/kennedy.xls     | 31.48 | 0.29   |

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