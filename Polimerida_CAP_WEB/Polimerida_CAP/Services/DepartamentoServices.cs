using Microsoft.EntityFrameworkCore;
using Polimerida_CAP.Models;
using Polimerida_CAP.Services.Data;

namespace Polimerida_CAP.Services
{
    public class DepartamentoServices : IDepartamentoServices
    {
        private readonly AppDbContext _context;

        public DepartamentoServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DepartamentoViewModel>> GetAllDepartamentosAsync()
        {
            return await _context.Departamento   
                .Where(d => d.RegStatus   == "A")
                .Select(d => new DepartamentoViewModel
                {
                    IdDepartamento = (int)d.Iddepartamento  ,
                    Descripcion = d.Descripcion  ,
                    RegStatus = d.RegStatus  
                })
                .ToListAsync();
        }

        public async Task<DepartamentoViewModel> GetDepartamentoByIdAsync(int id)
        {
            var departamento = await _context.Departamento   .FindAsync((uint)id);
            if (departamento == null) return null;
            return new DepartamentoViewModel
            {
                IdDepartamento = (int)departamento.Iddepartamento  ,
                Descripcion = departamento.Descripcion  ,
                RegStatus = departamento.RegStatus  
            };
        }

        public async Task<DepartamentoViewModel> CreateDepartamentoAsync(DepartamentoViewModel departamento)
        {
            var entity = new Services.Data .Departamento  
            {
                Descripcion   = departamento.Descripcion,
                RegStatus   = departamento.RegStatus
            };
            _context.Departamento   .Add(entity );
            await _context.SaveChangesAsync();
            departamento.IdDepartamento = (int)entity.Iddepartamento  ;
            return departamento;
        }

        public async Task<DepartamentoViewModel> UpdateDepartamentoAsync(int id, DepartamentoViewModel departamento)
        {
            var entity = await _context.Departamento  .FindAsync((uint)id);
            if (entity == null) return null;

            entity.Descripcion   = departamento.Descripcion;
            entity.RegStatus   = departamento.RegStatus;
            await _context.SaveChangesAsync();
            return departamento;
        }

        public async Task<bool> DeleteDepartamentoAsync(int id)
        {
            var entity = await _context.Departamento  .FindAsync((uint)id);
            if (entity == null) return false;

            // Soft delete
            entity.RegStatus   = "B";
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 