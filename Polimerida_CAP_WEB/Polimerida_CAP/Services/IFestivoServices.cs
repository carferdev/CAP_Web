using Polimerida_CAP.Models;

namespace Polimerida_CAP.Services;

public interface IFestivoServices
{
    Task<IEnumerable<FestivoViewModel>> GetAllAsync();
    Task<FestivoViewModel?> GetByIdAsync(int id);
    Task<FestivoViewModel> CreateAsync(FestivoViewModel festivo);
    Task<FestivoViewModel?> UpdateAsync(int id, FestivoViewModel festivo);
    Task<bool> DeleteAsync(int id);
} 