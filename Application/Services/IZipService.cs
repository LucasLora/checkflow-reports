namespace CheckFlow.Reports.Application.Services;

public interface IZipService
{
	/// <summary>
	///     Extracts the specified ZIP file to a temporary directory and returns the path to that directory.
	/// </summary>
	/// <param name="zipPath">Full path to the ZIP file.</param>
	/// <returns>Path to the temporary extraction folder.</returns>
	string ExtractZip(string zipPath);
}
