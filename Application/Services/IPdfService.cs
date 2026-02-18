using CheckFlow.Reports.Domain.Models;

namespace CheckFlow.Reports.Application.Services;

public interface IPdfService
{
	/// <summary>
	///     Generates the final PDF document for the specified checklist and saves it to the output folder.
	/// </summary>
	/// <param name="checklist">Checklist used to generate the PDF.</param>
	/// <param name="tempDir">Temporary directory used during PDF generation.</param>
	/// <param name="outputFolder">Target folder for the generated PDF.</param>
	/// <param name="outputFilePath">Output file path.</param>
	void GeneratePdf(Checklist checklist, string tempDir, string outputFolder, string outputFilePath);
}
