using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using CheckFlow.Reports.Application.Services;

namespace CheckFlow.Reports.Infrastructure.Services;

public class ZipService : IZipService
{
    public async Task<string> ExtractZipAsync(string zipPath)
    {
        if (string.IsNullOrWhiteSpace(zipPath))
            throw new ArgumentException("zipPath is null or empty.", nameof(zipPath));

        if (!File.Exists(zipPath))
            throw new FileNotFoundException("ZIP file not found.", zipPath);

        var tempDir = Path.Combine(Path.GetTempPath(), "CheckFlowReport_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);

        await Task.Run(() => { ZipFile.ExtractToDirectory(zipPath, tempDir); });

        return tempDir;
    }
}