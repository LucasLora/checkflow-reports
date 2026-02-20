using System;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace CheckFlow.Reports.Infrastructure.Pdf;

public static class PdfHelper
{
	private const int MaxWidth = 1200;
	private const int MaxHeight = 1200;

	public static (string Path, bool IsPortrait) PrepareImageForPdf(string originalPath, string workingFolder)
	{
		var imagesDir = Path.Combine(workingFolder, "_pdf");
		Directory.CreateDirectory(imagesDir);

		var tempFile = Path.Combine(imagesDir, $"{Guid.NewGuid():N}.jpg");

		using var image = Image.Load(originalPath);

		image.Mutate(x => x.AutoOrient());

		var isPortrait = image.Height > image.Width;

		if (image.Width > MaxWidth || image.Height > MaxHeight)
		{
			image.Mutate(x => x.Resize(new ResizeOptions
			{
				Mode = ResizeMode.Max,
				Size = new Size(MaxWidth, MaxHeight)
			}));
		}

		image.SaveAsJpeg(tempFile, new JpegEncoder { Quality = 80 });

		return (tempFile, isPortrait);
	}
}
