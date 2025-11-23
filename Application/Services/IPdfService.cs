using System.Threading.Tasks;
using CheckFlow.Reports.Domain.Models;

namespace CheckFlow.Reports.Application.Services;

public interface IPdfService
{
    /// <summary>
    ///     Gera o PDF final do checklist.
    /// </summary>
    Task GeneratePdfAsync(Checklist checklist, string tempDir, string outputFolder, string outputFile);
}