/*
  Copyright (C) 2023 Choi Ikpil <ikpil@naver.com> https://github.com/ikpil/DotFastLZ

  Permission is hereby granted, free of charge, to any person obtaining a copy
  of this software and associated documentation files (the "Software"), to deal
  in the Software without restriction, including without limitation the rights
  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
  copies of the Software, and to permit persons to whom the Software is
  furnished to do so, subject to the following conditions:

  The above copyright notice and this permission notice shall be included in
  all copies or substantial portions of the Software.

  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
  THE SOFTWARE.
*/


using System.IO;

namespace DotFastLZ.Compression.Tests.Fixtures;

public class SourceZip
{
    private readonly string _fileName;
    private readonly string _extractPath;

    public SourceZip(string fileName, string extractPath)
    {
        _fileName = fileName;
        _extractPath = extractPath;
    }

    public void Extract(string extractRootPath)
    {
        var zipFilePath = ResourceHelper.Find(_fileName);
        var directoryName = Path.GetDirectoryName(zipFilePath);
        var extractPath = Path.Combine(directoryName, extractRootPath, _extractPath);
        ResourceHelper.ExtractZipFile(zipFilePath, extractPath);
    }
}