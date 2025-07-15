using Microsoft.AspNetCore.Mvc;
using Polimerida_CAP.Models;
using Polimerida_CAP.Services;

namespace Polimerida_CAP.Controllers
{
    public class DepartamentosController : Controller
    {
        private readonly IDepartamentoServices _departamentoServices;

        public DepartamentosController(IDepartamentoServices departamentoServices)
        {
            _departamentoServices = departamentoServices;
        }

        // GET: /Departamentos
        public async Task<IActionResult> Index()
        {
            var departamentos = await _departamentoServices.GetAllDepartamentosAsync();
            return View(departamentos);
        }

        // GET: /Departamentos/Create
        [HttpGet]
        public IActionResult Create()
        {
            var model = new DepartamentoViewModel { RegStatus = "A" };
            return View(model);
        }

        // POST: /Departamentos/Create
        [HttpPost]
        
        public async Task<IActionResult> Create(DepartamentoViewModel departamento)
        {
            if (!ModelState.IsValid)
                return View(departamento);

            await _departamentoServices.CreateDepartamentoAsync(departamento);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Departamentos/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var departamento = await _departamentoServices.GetDepartamentoByIdAsync(id);
            if (departamento == null)
                return NotFound();
            return View(departamento);
        }

        // POST: /Departamentos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DepartamentoViewModel departamento)
        {
            if (!ModelState.IsValid)
                return View(departamento);

            var updated = await _departamentoServices.UpdateDepartamentoAsync(id, departamento);
            if (updated == null)
                return NotFound();
            return RedirectToAction(nameof(Index));
        }

        // GET: /Departamentos/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var departamento = await _departamentoServices.GetDepartamentoByIdAsync(id);
            if (departamento == null)
                return NotFound();
            return View(departamento);
        }

        // POST: /Departamentos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deleted = await _departamentoServices.DeleteDepartamentoAsync(id);
            if (!deleted)
                return NotFound();
            return RedirectToAction(nameof(Index));
        }

        // GET: /Departamentos/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var departamento = await _departamentoServices.GetDepartamentoByIdAsync(id);
            if (departamento == null)
                return NotFound();
            return View(departamento);
        }
    }
} 