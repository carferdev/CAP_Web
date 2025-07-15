using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Polimerida_CAP.Services.Data;
using Polimerida_CAP.Models;

namespace Polimerida_CAP.Controllers
{
    public class AsignarTurnosController : Controller
    {
        private readonly AppDbContext _ctx;
        public AsignarTurnosController(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet]
        public IActionResult Index(int? grupo)
        {
            var subgrupos = _ctx.Subgrupo
                .Where(s => s.RegStatus == "A" && (grupo == null || s.Idgrupo == grupo))
                .ToList();
            var turnos = _ctx.Turno.Where(t => t.RegStatus == "A").ToList();
            var grupos = _ctx.Grupo.Where(g => g.RegStatus == "A").ToList();
            ViewBag.Turnos = turnos;
            ViewBag.Grupos = grupos;
            ViewBag.GrupoSeleccionado = grupo;
            return View(subgrupos);
        }

        [HttpPost]
        public IActionResult Guardar(List<int> subgrupoIds, List<int> turnoIds)
        {
            for (int i = 0; i < subgrupoIds.Count; i++)
            {
                var subgrupo = _ctx.Subgrupo.Find(subgrupoIds[i]);
                if (subgrupo != null && subgrupo.Idturno != turnoIds[i])
                {
                    subgrupo.Idturno = turnoIds[i];
                }
            }
            _ctx.SaveChanges();
            TempData["Mensaje"] = "Turnos actualizados correctamente.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Empleado(int? empleadoId = null)
        {
            var empleados = _ctx.Empleado.Where(e => e.RegStatus == "A").ToList();
            var turnos = _ctx.Turno.Where(t => t.RegStatus == "A").ToList();
            ViewBag.Empleados = empleados;
            ViewBag.Turnos = turnos;
            if (empleadoId.HasValue)
            {
                var empleado = empleados.FirstOrDefault(e => e.Idempleado == empleadoId.Value);
                if (empleado != null && empleado.Idsubgrupo.HasValue)
                {
                    var subgrupo = _ctx.Subgrupo.FirstOrDefault(sg => sg.Idsubgrupo == empleado.Idsubgrupo);
                    ViewBag.SubgrupoActual = subgrupo?.Nombre;
                    if (subgrupo != null)
                    {
                        var turnoActual = _ctx.Turno.FirstOrDefault(t => t.Idturno == subgrupo.Idturno);
                        ViewBag.TurnoActual = turnoActual?.Nombre;
                    }
                }
            }
            return View();
        }

        [HttpPost]
        public IActionResult Empleado(int empleadoId, int turnoId)
        {
            var empleado = _ctx.Empleado.Find((uint)empleadoId);
            if (empleado != null)
            {
                // Eliminar uso de IdsubgrupoNavigation.Idturno, usar solo Idsubgrupo y buscar el subgrupo
                if (empleado.Idsubgrupo.HasValue)
                {
                    var subgrupo = _ctx.Subgrupo.Find(empleado.Idsubgrupo.Value);
                    if (subgrupo != null)
                    {
                        subgrupo.Idturno = turnoId;
                    }
                }
                _ctx.SaveChanges();
                TempData["Mensaje"] = "Turno actualizado correctamente para el empleado.";
            }
            else
            {
                TempData["Mensaje"] = "Empleado no encontrado.";
            }
            return RedirectToAction("Empleado");
        }

        [HttpGet]
        public IActionResult EmpleadoMultiple(int? grupoId = null)
        {
            var grupos = _ctx.Grupo.Where(g => g.RegStatus == "A").ToList();
            var turnos = _ctx.Turno.Where(t => t.RegStatus == "A").ToList();
            var empleados = new List<EmpleadoTurnoSeleccionViewModel>();
            if (grupoId.HasValue)
            {
                empleados = _ctx.Empleado
                    .Where(e => e.RegStatus == "A" && e.Idsubgrupo.HasValue && _ctx.Subgrupo.Any(sg => sg.Idsubgrupo == e.Idsubgrupo && sg.Idgrupo == grupoId))
                    .Select(e => new EmpleadoTurnoSeleccionViewModel
                    {
                        IdEmpleado = (int)e.Idempleado,
                        NombreCompleto = e.Primernombre + " " + e.Apellidopaterno + " " + e.Apellidomaterno,
                        Seleccionado = false,
                        IdTurnoSeleccionado = null,
                        TurnoActual = _ctx.Subgrupo.Where(sg => sg.Idsubgrupo == e.Idsubgrupo).Select(sg => _ctx.Turno.FirstOrDefault(t => t.Idturno == sg.Idturno).Nombre).FirstOrDefault()
                    }).ToList();
            }
            var vm = new AsignarTurnoMultipleViewModel
            {
                Empleados = empleados,
                TurnosDisponibles = turnos.Select(t => new TurnosViewModel
                {
                    IdTurno = (int)t.Idturno,
                    Nombre = t.Nombre,
                    HoraEntrada = t.Horaentrada,
                    HoraSalida = t.Horasalida,
                    RegStatus = t.RegStatus
                }).ToList()
            };
            ViewBag.Grupos = grupos;
            ViewBag.GrupoSeleccionado = grupoId;
            return View(vm);
        }

        [HttpPost]
        public IActionResult EmpleadoMultiple(AsignarTurnoMultipleViewModel model, int? grupoId)
        {
            if (model.Empleados != null)
            {
                foreach (var emp in model.Empleados)
                {
                    if (emp.Seleccionado && emp.IdTurnoSeleccionado.HasValue)
                    {
                        var empleado = _ctx.Empleado.Find((uint)emp.IdEmpleado);
                        if (empleado != null && empleado.Idsubgrupo.HasValue)
                        {
                            var subgrupo = _ctx.Subgrupo.Find(empleado.Idsubgrupo.Value);
                            if (subgrupo != null)
                            {
                                subgrupo.Idturno = emp.IdTurnoSeleccionado.Value;
                            }
                        }
                    }
                }
                _ctx.SaveChanges();
                TempData["Mensaje"] = "Turnos asignados correctamente.";
            }
            return RedirectToAction("EmpleadoMultiple", new { grupoId });
        }
    }
} 