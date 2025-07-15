using Microsoft.EntityFrameworkCore;
using Polimerida_CAP.Models;
using Polimerida_CAP.Services.Data;

namespace Polimerida_CAP.Services;

public class FestivoServices : IFestivoServices
{
    private readonly AppDbContext _ctx;

    public FestivoServices(AppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<IEnumerable<FestivoViewModel>> GetAllAsync()
    {
        return await _ctx.Festivo
            .Where(f => f.RegStatus == "A")
            .OrderBy(f => f.Fecha)
            .Select(f => new FestivoViewModel
            {
                IdFestivo = (int)f.Idfestivo,
                Fecha = f.Fecha,
                Descripcion = f.Descripcion,
                RegStatus = f.RegStatus
            })
            .ToListAsync();
    }

    public async Task<FestivoViewModel?> GetByIdAsync(int id)
    {
        var festivo = await _ctx.Festivo.FindAsync((uint)id);
        if (festivo == null) return null;

        return new FestivoViewModel
        {
            IdFestivo = (int)festivo.Idfestivo,
            Fecha = festivo.Fecha,
            Descripcion = festivo.Descripcion,
            RegStatus = festivo.RegStatus
        };
    }

    public async Task<FestivoViewModel> CreateAsync(FestivoViewModel festivo)
    {
        var entity = new Festivo
        {
            Fecha = festivo.Fecha,
            Descripcion = festivo.Descripcion,
            RegStatus = festivo.RegStatus
        };

        _ctx.Festivo.Add(entity);
        await _ctx.SaveChangesAsync();
        festivo.IdFestivo = (int)entity.Idfestivo;
        return festivo;
    }

    public async Task<FestivoViewModel?> UpdateAsync(int id, FestivoViewModel festivo)
    {
        var entity = await _ctx.Festivo.FindAsync((uint)id);
        if (entity == null) return null;

        entity.Fecha = festivo.Fecha;
        entity.Descripcion = festivo.Descripcion;
        entity.RegStatus = festivo.RegStatus;

        await _ctx.SaveChangesAsync();
        return festivo;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _ctx.Festivo.FindAsync((uint)id);
        if (entity == null) return false;

        entity.RegStatus = "B"; // Soft delete
        await _ctx.SaveChangesAsync();
        return true;
    }
} 