using Microsoft.EntityFrameworkCore;
using Polimerida_CAP.Models;
using Polimerida_CAP.Services.Data;

namespace Polimerida_CAP.Services;

public class IncidenciaServices : IIncidenciaServices
{
    private readonly AppDbContext _ctx;

    public IncidenciaServices(AppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<IEnumerable<IncidenciaViewModel>> GetAllAsync()
    {
        return await _ctx.Incidencias
            .Where(i => i.RegStatus == "A")
            .Select(i => new IncidenciaViewModel
            {
                IdIncidencia = (int)i.Idincidencia,
                Codigo = i.Codigo,
                Descripcion = i.Descripcion,
                RegStatus = i.RegStatus
            })
            .ToListAsync();
    }

    public async Task<IncidenciaViewModel?> GetByIdAsync(int id)
    {
        var incidencia = await _ctx.Incidencias.FindAsync((uint)id);
        if (incidencia == null) return null;

        return new IncidenciaViewModel
        {
            IdIncidencia = (int)incidencia.Idincidencia,
            Codigo = incidencia.Codigo,
            Descripcion = incidencia.Descripcion,
            RegStatus = incidencia.RegStatus
        };
    }

    public async Task<IncidenciaViewModel> CreateAsync(IncidenciaViewModel incidencia)
    {
        var entity = new Services.Data.Incidencias
        {
            Codigo = incidencia.Codigo,
            Descripcion = incidencia.Descripcion,
            RegStatus = incidencia.RegStatus
        };

        _ctx.Incidencias.Add(entity);
        await _ctx.SaveChangesAsync();
        incidencia.IdIncidencia = (int)entity.Idincidencia;
        return incidencia;
    }

    public async Task<IncidenciaViewModel?> UpdateAsync(int id, IncidenciaViewModel incidencia)
    {
        var entity = await _ctx.Incidencias.FindAsync((uint)id);
        if (entity == null) return null;

        entity.Codigo = incidencia.Codigo;
        entity.Descripcion = incidencia.Descripcion;
        entity.RegStatus = incidencia.RegStatus;

        await _ctx.SaveChangesAsync();
        return incidencia;
    }

    public async Task<bool> DeleteAsync(int id, DateTime? fechaBaja = null, string? regStatus = null)
    {
        var entity = await _ctx.Incidencias.FindAsync((uint)id);
        if (entity == null) return false;

        // Usar el RegStatus proporcionado o por defecto "B" (inactivo)
        entity.RegStatus = regStatus ?? "B";
        
        // Nota: La tabla de incidencias no tiene campo de fecha de baja
        // pero mantenemos el parámetro para consistencia con la interfaz
        
        await _ctx.SaveChangesAsync();
        return true;
    }
} 