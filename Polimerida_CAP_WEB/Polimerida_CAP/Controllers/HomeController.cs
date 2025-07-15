using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Polimerida_CAP.Models;
using Polimerida_CAP.Services;

namespace Polimerida_CAP.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEmpleadoServices _empleadoServices;
        private readonly IDepartamentoServices _departamentoServices;
        private readonly IGrupoServices _grupoServices;
        private readonly IDispositivoServices _dispositivoServices;
        private readonly IIncidenciaEmpleadoServices _incidenciaEmpleadoServices;

        public HomeController(
            ILogger<HomeController> logger,
            IEmpleadoServices empleadoServices,
            IDepartamentoServices departamentoServices,
            IGrupoServices grupoServices,
            IDispositivoServices dispositivoServices,
            IIncidenciaEmpleadoServices incidenciaEmpleadoServices)
        {
            _logger = logger;
            _empleadoServices = empleadoServices;
            _departamentoServices = departamentoServices;
            _grupoServices = grupoServices;
            _dispositivoServices = dispositivoServices;
            _incidenciaEmpleadoServices = incidenciaEmpleadoServices;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Panel()
        {
            // Verifica si el usuario está autenticado
            if (HttpContext.Session.GetString("Token") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            try
            {
                // Obtener datos reales de los servicios
                var empleados = await _empleadoServices.GetAllAsync();
                var departamentos = await _departamentoServices.GetAllDepartamentosAsync();
                var grupos = await _grupoServices.GetAllGruposAsync();
                var dispositivos = await _dispositivoServices.GetAllAsync();
                var incidenciasEmpleados = await _incidenciaEmpleadoServices.GetAllAsync();

                // Calcular estadísticas generales
                var totalEmpleados = empleados.Count();
                var empleadosActivos = empleados.Where(e => e.RegStatus == "A").Count();
                
                // Simular datos de asistencia (en un sistema real, esto vendría de registros de asistencia)
                var empleadosPresentes = empleadosActivos * 85 / 100; // 85% de asistencia simulada
                var empleadosAusentes = empleadosActivos - empleadosPresentes;
                var empleadosJustificados = empleadosAusentes / 3; // 1/3 de ausencias justificadas

                var datosDelPanel = new PanelViewModel
                {
                    EmpleadosPresentes = empleadosPresentes,
                    EmpleadosAusentes = empleadosAusentes - empleadosJustificados,
                    EmpleadosJustificados = empleadosJustificados,
                    TotalEmpleados = totalEmpleados,
                    PorcentajeAsistencia = totalEmpleados > 0 ? (double)empleadosPresentes / totalEmpleados * 100 : 0
                };

                // Estadísticas por departamento
                foreach (var dept in departamentos)
                {
                    var empleadosDept = empleados.Where(e => e.IdDepartamento == dept.IdDepartamento).ToList();
                    var totalDept = empleadosDept.Count;
                    var presentesDept = totalDept * 85 / 100; // Simulación
                    var ausentesDept = totalDept - presentesDept;
                    var justificadosDept = ausentesDept / 3;

                    datosDelPanel.EstadisticasPorDepartamento.Add(new DepartamentoEstadistica
                    {
                        IdDepartamento = dept.IdDepartamento,
                        NombreDepartamento = dept.Descripcion,
                        TotalEmpleados = totalDept,
                        Presentes = presentesDept,
                        Ausentes = ausentesDept - justificadosDept,
                        Justificados = justificadosDept,
                        PorcentajeAsistencia = totalDept > 0 ? (double)presentesDept / totalDept * 100 : 0
                    });
                }

                // Estadísticas por grupo
                foreach (var grupo in grupos)
                {
                    var empleadosGrupo = empleados.Where(e => e.IdSubgrupo.HasValue).ToList(); // Simplificado
                    var totalGrupo = empleadosGrupo.Count;
                    var presentesGrupo = totalGrupo * 88 / 100; // Simulación
                    var ausentesGrupo = totalGrupo - presentesGrupo;
                    var justificadosGrupo = ausentesGrupo / 4;

                    datosDelPanel.EstadisticasPorGrupo.Add(new GrupoEstadistica
                    {
                        IdGrupo = grupo.IdGrupo,
                        NombreGrupo = grupo.Descripcion,
                        TotalEmpleados = totalGrupo,
                        Presentes = presentesGrupo,
                        Ausentes = ausentesGrupo - justificadosGrupo,
                        Justificados = justificadosGrupo,
                        PorcentajeAsistencia = totalGrupo > 0 ? (double)presentesGrupo / totalGrupo * 100 : 0
                    });
                }

                // Dispositivos activos
                foreach (var dispositivo in dispositivos)
                {
                    var empleadosDispositivo = empleados.Where(e => e.IdDispositivo == dispositivo.IdDispositivo).Count();
                    
                    datosDelPanel.DispositivosActivos.Add(new DispositivoEstadistica
                    {
                        IdDispositivo = dispositivo.IdDispositivo,
                        NombreDispositivo = dispositivo.Descripcion,
                        Ip = dispositivo.Ip,
                        Estado = true, // Simulado como activo
                        EmpleadosRegistrados = empleadosDispositivo
                    });
                }

                // Últimas incidencias de empleados (limitado a 5)
                datosDelPanel.UltimasIncidenciasEmpleados = incidenciasEmpleados
                    .OrderByDescending(ie => ie.FechaInicio)
                    .Take(5)
                    .ToList();

                return View(datosDelPanel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar datos del panel");
                
                // En caso de error, mostrar datos básicos
                var datosDelPanel = new PanelViewModel
                {
                    EmpleadosPresentes = 0,
                    EmpleadosAusentes = 0,
                    EmpleadosJustificados = 0,
                    TotalEmpleados = 0,
                    PorcentajeAsistencia = 0
                };

                return View(datosDelPanel);
            }
        }
    }
}
