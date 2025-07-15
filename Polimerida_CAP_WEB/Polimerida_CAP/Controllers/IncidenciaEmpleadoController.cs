using Microsoft.AspNetCore.Mvc;
using Polimerida_CAP.Models;
using Polimerida_CAP.Services;

namespace Polimerida_CAP.Controllers;

public class IncidenciaEmpleadoController : Controller
{
    private readonly IIncidenciaEmpleadoServices _incidenciaEmpleadoServices;

    public IncidenciaEmpleadoController(IIncidenciaEmpleadoServices incidenciaEmpleadoServices)
    {
        _incidenciaEmpleadoServices = incidenciaEmpleadoServices;
    }

    // GET: IncidenciaEmpleado
    public async Task<IActionResult> Index()
    {
        var incidenciasEmpleados = await _incidenciaEmpleadoServices.GetAllAsync();
        return View(incidenciasEmpleados);
    }

    // GET: IncidenciaEmpleado/Create
    public async Task<IActionResult> Create()
    {
        var viewModel = await _incidenciaEmpleadoServices.GetCreateViewModelAsync();
        return View(viewModel);
    }

    // POST: IncidenciaEmpleado/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("IdEmpleado,IdIncidencia,FechaInicio,FechaFin")] IncidenciaEmpleadoViewModel incidenciaEmpleado)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _incidenciaEmpleadoServices.CreateAsync(incidenciaEmpleado);
                TempData["SuccessMessage"] = "Incidencia asignada al empleado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al crear la incidencia: " + ex.Message);
            }
        }

        // Si hay errores, recargar los dropdowns
        var viewModel = await _incidenciaEmpleadoServices.GetCreateViewModelAsync();
        viewModel.IdEmpleado = incidenciaEmpleado.IdEmpleado;
        viewModel.IdIncidencia = incidenciaEmpleado.IdIncidencia;
        viewModel.FechaInicio = incidenciaEmpleado.FechaInicio;
        viewModel.FechaFin = incidenciaEmpleado.FechaFin;
        return View(viewModel);
    }

    // GET: IncidenciaEmpleado/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var incidenciaEmpleado = await _incidenciaEmpleadoServices.GetByIdAsync(id);
        if (incidenciaEmpleado == null)
        {
            return NotFound();
        }

        var viewModel = await _incidenciaEmpleadoServices.GetCreateViewModelAsync();
        viewModel.IdIncidenciaEmpleado = incidenciaEmpleado.IdIncidenciaEmpleado;
        viewModel.IdEmpleado = incidenciaEmpleado.IdEmpleado;
        viewModel.IdIncidencia = incidenciaEmpleado.IdIncidencia;
        viewModel.FechaInicio = incidenciaEmpleado.FechaInicio;
        viewModel.FechaFin = incidenciaEmpleado.FechaFin;
        viewModel.RegStatus = incidenciaEmpleado.RegStatus;

        return View(viewModel);
    }

    // POST: IncidenciaEmpleado/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("IdIncidenciaEmpleado,IdEmpleado,IdIncidencia,FechaInicio,FechaFin,RegStatus")] IncidenciaEmpleadoViewModel incidenciaEmpleado)
    {
        if (id != incidenciaEmpleado.IdIncidenciaEmpleado)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var result = await _incidenciaEmpleadoServices.UpdateAsync(id, incidenciaEmpleado);
                if (result == null)
                {
                    return NotFound();
                }
                TempData["SuccessMessage"] = "Incidencia del empleado actualizada exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al actualizar la incidencia: " + ex.Message);
            }
        }

        // Si hay errores, recargar los dropdowns
        var viewModel = await _incidenciaEmpleadoServices.GetCreateViewModelAsync();
        viewModel.IdIncidenciaEmpleado = incidenciaEmpleado.IdIncidenciaEmpleado;
        viewModel.IdEmpleado = incidenciaEmpleado.IdEmpleado;
        viewModel.IdIncidencia = incidenciaEmpleado.IdIncidencia;
        viewModel.FechaInicio = incidenciaEmpleado.FechaInicio;
        viewModel.FechaFin = incidenciaEmpleado.FechaFin;
        viewModel.RegStatus = incidenciaEmpleado.RegStatus;

        return View(viewModel);
    }

    // GET: IncidenciaEmpleado/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var incidenciaEmpleado = await _incidenciaEmpleadoServices.GetByIdAsync(id);
        if (incidenciaEmpleado == null)
        {
            return NotFound();
        }

        return View(incidenciaEmpleado);
    }

    // POST: IncidenciaEmpleado/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var result = await _incidenciaEmpleadoServices.DeleteAsync(id);
        if (!result)
        {
            return NotFound();
        }

        TempData["SuccessMessage"] = "Incidencia del empleado eliminada exitosamente.";
        return RedirectToAction(nameof(Index));
    }
} 