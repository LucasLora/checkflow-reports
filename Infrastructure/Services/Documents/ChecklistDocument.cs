using System;
using System.IO;
using Avalonia.Platform;
using CheckFlow.Reports.Domain.Enums;
using CheckFlow.Reports.Domain.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace CheckFlow.Reports.Infrastructure.Services.Documents;

public class ChecklistDocument(Checklist checklist, string rootFolder) : IDocument
{
	public DocumentMetadata GetMetadata()
	{
		return new DocumentMetadata
		{
			Title = checklist.Name
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
			col.Item()
				.AlignCenter()
				.Width(125)
				.Image(banner);
		else
			col.Item()
				.AlignCenter()
				.Text("Não foi possível carregar o banner")
				.Italic();

		col.Item().AlignCenter()
			.Text(checklist.Name)
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
						.Text(item.Name)
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
				case PhotoStatus.MissingOnDevice:
					col.Item()
						.Text("Foto removida do dispositivo antes da exportação.")
						.Italic();
					break;

				case PhotoStatus.MissingInZip:
					col.Item()
						.Text("Foto não encontrada no arquivo ZIP.")
						.Italic();
					break;

				case PhotoStatus.Ok:
					var fullPath = Path.Combine(
						rootFolder,
						photo.Path?.Replace('\\', Path.DirectorySeparatorChar)
							?.Replace('/', Path.DirectorySeparatorChar)
						?? photo.FileName
					);

					if (File.Exists(fullPath))
						col.Item()
							.Image(fullPath)
							.FitWidth();
					else
						col.Item()
							.Text("Arquivo de imagem não encontrado no disco.")
							.Italic();
					break;

				default:
					throw new NotSupportedException(
						$"Photo status não configurado no relatório: {photo.Status}");
			}

			col.Item()
				.Height(5);
		}
	}
}
