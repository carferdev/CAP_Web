using Microsoft.EntityFrameworkCore;
using Polimerida_CAP.Models;
using Polimerida_CAP.Services.Data;
using System.Runtime.Intrinsics.X86;

namespace Polimerida_CAP.Services;

public class SubgrupoServices : ISubgrupoServices
{
    private readonly AppDbContext _ctx;
    public SubgrupoServices(AppDbContext ctx) => _ctx = ctx;

    public async Task<IEnumerable<SubgrupoViewModel>> GetAllAsync()
    {
        return await _ctx.Subgrupo
            .Where(s => s.RegStatus == "A")
            .Select(s => new SubgrupoViewModel
            {
                IdSubgrupo = s.Idsubgrupo,
                Nombre = s.Nombre  ?? "",
                Descripcion = s.Descripcion ?? "",
                IdGrupo = s.Idgrupo,
                IdTurno = s.Idturno,
                RegStatus = s.RegStatus ?? ""
            }).ToListAsync();
    }

    public async Task<SubgrupoViewModel?> GetByIdAsync(int id)
    {
        var s = await _ctx.Subgrupo.FindAsync(id);
        if (s == null) return null;
        return new SubgrupoViewModel
        {
            IdSubgrupo = s.Idsubgrupo,
            Nombre = s.Nombre  ?? "",
            Descripcion = s.Descripcion ?? "",
            IdGrupo = s.Idgrupo,
            IdTurno = s.Idturno,
            RegStatus = s.RegStatus ?? ""
        };
    }

    public async Task<SubgrupoViewModel> CreateAsync(SubgrupoViewModel vm)
    {
        var ent = new Subgrupo
        {
            Descripcion = vm.Descripcion ,
            Nombre = vm .Nombre ,
            Idgrupo = vm.IdGrupo ?? 0,
            Idturno = vm.IdTurno ?? 0,
            RegStatus = vm.RegStatus
        };
        _ctx.Subgrupo.Add(ent);
        await _ctx.SaveChangesAsync();
        vm.IdSubgrupo = ent.Idsubgrupo;
        return vm;
    }

    public async Task<SubgrupoViewModel?> UpdateAsync(int id, SubgrupoViewModel vm)
    {
        var ent = await _ctx.Subgrupo.FindAsync(id);
        if (ent == null) return null;
        ent.Descripcion = vm.Descripcion ;
        ent.Nombre = vm.Nombre;
        ent.Idgrupo = vm.IdGrupo ?? 0;
        ent.Idturno = vm.IdTurno ?? 0;
        ent.RegStatus = vm.RegStatus;
        await _ctx.SaveChangesAsync();
        return vm;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var ent = await _ctx.Subgrupo.FindAsync(id);
        if (ent == null) return false;
        ent.RegStatus = "B";
        await _ctx.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<SubgrupoViewModel>> GetByGrupoAsync(int idGrupo)
    {
        return await _ctx.Subgrupo
            .Where(s => s.Idgrupo == idGrupo && s.RegStatus == "A")
            .Select(s => new SubgrupoViewModel
            {
                IdSubgrupo = s.Idsubgrupo,
                Nombre = s.Nombre ?? "",
                Descripcion = s.Descripcion ?? "",
                IdGrupo = s.Idgrupo,
                IdTurno = s.Idturno,
                RegStatus = s.RegStatus ?? ""
            }).ToListAsync();
    }
} 