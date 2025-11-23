using System.Threading.Tasks;
using CheckFlow.Reports.Domain.Models;

namespace CheckFlow.Reports.Application.Services;

public interface IPhotoService
{
    /// <summary>
    ///     Verifica quais fotos existem e quais estão faltando dentro da pasta extraída.
    /// </summary>
    Task ValidatePhotosAsync(Checklist checklist, string extractedFolder);
}