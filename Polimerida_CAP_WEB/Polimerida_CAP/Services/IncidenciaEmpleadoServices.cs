using Microsoft.EntityFrameworkCore;
using Polimerida_CAP.Models;
using Polimerida_CAP.Services.Data;

namespace Polimerida_CAP.Services;

public class IncidenciaEmpleadoServices : IIncidenciaEmpleadoServices
{
    private readonly AppDbContext _ctx;
    private readonly IEmpleadoServices _empleadoServices;
    private readonly IIncidenciaServices _incidenciaServices;

    public IncidenciaEmpleadoServices(AppDbContext ctx, IEmpleadoServices empleadoServices, IIncidenciaServices incidenciaServices)
    {
        _ctx = ctx;
        _empleadoServices = empleadoServices;
        _incidenciaServices = incidenciaServices;
    }

    public async Task<IEnumerable<IncidenciaEmpleadoViewModel>> GetAllAsync()
    {
        return await _ctx.Incidenciaxempleado
            .Where(ie => ie.RegStatus == "A")
            .Include(ie => ie.IdempleadoNavigation)
            .Include(ie => ie.IdincidenciaNavigation)
            .Select(ie => new IncidenciaEmpleadoViewModel
            {
                IdIncidenciaEmpleado = (int)ie.Idincidenciaxempleado,
                IdEmpleado = (int)ie.Idempleado,
                NombreEmpleado = $"{ie.IdempleadoNavigation.Primernombre} {ie.IdempleadoNavigation.Apellidopaterno} {ie.IdempleadoNavigation.Apellidomaterno}".Trim(),
                IdIncidencia = (int)ie.Idincidencia,
                DescripcionIncidencia = ie.IdincidenciaNavigation.Descripcion,
                FechaInicio = ie.Fechainicio,
                FechaFin = ie.Fechafin,
                RegStatus = ie.RegStatus
            })
            .ToListAsync();
    }

    public async Task<IncidenciaEmpleadoViewModel?> GetByIdAsync(int id)
    {
        var incidenciaEmpleado = await _ctx.Incidenciaxempleado
            .Include(ie => ie.IdempleadoNavigation)
            .Include(ie => ie.IdincidenciaNavigation)
            .FirstOrDefaultAsync(ie => ie.Idincidenciaxempleado == (uint)id && ie.RegStatus == "A");

        if (incidenciaEmpleado == null) return null;

        return new IncidenciaEmpleadoViewModel
        {
            IdIncidenciaEmpleado = (int)incidenciaEmpleado.Idincidenciaxempleado,
            IdEmpleado = (int)incidenciaEmpleado.Idempleado,
            NombreEmpleado = $"{incidenciaEmpleado.IdempleadoNavigation.Primernombre} {incidenciaEmpleado.IdempleadoNavigation.Apellidopaterno} {incidenciaEmpleado.IdempleadoNavigation.Apellidomaterno}".Trim(),
            IdIncidencia = (int)incidenciaEmpleado.Idincidencia,
            DescripcionIncidencia = incidenciaEmpleado.IdincidenciaNavigation.Descripcion,
            FechaInicio = incidenciaEmpleado.Fechainicio,
            FechaFin = incidenciaEmpleado.Fechafin,
            RegStatus = incidenciaEmpleado.RegStatus
        };
    }

    public async Task<IncidenciaEmpleadoViewModel> CreateAsync(IncidenciaEmpleadoViewModel incidenciaEmpleado)
    {
        var entity = new Incidenciaxempleado
        {
            Idempleado = (uint)incidenciaEmpleado.IdEmpleado,
            Idincidencia = (uint)incidenciaEmpleado.IdIncidencia,
            Fechainicio = incidenciaEmpleado.FechaInicio,
            Fechafin = incidenciaEmpleado.FechaFin,
            RegStatus = incidenciaEmpleado.RegStatus
        };

        _ctx.Incidenciaxempleado.Add(entity);
        await _ctx.SaveChangesAsync();
        incidenciaEmpleado.IdIncidenciaEmpleado = (int)entity.Idincidenciaxempleado;
        return incidenciaEmpleado;
    }

    public async Task<IncidenciaEmpleadoViewModel?> UpdateAsync(int id, IncidenciaEmpleadoViewModel incidenciaEmpleado)
    {
        var entity = await _ctx.Incidenciaxempleado.FindAsync((uint)id);
        if (entity == null) return null;

        entity.Idempleado = (uint)incidenciaEmpleado.IdEmpleado;
        entity.Idincidencia = (uint)incidenciaEmpleado.IdIncidencia;
        entity.Fechainicio = incidenciaEmpleado.FechaInicio;
        entity.Fechafin = incidenciaEmpleado.FechaFin;
        entity.RegStatus = incidenciaEmpleado.RegStatus;

        await _ctx.SaveChangesAsync();
        return incidenciaEmpleado;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _ctx.Incidenciaxempleado.FindAsync((uint)id);
        if (entity == null) return false;

        entity.RegStatus = "B"; // Soft delete
        await _ctx.SaveChangesAsync();
        return true;
    }

    public async Task<IncidenciaEmpleadoViewModel> GetCreateViewModelAsync()
    {
        var empleados = await _empleadoServices.GetAllAsync();
        var incidencias = await _incidenciaServices.GetAllAsync();

        return new IncidenciaEmpleadoViewModel
        {
            Empleados = empleados.ToList(),
            Incidencias = incidencias.ToList()
        };
    }
} 