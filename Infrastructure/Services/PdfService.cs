using System;
using System.IO;
using System.Threading.Tasks;
using CheckFlow.Reports.Application.Services;
using CheckFlow.Reports.Domain.Models;
using CheckFlow.Reports.Infrastructure.Services.Documents;
using QuestPDF.Fluent;

namespace CheckFlow.Reports.Infrastructure.Services;

public class PdfService : IPdfService
{
	public Task GeneratePdfAsync(Checklist checklist, string tempDir, string outputFolder, string outputFile)
	{
		ArgumentNullException.ThrowIfNull(checklist);

		if (string.IsNullOrWhiteSpace(tempDir))
			throw new ArgumentException("tempDir is null or empty.", nameof(tempDir));

		if (string.IsNullOrWhiteSpace(outputFolder))
			throw new ArgumentException("outputFolder is null or empty.", nameof(outputFolder));

		if (string.IsNullOrWhiteSpace(outputFile))
			throw new ArgumentException("outputFilePath is null or empty.", nameof(outputFile));

		if (!Directory.Exists(outputFolder))
			Directory.CreateDirectory(outputFolder);

		var document = new ChecklistDocument(checklist, tempDir);
		document.GeneratePdf(outputFile);

		return Task.CompletedTask;
	}
}
