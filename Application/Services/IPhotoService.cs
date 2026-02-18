using CheckFlow.Reports.Domain.Models;

namespace CheckFlow.Reports.Application.Services;

public interface IPhotoService
{
	/// ///
	/// <summary>
	///     Updates photo statuses based on their presence in the extracted folder.
	/// </summary>
	/// <param name="checklist">Checklist containing the photos to evaluate.</param>
	/// <param name="extractedFolder">Path to the extracted checklist folder.</param>
	void UpdatePhotoStatuses(Checklist checklist, string extractedFolder);
}
