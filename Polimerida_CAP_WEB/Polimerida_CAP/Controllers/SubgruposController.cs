using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Polimerida_CAP.Models;
using Polimerida_CAP.Services;

namespace Polimerida_CAP.Controllers;

public class SubgruposController : Controller
{
    private readonly ISubgrupoServices _svc;
    private readonly IGrupoServices _grpSvc;
    private readonly IturnosServices _turnoSvc;

    public SubgruposController(ISubgrupoServices svc, IGrupoServices grp, IturnosServices turnoSvc)
    {
        _svc = svc; _grpSvc = grp; _turnoSvc = turnoSvc;
    }

    private async Task LoadSelects()
    {
        ViewBag.Grupos = (await _grpSvc.GetAllGruposAsync())
            .Select(g => new SelectListItem { Value = g.IdGrupo.ToString(), Text = g.Nombre });
        ViewBag.Turnos = (await _turnoSvc.GetAllTurnosAsync())
            .Select(t => new SelectListItem { Value = t.IdTurno.ToString(), Text = t.Nombre });
    }

    public async Task<IActionResult> Index()
    {
        var list = await _svc.GetAllAsync();
        return View(list);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await LoadSelects();
        return View(new SubgrupoViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SubgrupoViewModel vm)
    {
        if (!ModelState.IsValid) { await LoadSelects(); return View(vm); }
        await _svc.CreateAsync(vm);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var s = await _svc.GetByIdAsync(id);
        if (s == null) return NotFound();
        await LoadSelects();
        return View(s);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, SubgrupoViewModel vm)
    {
        if (!ModelState.IsValid) { await LoadSelects(); return View(vm); }
        var up = await _svc.UpdateAsync(id, vm);
        if (up == null) return NotFound();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var s = await _svc.GetByIdAsync(id);
        if (s == null) return NotFound();
        return View(s);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var ok = await _svc.DeleteAsync(id);
        if (!ok) return NotFound();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Details(int id)
    {
        var s = await _svc.GetByIdAsync(id);
        if (s == null) return NotFound();
        return View(s);
    }

    [HttpGet]
    public async Task<IActionResult> GetSubgruposByDispositivo(int idDispositivo)
    {
        // Obtener el dispositivo usando el servicio
        var dispositivo = await _grpSvc.GetDispositivoByIdAsync(idDispositivo);
        if (dispositivo == null)
            return Json(new List<object>());

        // Determinar grupo
        string grupoNombre = dispositivo.Descripcion.Trim().ToLower() == "administracion" ? "B" : "A";

        // Obtener el grupo correspondiente usando el servicio
        var grupo = await _grpSvc.GetGrupoByNombreAsync(grupoNombre);
        if (grupo == null)
            return Json(new List<object>());

        // Obtener subgrupos de ese grupo usando el servicio
        var subgrupos = await _svc.GetByGrupoAsync(grupo.IdGrupo);

        // Obtener turnos usando el servicio
        var turnos = await _turnoSvc.GetAllTurnosAsync();

        // Construir resultado con nombre + horario
        var result = subgrupos.Select(sg => {
            var turno = turnos.FirstOrDefault(t => t.IdTurno == sg.IdTurno);
            string horario = turno != null ? $" ({turno.HoraEntrada:HH:mm} - {turno.HoraSalida:HH:mm})" : "";
            return new {
                IdSubgrupo = sg.IdSubgrupo,
                Nombre = (sg.Nombre ?? "") + horario
            };
        });

        return Json(result);
    }
} 