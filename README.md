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