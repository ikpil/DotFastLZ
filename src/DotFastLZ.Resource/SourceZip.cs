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

namespace DotFastLZ.Resource;

public class SourceZip
{
    private readonly string _path;
    private readonly string _deletablePath;

    public SourceZip(string path, string deletablePath)
    {
        _path = path;
        _deletablePath = deletablePath;
    }

    public void Extract(string extractRootPath)
    {
        var zipFilePath = R.Find(_path);
        var directoryName = Path.GetDirectoryName(zipFilePath);
        if (null == directoryName)
        {
            throw new DirectoryNotFoundException($"not found directoryName - {zipFilePath}");
        }

        var extractPath = Path.Combine(directoryName, extractRootPath);
        R.ExtractZipFile(zipFilePath, extractPath);
    }

    public void Delete()
    {
        var deletePath = R.Find(_deletablePath);
        if (Directory.Exists(deletePath))
        {
            Directory.Delete(deletePath, true);
        }
        else if (File.Exists(deletePath))
        {
            File.Delete(deletePath);
        }
    }
}