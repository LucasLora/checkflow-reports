using System;
using System.IO;
using System.Threading.Tasks;
using CheckFlow.Reports.Application.Services;
using CheckFlow.Reports.Domain.Enums;
using CheckFlow.Reports.Domain.Models;

namespace CheckFlow.Reports.Infrastructure.Services;

public class PhotoService : IPhotoService
{
	public async Task ValidatePhotosAsync(Checklist checklist, string extractedFolder)
	{
		ArgumentNullException.ThrowIfNull(checklist);

		if (string.IsNullOrWhiteSpace(extractedFolder))
			throw new ArgumentException("extractedFolder is null or empty.", nameof(extractedFolder));

		if (!Directory.Exists(extractedFolder))
			throw new DirectoryNotFoundException($"Extracted folder not found: {extractedFolder}");

		await Task.Run(() =>
		{
			foreach (var item in checklist.Items)
				foreach (var photo in item.Photos)
				{
					if (photo.Status == PhotoStatus.MissingOnDevice)
						continue;

					var relative =
						photo.Path?.Replace('/', Path.DirectorySeparatorChar)
							.Replace('\\', Path.DirectorySeparatorChar) ??
						photo.FileName;

					var fullPath = Path.Combine(extractedFolder, relative);
					var exists = File.Exists(fullPath);

					photo.Status = exists ? PhotoStatus.Ok : PhotoStatus.MissingInZip;
				}
		});
	}
}
