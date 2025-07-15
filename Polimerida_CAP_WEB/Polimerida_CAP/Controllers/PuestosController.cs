using Microsoft.AspNetCore.Mvc;
using Polimerida_CAP.Models;
using Polimerida_CAP.Services;

namespace Polimerida_CAP.Controllers
{
    public class PuestosController : Controller
    {
        private readonly IPuestoServices _puestoServices;

        public PuestosController(IPuestoServices puestoServices)
        {
            _puestoServices = puestoServices;
        }

        public async Task<IActionResult> Index()
        {
            var puestos = await _puestoServices.GetAllPuestosAsync();
            return View(puestos);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new PuestoViewModel { RegStatus = "A" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PuestoViewModel puesto)
        {
            if (!ModelState.IsValid)
                return View(puesto);
            await _puestoServices.CreatePuestoAsync(puesto);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var puesto = await _puestoServices.GetPuestoByIdAsync(id);
            if (puesto == null) return NotFound();
            return View(puesto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PuestoViewModel puesto)
        {
            if (!ModelState.IsValid)
                return View(puesto);
            var updated = await _puestoServices.UpdatePuestoAsync(id, puesto);
            if (updated == null) return NotFound();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var puesto = await _puestoServices.GetPuestoByIdAsync(id);
            if (puesto == null) return NotFound();
            return View(puesto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deleted = await _puestoServices.DeletePuestoAsync(id);
            if (!deleted) return NotFound();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var puesto = await _puestoServices.GetPuestoByIdAsync(id);
            if (puesto == null) return NotFound();
            return View(puesto);
        }        
    }
} 