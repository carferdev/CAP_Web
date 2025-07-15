using Microsoft.AspNetCore.Mvc;
using Polimerida_CAP.Models;
using Polimerida_CAP.Services;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Polimerida_CAP.Controllers;

public class EmpleadosController : Controller
{
    private readonly IEmpleadoServices _svc;
    private readonly IDepartamentoServices _depSvc;
    private readonly IDispositivoServices _dispSvc;
    private readonly ISubgrupoServices _subgSvc;
    private readonly IPuestoServices _ptoSvc;

    public EmpleadosController(IEmpleadoServices svc, IDepartamentoServices dep, IDispositivoServices disp, ISubgrupoServices subg, IPuestoServices pto)
    {
        _svc = svc; _depSvc = dep; _dispSvc = disp; _subgSvc = subg; _ptoSvc = pto;
    }

    private async Task LoadSelectLists()
    {
        ViewBag.Departamentos = (await _depSvc.GetAllDepartamentosAsync())
            .Select(d => new SelectListItem { Value = d.IdDepartamento.ToString(), Text = d.Descripcion });
        ViewBag.Puestos = (await _ptoSvc.GetAllPuestosAsync())
            .Select(p => new SelectListItem { Value = p.IdPuesto.ToString(), Text = p.Descripcion });
        ViewBag.Dispositivos = (await _dispSvc.GetAllAsync())
            .Select(d => new SelectListItem { Value = d.IdDispositivo.ToString(), Text = d.Descripcion });
        ViewBag.Subgrupos = (await _subgSvc.GetAllAsync())
            .Select(s => new SelectListItem { Value = s.IdSubgrupo.ToString(), Text = s.Nombre });
    }

    public async Task<IActionResult> Index()
    {
        var list = await _svc.GetAllAsync();
        return View(list);
    }

    public async Task<IActionResult> Create()
    {
        await LoadSelectLists();
        return View(new EmpleadoViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(EmpleadoViewModel vm)
    {
        // Validar tamaño del archivo
        if (Request.Form.Files.Count > 0)
        {
            var file = Request.Form.Files[0];
            const int maxSize = 200 * 1024; // 200KB en bytes
            
            if (file.Length > maxSize)
            {
                ModelState.AddModelError("Foto", "El archivo es demasiado grande. El tamaño máximo es 200KB.");
                await LoadSelectLists();
                return View(vm);
            }
            
            // Validar tipo de archivo
            if (!file.ContentType.StartsWith("image/"))
            {
                ModelState.AddModelError("Foto", "Por favor selecciona un archivo de imagen válido.");
                await LoadSelectLists();
                return View(vm);
            }
        }
        
        if (!ModelState.IsValid) { await LoadSelectLists(); return View(vm); }
        
        var (empleado, deviceResponse) = await _svc.CreateAsync(vm);
        
        if (deviceResponse != null)
        {
            // Almacenar las respuestas en TempData para mostrarlas en la vista
            TempData["UserResponse"] = deviceResponse.UserResponse;
            TempData["FaceResponse"] = deviceResponse.FaceResponse;
            TempData["UserSuccess"] = deviceResponse.UserSuccess;
            TempData["FaceSuccess"] = deviceResponse.FaceSuccess;
            TempData["ErrorMessage"] = deviceResponse.ErrorMessage;
        }
        
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var emp = await _svc.GetByIdAsync(id);
        if (emp == null) return NotFound();
        await LoadSelectLists();
        // Obtener nombre del dispositivo
        string fotoUrl = null;
        if (emp.IdDispositivo.HasValue && !string.IsNullOrEmpty(emp.UrlFoto))
        {
            var dispositivo = await _dispSvc.GetByIdAsync(emp.IdDispositivo.Value);
            if (dispositivo != null)
            {
                var config = HttpContext.RequestServices.GetService(typeof(IConfiguration)) as IConfiguration;
                string baseUrl = config?["FileConsultaSettings:BaseUrl"] ?? "";
                fotoUrl = baseUrl.TrimEnd('/') + "/" + dispositivo.Descripcion + "/" + emp.UrlFoto;
            }
        }
        ViewBag.FotoUrl = fotoUrl;
        return View(emp);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EmpleadoViewModel vm)
    {
        // Validar tamaño del archivo
        if (Request.Form.Files.Count > 0)
        {
            var file = Request.Form.Files[0];
            const int maxSize = 200 * 1024; // 200KB en bytes
            
            if (file.Length > maxSize)
            {
                ModelState.AddModelError("Foto", "El archivo es demasiado grande. El tamaño máximo es 200KB.");
                await LoadSelectLists();
                return View(vm);
            }
            
            // Validar tipo de archivo
            if (!file.ContentType.StartsWith("image/"))
            {
                ModelState.AddModelError("Foto", "Por favor selecciona un archivo de imagen válido.");
                await LoadSelectLists();
                return View(vm);
            }
        }
        
        if (!ModelState.IsValid) { await LoadSelectLists(); return View(vm); }
        
        var (updated, deviceResponse) = await _svc.UpdateAsync(id, vm);
        if (updated == null) return NotFound();

        if (deviceResponse != null)
        {
            // Almacenar las respuestas en TempData para mostrarlas en la vista
            TempData["UserResponse"] = deviceResponse.UserResponse;
            TempData["FaceResponse"] = deviceResponse.FaceResponse;
            TempData["UserSuccess"] = deviceResponse.UserSuccess;
            TempData["FaceSuccess"] = deviceResponse.FaceSuccess;
            TempData["ErrorMessage"] = deviceResponse.ErrorMessage;
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var emp = await _svc.GetByIdAsync(id);
        if (emp == null) return NotFound();
        await LoadSelectLists();
        return View(emp);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, DateTime? FechaBaja, string? RegStatus)
    {
        var ok = await _svc.DeleteAsync(id, FechaBaja);
        if (!ok) return NotFound();
        TempData["SuccessMessage"] = "Empleado eliminado correctamente, también se eliminó del dispositivo si correspondía.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Details(int id)
    {
        var emp = await _svc.GetByIdAsync(id);
        if (emp == null) return NotFound();
        await LoadSelectLists();
        return View(emp);
    }

    [HttpGet]
    public async Task<IActionResult> EmpleadosTabla(bool mostrarInactivos = false)
    {
        var empleados = await _svc.GetAllAsync();
        if (mostrarInactivos)
            empleados = empleados.Where(e => e.RegStatus != "A").ToList();
        else
            empleados = empleados.Where(e => e.RegStatus == "A").ToList();
        return PartialView("_EmpleadosTabla", empleados);
    }
} 