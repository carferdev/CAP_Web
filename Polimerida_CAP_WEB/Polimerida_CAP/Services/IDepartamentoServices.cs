using Polimerida_CAP.Models;

namespace Polimerida_CAP.Services
{
    public interface IDepartamentoServices
    {
        Task<IEnumerable<DepartamentoViewModel>> GetAllDepartamentosAsync();
        Task<DepartamentoViewModel> GetDepartamentoByIdAsync(int id);
        Task<DepartamentoViewModel> CreateDepartamentoAsync(DepartamentoViewModel departamento);
        Task<DepartamentoViewModel> UpdateDepartamentoAsync(int id, DepartamentoViewModel departamento);
        Task<bool> DeleteDepartamentoAsync(int id);
    }
} 