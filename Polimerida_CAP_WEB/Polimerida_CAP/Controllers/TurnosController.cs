using Microsoft.AspNetCore.Mvc;
using Polimerida_CAP.Models;
using Polimerida_CAP.Services;

namespace Polimerida_CAP.Controllers
{
    public class TurnosController : Controller
    {
        private readonly IturnosServices _turnosServices;

        public TurnosController(IturnosServices turnosServices)
        {
            _turnosServices = turnosServices;
        }

        // Vista principal del catálogo de turnos
        public async Task<IActionResult> Index()
        {
            var turnos = await _turnosServices.GetAllTurnosAsync();
            return View(turnos);
        }

        // Vista para crear un nuevo turno
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Acción para crear un nuevo turno
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TurnosViewModel turno)
        {
            try
            {
                await _turnosServices.CreateTurnoAsync(turno);
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(turno);
            }
        }

        // Vista para editar un turno existente
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var turno = await _turnosServices.GetTurnoByIdAsync(id);
            if (turno == null)
                return NotFound();
            // Asegurarse de que Entradacomida y Salidacomida estén presentes en el modelo
            // (esto depende de que el servicio los asigne correctamente)
            return View(turno);
        }

        // Acción para actualizar un turno
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TurnosViewModel turno)
        {
            if (!ModelState.IsValid)
                return View(turno);

            var updated = await _turnosServices.UpdateTurnoAsync(id, turno);
            if (updated == null)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }

        // Vista para confirmar la eliminación de un turno
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var turno = await _turnosServices.GetTurnoByIdAsync(id);
            if (turno == null)
                return NotFound();
            return View(turno);
        }

        // Acción para eliminar un turno
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deleted = await _turnosServices.DeleteTurnoAsync(id);
            if (!deleted)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }

        // Acción para ver detalles de un turno
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var turno = await _turnosServices.GetTurnoByIdAsync(id);
            if (turno == null)
                return NotFound();
            return View(turno);
        }
    }
}
