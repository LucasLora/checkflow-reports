using System.Threading.Tasks;

namespace CheckFlow.Reports.Application.Services;

public interface IZipService
{
	/// <summary>
	///     Extrai o arquivo ZIP para uma pasta temporária e retorna o caminho da pasta.
	/// </summary>
	Task<string> ExtractZipAsync(string zipPath);
}
