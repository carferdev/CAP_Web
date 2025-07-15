using Polimerida_CAP.Models;

namespace Polimerida_CAP.Services
{
    public interface IPuestoServices
    {
        Task<IEnumerable<PuestoViewModel>> GetAllPuestosAsync();
        Task<PuestoViewModel> GetPuestoByIdAsync(int id);
        Task<PuestoViewModel> CreatePuestoAsync(PuestoViewModel puesto);
        Task<PuestoViewModel> UpdatePuestoAsync(int id, PuestoViewModel puesto);
        Task<bool> DeletePuestoAsync(int id);
    }
} 