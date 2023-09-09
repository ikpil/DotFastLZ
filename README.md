[![License](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)
[![.NET](https://github.com/ikpil/DotFastLZ/actions/workflows/dotnet.yml/badge.svg)](https://github.com/ikpil/DotFastLZ/actions/workflows/dotnet.yml)
[![CodeQL](https://github.com/ikpil/DotFastLZ/actions/workflows/github-code-scanning/codeql/badge.svg)](https://github.com/ikpil/DotFastLZ/actions/workflows/github-code-scanning/codeql)

## Introduction

DotFastLZ is a C# port of [ariya/FastLZ](https://github.com/ariya/FastLZ) for C# projects and Unity3D.

## Overview

FastLZ (MIT license) is an ANSI C/C90 implementation of [Lempel-Ziv 77 algorithm](https://en.wikipedia.org/wiki/LZ77_and_LZ78#LZ77) (LZ77) of lossless data compression. It is suitable to compress series of text/paragraphs, sequences of raw pixel data, or any other blocks of data with lots of repetition. It is not intended to be used on images, videos, and other formats of data typically already in an optimal compressed form.

The focus for FastLZ is a very fast compression and decompression, doing that at the cost of the compression ratio. As an illustration, the comparison with zlib when compressing [enwik8](http://www.mattmahoney.net/dc/textdata.html) (also in [more details](https://github.com/inikep/lzbench)):