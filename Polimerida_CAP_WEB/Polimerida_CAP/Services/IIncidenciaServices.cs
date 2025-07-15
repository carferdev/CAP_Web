using Polimerida_CAP.Models;

namespace Polimerida_CAP.Services;

public interface IIncidenciaServices
{
    Task<IEnumerable<IncidenciaViewModel>> GetAllAsync();
    Task<IncidenciaViewModel?> GetByIdAsync(int id);
    Task<IncidenciaViewModel> CreateAsync(IncidenciaViewModel incidencia);
    Task<IncidenciaViewModel?> UpdateAsync(int id, IncidenciaViewModel incidencia);
    Task<bool> DeleteAsync(int id, DateTime? fechaBaja = null, string? regStatus = null);
} 