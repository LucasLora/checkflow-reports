using System;
using System.IO;
using Avalonia.Platform;
using CheckFlow.Reports.Domain.Enums;
using CheckFlow.Reports.Domain.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace CheckFlow.Reports.Infrastructure.Pdf;

public class ChecklistReportDocument(Checklist checklist, string rootFolder) : IDocument
{
	public DocumentMetadata GetMetadata()
	{
		return new DocumentMetadata
		{
			Title = checklist.Title
		};
	}

	public void Compose(IDocumentContainer container)
	{
		container.Page(page =>
		{
			page.Size(PageSizes.A4);
			page.Margin(36);
			page.Content().Element(ComposeContent);
		});
	}

	private static byte[] LoadBanner()
	{
		try
		{
			var uri = new Uri("avares://CheckFlow.Reports/Assets/ReportBanner.png");
			using var stream = AssetLoader.Open(uri);
			using var ms = new MemoryStream();
			stream.CopyTo(ms);
			return ms.ToArray();
		}
		catch
		{
			return [];
		}
	}

	private void ComposeContent(IContainer container)
	{
		container.Column(col =>
			{
				FirstPageHeader(col);
				ItemsListing(col);
			}
		);
	}

	private void FirstPageHeader(ColumnDescriptor col)
	{
		var banner = LoadBanner();
		if (banner.Length > 0)
		{
			col.Item()
				.AlignCenter()
				.Width(125)
				.Image(banner);
		}
		else
		{
			col.Item()
				.AlignCenter()
				.Text("Unable to load banner image.")
				.Italic();
		}

		col.Item().AlignCenter()
			.Text(checklist.Title)
			.FontSize(20)
			.Bold();

		col.Item()
			.Height(10);
	}

	private void ItemsListing(ColumnDescriptor col)
	{
		col.Item().PaddingVertical(10).Column(column =>
		{
			foreach (var item in checklist.Items)
			{
				column.Item().Column(x =>
				{
					x.Item()
						.Text(item.Title)
						.FontSize(14)
						.Bold();

					x.Item()
						.Height(5);

					PhotosListing(item, x);
				});

				column.Item()
					.PaddingVertical(5)
					.LineHorizontal(1f);
			}
		});
	}

	private void PhotosListing(Item item, ColumnDescriptor col)
	{
		foreach (var photo in item.Photos)
		{
			switch (photo.Status)
			{
				case PhotoStatus.MissingFromDevice:
					col.Item()
						.Text("Foto ausente no dispositivo durante a exportação.")
						.Italic();
					break;

				case PhotoStatus.MissingFromZip:
					col.Item()
						.Text("Foto não encontrada dentro do arquivo ZIP.")
						.Italic();
					break;

				case PhotoStatus.Available:
					var relativePath = photo.Path?
						                   .Replace('\\', Path.DirectorySeparatorChar)
						                   .Replace('/', Path.DirectorySeparatorChar)
					                   ?? photo.FileName;

					var fullPath = Path.Combine(rootFolder, relativePath);

					if (File.Exists(fullPath))
					{
						col.Item()
							.Image(fullPath)
							.FitWidth();
					}
					else
					{
						col.Item()
							.Text("Arquivo da foto não encontrado no disco.")
							.Italic();
					}

					break;

				default:
					throw new NotSupportedException(
						$"Unsupported photo status in report generation: {photo.Status}"
					);
			}

			col.Item()
				.Height(5);
		}
	}
}
