using System;
using System.IO;
using System.Threading.Tasks;
using CheckFlow.Reports.Application.Services;
using CheckFlow.Reports.Domain.Models;
using QuestPDF.Fluent;

namespace CheckFlow.Reports.Infrastructure.Services;

public class PdfService : IPdfService
{
    public Task GeneratePdfAsync(Checklist checklist, string outputFolder)
    {
        ArgumentNullException.ThrowIfNull(checklist);

        if (string.IsNullOrWhiteSpace(outputFolder))
            throw new ArgumentException("outputFolder is null or empty.", nameof(outputFolder));

        if (!Directory.Exists(outputFolder))
            Directory.CreateDirectory(outputFolder);

        var safeName = string.Concat(checklist.Name.Split(Path.GetInvalidFileNameChars()));
        var outputFile = Path.Combine(outputFolder, $"{safeName}_report.pdf");

        Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);

                    page.Header().Text($"Checklist: {checklist.Name}").FontSize(20).Bold();
                    page.Content().Column(column =>
                    {
                        column.Item().PaddingTop(8).Text($"Items ({checklist.Items.Count}):").FontSize(12).Bold();

                        foreach (var item in checklist.Items)
                            column.Item().PaddingTop(6).Column(it =>
                            {
                                it.Item().Text($"{item.Name}").FontSize(10).Bold();
                                it.Item().Text($"Photos: {item.Photos?.Count ?? 0}").FontSize(9);

                                if (item.Photos is not { Count: > 0 }) return;
                                foreach (var p in item.Photos)
                                    it.Item().Text($"  • {p.FileName} {(p.Missing ? "(missing)" : "")}").FontSize(9);
                            });
                    });

                    page.Footer().AlignCenter().Text(x => x.CurrentPageNumber());
                });
            })
            .GeneratePdf(outputFile);

        return Task.CompletedTask;
    }
}