using Microsoft.AspNetCore.Mvc;
using Polimerida_CAP.Models;
using Polimerida_CAP.Services;

namespace Polimerida_CAP.Controllers;

public class FestivosController : Controller
{
    private readonly IFestivoServices _festivoServices;

    public FestivosController(IFestivoServices festivoServices)
    {
        _festivoServices = festivoServices;
    }

    // GET: Festivos
    public async Task<IActionResult> Index()
    {
        var festivos = await _festivoServices.GetAllAsync();
        return View(festivos);
    }

    // GET: Festivos/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Festivos/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Fecha,Descripcion")] FestivoViewModel festivo)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _festivoServices.CreateAsync(festivo);
                TempData["SuccessMessage"] = "Día festivo creado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al crear el día festivo: " + ex.Message);
            }
        }
        return View(festivo);
    }

    // GET: Festivos/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var festivo = await _festivoServices.GetByIdAsync(id);
        if (festivo == null)
        {
            return NotFound();
        }
        return View(festivo);
    }

    // POST: Festivos/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("IdFestivo,Fecha,Descripcion,RegStatus")] FestivoViewModel festivo)
    {
        if (id != festivo.IdFestivo)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var result = await _festivoServices.UpdateAsync(id, festivo);
                if (result == null)
                {
                    return NotFound();
                }
                TempData["SuccessMessage"] = "Día festivo actualizado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al actualizar el día festivo: " + ex.Message);
            }
        }
        return View(festivo);
    }

    // GET: Festivos/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var festivo = await _festivoServices.GetByIdAsync(id);
        if (festivo == null)
        {
            return NotFound();
        }

        return View(festivo);
    }

    // POST: Festivos/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var result = await _festivoServices.DeleteAsync(id);
        if (!result)
        {
            return NotFound();
        }

        TempData["SuccessMessage"] = "Día festivo eliminado exitosamente.";
        return RedirectToAction(nameof(Index));
    }
} 