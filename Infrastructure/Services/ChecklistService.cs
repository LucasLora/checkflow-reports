using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CheckFlow.Reports.Application.Services;
using CheckFlow.Reports.Domain.Models;

namespace CheckFlow.Reports.Infrastructure.Services;

public class ChecklistService : IChecklistService
{
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        AllowTrailingCommas = true
    };

    public async Task<Checklist> LoadChecklistAsync(string extractedFolder)
    {
        if (string.IsNullOrWhiteSpace(extractedFolder))
            throw new ArgumentException("extractedFolder is null or empty.", nameof(extractedFolder));

        if (!Directory.Exists(extractedFolder))
            throw new DirectoryNotFoundException($"Extracted folder not found: {extractedFolder}");

        var jsonPath = Directory.GetFiles(extractedFolder, "metadata.json", SearchOption.AllDirectories)
            .FirstOrDefault();

        if (jsonPath == null)
            throw new FileNotFoundException("metadata.json not found in extracted ZIP.", extractedFolder);

        var json = await File.ReadAllTextAsync(jsonPath);
        var checklist = JsonSerializer.Deserialize<Checklist>(json, _jsonOptions);

        return checklist ?? throw new Exception("Failed to deserialize metadata.json to Checklist.");
    }

    public void ValidateChecklist(Checklist checklist)
    {
        ArgumentNullException.ThrowIfNull(checklist);

        if (string.IsNullOrWhiteSpace(checklist.Name)) throw new Exception("Checklist name is required.");

        foreach (var item in checklist.Items)
        {
            if (string.IsNullOrWhiteSpace(item.Name)) throw new Exception($"Item {item.ItemId} name is required.");

            foreach (var photo in item.Photos)
            {
                if (string.IsNullOrWhiteSpace(photo.FileName))
                    throw new Exception($"Photo {photo.PhotoId} file name is required.");

                if (string.IsNullOrWhiteSpace(photo.Path))
                    throw new Exception($"Photo {photo.PhotoId} path is required.");
            }
        }
    }
}