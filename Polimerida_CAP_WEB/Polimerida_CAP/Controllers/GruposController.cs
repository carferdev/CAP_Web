using Microsoft.AspNetCore.Mvc;
using Polimerida_CAP.Models;
using Polimerida_CAP.Services;

namespace Polimerida_CAP.Controllers
{
    public class GruposController : Controller
    {
        private readonly IGrupoServices _grupoServices;

        public GruposController(IGrupoServices grupoServices)
        {
            _grupoServices = grupoServices;
        }

        // GET: /Grupos
        public async Task<IActionResult> Index()
        {
            var grupos = await _grupoServices.GetAllGruposAsync();
            return View(grupos);
        }

        // GET: /Grupos/Create
        [HttpGet]
        public IActionResult Create()
        {
            var model = new GrupoViewModel { RegStatus = "A" };
            return View(model);
        }

        // POST: /Grupos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GrupoViewModel grupo)
        {
            if (!ModelState.IsValid)
                return View(grupo);

            await _grupoServices.CreateGrupoAsync(grupo);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Grupos/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var grupo = await _grupoServices.GetGrupoByIdAsync(id);
            if (grupo == null) return NotFound();
            return View(grupo);
        }

        // POST: /Grupos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GrupoViewModel grupo)
        {
            if (!ModelState.IsValid)
                return View(grupo);

            var updated = await _grupoServices.UpdateGrupoAsync(id, grupo);
            if (updated == null) return NotFound();
            return RedirectToAction(nameof(Index));
        }

        // GET: /Grupos/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var grupo = await _grupoServices.GetGrupoByIdAsync(id);
            if (grupo == null) return NotFound();
            return View(grupo);
        }

        // POST: /Grupos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deleted = await _grupoServices.DeleteGrupoAsync(id);
            if (!deleted) return NotFound();
            return RedirectToAction(nameof(Index));
        }

        // GET: /Grupos/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var grupo = await _grupoServices.GetGrupoByIdAsync(id);
            if (grupo == null) return NotFound();
            return View(grupo);
        }
    }
} 