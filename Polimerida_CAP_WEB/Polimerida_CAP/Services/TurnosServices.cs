using Polimerida_CAP.Models;
using Polimerida_CAP.Services.Data;
using Microsoft.EntityFrameworkCore;

namespace Polimerida_CAP.Services
{
    public class TurnosServices : IturnosServices
    {
        private readonly AppDbContext _context;

        // Constructor que recibe el contexto
        public TurnosServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TurnosViewModel>> GetAllTurnosAsync()
        {
            return await _context.Turno
                .Where(t => t.RegStatus == "A")
                .Select(t => new TurnosViewModel
                {
                    IdTurno = (int)t.Idturno,
                    HoraEntrada = t.Horaentrada,
                    HoraSalida = t.Horasalida,
                    Nombre = t.Nombre,
                    RegStatus = t.RegStatus,
                    Entradacomida = t.Entradacomida,
                    Salidacomida = t.Salidacomida
                })
                .ToListAsync();
        }

        public async Task<TurnosViewModel?> GetTurnoByIdAsync(int id)
        {
            var turno = await _context.Turno.FindAsync((uint)id);
            if (turno == null) return null;
            return new TurnosViewModel
            {
                IdTurno = (int)turno.Idturno,
                HoraEntrada = turno.Horaentrada,
                HoraSalida = turno.Horasalida,
                Nombre = turno.Nombre,
                RegStatus = turno.RegStatus,
                Entradacomida = turno.Entradacomida,
                Salidacomida = turno.Salidacomida
            };
        }

        public async Task<TurnosViewModel> CreateTurnoAsync(TurnosViewModel turno)
        {
            // Validar duplicados por hora de entrada y salida (ignorando el nombre)
            bool existe = await _context.Turno.AnyAsync(t =>
                t.Horaentrada.TimeOfDay == turno.HoraEntrada.TimeOfDay &&
                t.Horasalida.TimeOfDay == turno.HoraSalida.TimeOfDay &&
                t.RegStatus == "A");
            if (existe)
            {
                throw new InvalidOperationException("Ya existe un turno con la misma hora de entrada y salida.");
            }
            var entity = new Services.Data.Turno
            {
                Horaentrada = turno.HoraEntrada,
                Horasalida = turno.HoraSalida,
                Nombre = turno.Nombre,
                RegStatus = turno.RegStatus,
                Entradacomida = turno.Entradacomida,
                Salidacomida = turno.Salidacomida
            };
            _context.Turno.Add(entity);
            await _context.SaveChangesAsync();
            turno.IdTurno = (int)entity.Idturno;
            return turno;
        }

        public async Task<TurnosViewModel?> UpdateTurnoAsync(int id, TurnosViewModel turno)
        {
            var entity = await _context.Turno.FindAsync((uint)id);
            if (entity == null) return null;
            entity.Horaentrada = turno.HoraEntrada;
            entity.Horasalida = turno.HoraSalida;
            entity.Nombre = turno.Nombre;
            entity.RegStatus = turno.RegStatus;
            entity.Entradacomida = turno.Entradacomida;
            entity.Salidacomida = turno.Salidacomida;
            await _context.SaveChangesAsync();
            return turno;
        }

        public async Task<bool> DeleteTurnoAsync(int id)
        {
            var entity = await _context.Turno.FindAsync((uint)id);
            if (entity == null) return false;
            // Soft delete: marcamos como "B" (baja) en lugar de eliminar físicamente
            entity.RegStatus = "B";
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
