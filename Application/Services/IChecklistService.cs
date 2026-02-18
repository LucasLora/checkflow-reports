using System.Threading.Tasks;
using CheckFlow.Reports.Domain.Models;

namespace CheckFlow.Reports.Application.Services;

public interface IChecklistService
{
	/// <summary>
	///     Lê o arquivo JSON na pasta extraída e retorna o checklist desserializado.
	/// </summary>
	Task<Checklist> LoadChecklistAsync(string extractedFolder);

	/// <summary>
	///     Valida estrutura e campos obrigatórios.
	/// </summary>
	void ValidateChecklist(Checklist checklist);
}
