using System;
using System.IO;
using System.IO.Compression;

namespace DotFastLZ.Compression.Tests.Fixtures;

public static class ResourceHelper
{
    public static byte[] ToBytes(string filename)
    {
        var filepath = Find(filename);
        using var fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
        byte[] buffer = new byte[fs.Length];
        fs.Read(buffer, 0, buffer.Length);

        return buffer;
    }

    public static string Find(string filename)
    {
        string filePath = Path.Combine("resources", filename);
        for (int i = 0; i < 10; ++i)
        {
            if (File.Exists(filePath))
            {
                return Path.GetFullPath(filePath);
            }

            filePath = Path.Combine("..", filePath);
        }

        return Path.GetFullPath(filename);
    }

    public static int ExtractZipFile(string zipFilePath, string extractPath)
    {
        int count = 0;
        try
        {
            if (!Directory.Exists(extractPath))
                Directory.CreateDirectory(extractPath);

            using var archive = ZipFile.OpenRead(zipFilePath);
            foreach (ZipArchiveEntry entry in archive.Entries)
            {
                string entryPath = Path.Combine(extractPath, entry.FullName);

                if (entry.FullName.EndsWith("/"))
                {
                    Directory.CreateDirectory(entryPath);
                }
                else
                {
                    entry.ExtractToFile(entryPath, true);
                    Console.WriteLine($"extract file - {entryPath}");
                }

                count += 1;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return count;
    }
}