using Polimerida_CAP.Models;
using Polimerida_CAP.Services.Data;
using System.Net.Http;
namespace Polimerida_CAP.Services;
public interface IDispositivoServices
{
    Task<IEnumerable<DispositivoViewModel>> GetAllAsync();
    Task<DispositivoViewModel?> GetByIdAsync(int id);
    Task<DispositivoViewModel> CreateAsync(DispositivoViewModel d);
    Task<DispositivoViewModel?> UpdateAsync(int id, DispositivoViewModel d);
    Task<bool> DeleteAsync(int id);
    Task<DeviceResponse> RegisterEmployeeWithFaceAsync(string ipAddress, Services.Data.Empleado employee, string? faceUrl = null);
    Task<DeviceResponse> EditarUsuarioConFaceAsync(string ipAddress, Empleado employee, string? faceUrl, bool imagenModificada);
    Task<DeviceResponse> EliminarUsuarioAsync(string ipAddress, Empleado employee);
} 