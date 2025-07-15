using Polimerida_CAP.Models;

namespace Polimerida_CAP.Services;

public interface IEmpleadoServices
{
    Task<IEnumerable<EmpleadoViewModel>> GetAllAsync();
    Task<EmpleadoViewModel?> GetByIdAsync(int id);
    Task<(EmpleadoViewModel, DeviceResponse?)> CreateAsync(EmpleadoViewModel empleado);
    Task<(EmpleadoViewModel?, DeviceResponse?)> UpdateAsync(int id, EmpleadoViewModel empleado);
    Task<bool> DeleteAsync(int id, DateTime? fechaBaja = null);
} 