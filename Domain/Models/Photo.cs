using System;
using CheckFlow.Reports.Domain.Enums;

namespace CheckFlow.Reports.Domain.Models;

/// <summary>
///     Represents a photo attached to a checklist item.
/// </summary>
public class Photo
{
	public int PhotoId { get; set; }
	public string FileName { get; set; } = string.Empty;

	/// <summary>
	///     Relative path of the photo inside the exported ZIP archive (e.g., "photos/1_photo.jpg").
	/// </summary>
	public string Path { get; set; } = string.Empty;

	public DateTime PhotoAttachedAt { get; set; }

	/// <summary>
	///     Indicates whether the photo was missing on the device at the time the checklist was exported
	///     from the mobile application.
	/// </summary>
	public bool WasMissingOnExport { get; set; }

	/// <summary>
	///     Current processing status of the photo during report generation.
	/// </summary>
	public PhotoStatus Status { get; set; } = PhotoStatus.Available;
}
