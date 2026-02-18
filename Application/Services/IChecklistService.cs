using System.Threading.Tasks;
using CheckFlow.Reports.Domain.Models;

namespace CheckFlow.Reports.Application.Services;

public interface IChecklistService
{
	/// <summary>
	///     Loads and deserializes the checklist metadata from the extracted folder.
	/// </summary>
	/// <param name="extractedFolder">Path to the folder containing the extracted checklist files.</param>
	Task<Checklist> LoadChecklistAsync(string extractedFolder);

	/// <summary>
	///     Validates the checklist structure and required fields.
	///     Throws an exception if validation fails.
	/// </summary>
	/// <param name="checklist">Checklist instance to validate.</param>
	void ValidateChecklist(Checklist checklist);
}
