using Microsoft.EntityFrameworkCore;
using Polimerida_CAP.Models;
using Polimerida_CAP.Services.Data;

namespace Polimerida_CAP.Services
{
    public class PuestoServices : IPuestoServices
    {
        private readonly AppDbContext _context;

        public PuestoServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PuestoViewModel>> GetAllPuestosAsync()
        {
            return await _context.Puesto  
                .Where(p => p.RegStatus   == "A")
                .Select(p => new PuestoViewModel
                {
                    IdPuesto = (int)p.Idpuesto  ,
                    Descripcion = p.Descripcion  ,
                    RegStatus = p.RegStatus  
                })
                .ToListAsync();
        }
        
        public async Task<PuestoViewModel> GetPuestoByIdAsync(int id)
        {
            var puesto = await _context.Puesto  .FindAsync((uint)id);
            if (puesto == null) return null;
            return new PuestoViewModel
            {
                IdPuesto = (int)puesto.Idpuesto  ,
                Descripcion = puesto.Descripcion  ,
                RegStatus = puesto.RegStatus  
            };
        }

        public async Task<PuestoViewModel> CreatePuestoAsync(PuestoViewModel puesto)
        {
            var entity = new Services.Data.Puesto
            {
                Descripcion = puesto.Descripcion,
                RegStatus = puesto.RegStatus
            };
            _context.Puesto  .Add(entity);
            await _context.SaveChangesAsync();
            puesto.IdPuesto = (int)entity.Idpuesto;
            return puesto;
        }

        public async Task<PuestoViewModel> UpdatePuestoAsync(int id, PuestoViewModel puesto)
        {
            var entity = await _context.Puesto  .FindAsync((uint)id);
            if (entity == null) return null;
            entity.Descripcion   = puesto.Descripcion;
            entity.RegStatus   = puesto.RegStatus;
            await _context.SaveChangesAsync();
            return puesto;
        }

        public async Task<bool> DeletePuestoAsync(int id)
        {
            var entity = await _context.Puesto  .FindAsync((uint)id);
            if (entity == null) return false;
            entity.RegStatus   = "B";
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 