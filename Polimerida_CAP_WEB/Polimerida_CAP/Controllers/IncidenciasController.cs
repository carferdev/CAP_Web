using Microsoft.AspNetCore.Mvc;
using Polimerida_CAP.Models;
using Polimerida_CAP.Services;

namespace Polimerida_CAP.Controllers
{
    public class IncidenciasController : Controller
    {
        private readonly IIncidenciaServices _svc;

        public IncidenciasController(IIncidenciaServices svc)
        {
            _svc = svc;
        }

        public async Task<IActionResult> Index()
        {
            var incidencias = await _svc.GetAllAsync();
            return View(incidencias);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new IncidenciaViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IncidenciaViewModel incidencia)
        {
            if (ModelState.IsValid)
            {
                await _svc.CreateAsync(incidencia);
                TempData["Mensaje"] = "Incidencia creada correctamente.";
                return RedirectToAction("Index");
            }
            return View(incidencia);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var incidencia = await _svc.GetByIdAsync(id);
            if (incidencia == null) return NotFound();
            return View(incidencia);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IncidenciaViewModel incidencia)
        {
            if (ModelState.IsValid)
            {
                var result = await _svc.UpdateAsync(id, incidencia);
                if (result == null) return NotFound();
                TempData["Mensaje"] = "Incidencia actualizada correctamente.";
                return RedirectToAction("Index");
            }
            return View(incidencia);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var incidencia = await _svc.GetByIdAsync(id);
            if (incidencia == null) return NotFound();
            return View(incidencia);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, DateTime? FechaBaja, string? RegStatus)
        {
            var result = await _svc.DeleteAsync(id, FechaBaja, RegStatus);
            if (result)
            {
                TempData["Mensaje"] = "Incidencia eliminada correctamente.";
            }
            return RedirectToAction("Index");
        }
    }
} 