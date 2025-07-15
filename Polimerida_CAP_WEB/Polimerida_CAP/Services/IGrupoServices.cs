using Polimerida_CAP.Models;

namespace Polimerida_CAP.Services
{
    public interface IGrupoServices
    {
        Task<IEnumerable<GrupoViewModel>> GetAllGruposAsync();
        Task<GrupoViewModel?> GetGrupoByIdAsync(int id);
        Task<GrupoViewModel?> GetGrupoByNombreAsync(string nombre);
        Task<DispositivoViewModel?> GetDispositivoByIdAsync(int id);
        Task<GrupoViewModel> CreateGrupoAsync(GrupoViewModel grupo);
        Task<GrupoViewModel?> UpdateGrupoAsync(int id, GrupoViewModel grupo);
        Task<bool> DeleteGrupoAsync(int id);
    }
} 