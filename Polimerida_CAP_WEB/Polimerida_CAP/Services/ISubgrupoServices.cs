using Polimerida_CAP.Models;
namespace Polimerida_CAP.Services;
public interface ISubgrupoServices
{
    Task<IEnumerable<SubgrupoViewModel>> GetAllAsync();
    Task<SubgrupoViewModel?> GetByIdAsync(int id);
    Task<SubgrupoViewModel> CreateAsync(SubgrupoViewModel vm);
    Task<SubgrupoViewModel?> UpdateAsync(int id, SubgrupoViewModel vm);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<SubgrupoViewModel>> GetByGrupoAsync(int idGrupo);
} 