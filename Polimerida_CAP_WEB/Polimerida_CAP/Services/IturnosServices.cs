using Polimerida_CAP.Models;

namespace Polimerida_CAP.Services
{
    public interface IturnosServices
    {
        //Add methods for crud operations on turnos table urig for TurnosViewModel
        Task<IEnumerable<TurnosViewModel>> GetAllTurnosAsync();
        Task<TurnosViewModel?> GetTurnoByIdAsync(int id);
        Task<TurnosViewModel> CreateTurnoAsync(TurnosViewModel turno);
        Task<TurnosViewModel?> UpdateTurnoAsync(int id, TurnosViewModel turno);
        Task<bool> DeleteTurnoAsync(int id);
    }
}
