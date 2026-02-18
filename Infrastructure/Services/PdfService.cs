using System;
using System.IO;
using CheckFlow.Reports.Application.Services;
using CheckFlow.Reports.Domain.Models;
using CheckFlow.Reports.Infrastructure.Pdf;
using QuestPDF.Fluent;

namespace CheckFlow.Reports.Infrastructure.Services;

public class PdfService : IPdfService
{
	public void GeneratePdf(Checklist checklist, string tempDir, string outputFolder, string outputFilePath)
	{
		ArgumentNullException.ThrowIfNull(checklist);

		if (string.IsNullOrWhiteSpace(tempDir))
		{
			throw new ArgumentException("tempDir is null or empty.", nameof(tempDir));
		}

		if (string.IsNullOrWhiteSpace(outputFolder))
		{
			throw new ArgumentException("outputFolder is null or empty.", nameof(outputFolder));
		}

		if (string.IsNullOrWhiteSpace(outputFilePath))
		{
			throw new ArgumentException("outputFilePath is null or empty.", nameof(outputFilePath));
		}

		if (!Directory.Exists(outputFolder))
		{
			Directory.CreateDirectory(outputFolder);
		}

		var document = new ChecklistReportDocument(checklist, tempDir);
		document.GeneratePdf(outputFilePath);
	}
}
