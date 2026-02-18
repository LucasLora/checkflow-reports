using System;
using System.IO;
using System.Linq;
using CheckFlow.Reports.Application.Services;
using CheckFlow.Reports.Domain.Enums;
using CheckFlow.Reports.Domain.Models;

namespace CheckFlow.Reports.Infrastructure.Services;

public class PhotoService : IPhotoService
{
	public void UpdatePhotoStatuses(Checklist checklist, string extractedFolder)
	{
		ArgumentNullException.ThrowIfNull(checklist);

		if (string.IsNullOrWhiteSpace(extractedFolder))
		{
			throw new ArgumentException("extractedFolder is null or empty.", nameof(extractedFolder));
		}

		if (!Directory.Exists(extractedFolder))
		{
			throw new DirectoryNotFoundException($"Extracted folder not found: {extractedFolder}");
		}

		foreach (var photo in checklist.Items.SelectMany(item => item.Photos))
		{
			if (photo.WasMissingOnExport)
			{
				photo.Status = PhotoStatus.MissingFromDevice;
				continue;
			}

			var relative =
				photo.Path?.Replace('/', Path.DirectorySeparatorChar)
					.Replace('\\', Path.DirectorySeparatorChar) ??
				photo.FileName;

			var fullPath = Path.Combine(extractedFolder, relative);
			var exists = File.Exists(fullPath);

			photo.Status = exists
				? PhotoStatus.Available
				: PhotoStatus.MissingFromZip;
		}
	}
}
