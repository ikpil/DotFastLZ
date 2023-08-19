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