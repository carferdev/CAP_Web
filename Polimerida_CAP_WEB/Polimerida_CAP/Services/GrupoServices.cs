using Microsoft.EntityFrameworkCore;
using Polimerida_CAP.Models;
using Polimerida_CAP.Services.Data;

namespace Polimerida_CAP.Services
{
    public class GrupoServices : IGrupoServices
    {
        private readonly AppDbContext _context;

        public GrupoServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GrupoViewModel>> GetAllGruposAsync()
        {
            return await _context.Grupo
                .Where(g => g.RegStatus == "A")
                .Select(g => new GrupoViewModel
                {
                    IdGrupo = (int)g.Idgrupo,
                    Nombre = g.Descripcion,
                    Descripcion = g.Descripcion,
                    RegStatus = g.RegStatus  
                })
                .ToListAsync();
        }

        public async Task<GrupoViewModel?> GetGrupoByIdAsync(int id)
        {
            var grupo = await _context.Grupo.FindAsync((uint)id);
            if (grupo == null) return null;
            return new GrupoViewModel
            {
                IdGrupo = (int)grupo.Idgrupo,
                Nombre = grupo.Descripcion,
                Descripcion = grupo.Descripcion,
                RegStatus = grupo.RegStatus  
            };
        }

        public async Task<GrupoViewModel?> GetGrupoByNombreAsync(string nombre)
        {
            var grupo = await _context.Grupo.FirstOrDefaultAsync(g => g.Nombre == nombre && g.RegStatus == "A");
            if (grupo == null) return null;
            return new GrupoViewModel
            {
                IdGrupo = (int)grupo.Idgrupo,
                Nombre = grupo.Nombre,
                Descripcion = grupo.Descripcion,
                RegStatus = grupo.RegStatus
            };
        }

        public async Task<DispositivoViewModel?> GetDispositivoByIdAsync(int id)
        {
            var dispositivo = await _context.Dispositivo.FindAsync((uint)id);
            if (dispositivo == null) return null;
            return new DispositivoViewModel
            {
                IdDispositivo = (int)dispositivo.Iddispositivo,
                Descripcion = dispositivo.Descripcion,
                Clase = dispositivo.Clase,
                Division = dispositivo.Division,
                Tipo = dispositivo.Tipo,
                Ip = dispositivo.Ip,
                RegStatus = dispositivo.RegStatus
            };
        }

        public async Task<GrupoViewModel> CreateGrupoAsync(GrupoViewModel grupo)
        {
            var entity = new Services.Data.Grupo
            {
                Descripcion = grupo.Descripcion,
                RegStatus = grupo.RegStatus
            };
            _context.Grupo.Add(entity);
            await _context.SaveChangesAsync();
            grupo.IdGrupo = (int)entity.Idgrupo;
            return grupo;
        }

        public async Task<GrupoViewModel?> UpdateGrupoAsync(int id, GrupoViewModel grupo)
        {
            var entity = await _context.Grupo.FindAsync((uint)id);
            if (entity == null) return null;
            entity.Descripcion = grupo.Descripcion;
            entity.RegStatus = grupo.RegStatus;
            await _context.SaveChangesAsync();
            return grupo;
        }

        public async Task<bool> DeleteGrupoAsync(int id)
        {
            var entity = await _context.Grupo.FindAsync((uint)id);
            if (entity == null) return false;
            entity.RegStatus = "B";
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 