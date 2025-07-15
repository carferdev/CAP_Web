using Polimerida_CAP.Models;

namespace Polimerida_CAP.Services;

public interface IIncidenciaEmpleadoServices
{
    Task<IEnumerable<IncidenciaEmpleadoViewModel>> GetAllAsync();
    Task<IncidenciaEmpleadoViewModel?> GetByIdAsync(int id);
    Task<IncidenciaEmpleadoViewModel> CreateAsync(IncidenciaEmpleadoViewModel incidenciaEmpleado);
    Task<IncidenciaEmpleadoViewModel?> UpdateAsync(int id, IncidenciaEmpleadoViewModel incidenciaEmpleado);
    Task<bool> DeleteAsync(int id);
    Task<IncidenciaEmpleadoViewModel> GetCreateViewModelAsync();
} 