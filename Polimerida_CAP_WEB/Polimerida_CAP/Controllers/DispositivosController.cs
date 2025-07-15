using Microsoft.AspNetCore.Mvc;
using Polimerida_CAP.Models;
using Polimerida_CAP.Services;

namespace Polimerida_CAP.Controllers;

public class DispositivosController : Controller
{
    private readonly IDispositivoServices _svc;
    public DispositivosController(IDispositivoServices svc) => _svc = svc;

    public async Task<IActionResult> Index()
    {
        var list = await _svc.GetAllAsync();
        return View(list);
    }

    [HttpGet]
    public IActionResult Create() => View(new DispositivoViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(DispositivoViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);
        await _svc.CreateAsync(vm);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var d = await _svc.GetByIdAsync(id);
        if (d == null) return NotFound();
        return View(d);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, DispositivoViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);
        var up = await _svc.UpdateAsync(id, vm);
        if (up == null) return NotFound();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var d = await _svc.GetByIdAsync(id);
        if (d == null) return NotFound();
        return View(d);
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
        var d = await _svc.GetByIdAsync(id);
        if (d == null) return NotFound();
        return View(d);
    }
} 