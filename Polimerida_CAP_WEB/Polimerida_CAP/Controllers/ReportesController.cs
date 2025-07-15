using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Polimerida_CAP.Models;
using Polimerida_CAP.Services.Data;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Rendering;
using ClosedXML.Excel;

namespace Polimerida_CAP.Controllers
{
    public class ReportesController : Controller
    {
        private readonly AppDbContext _ctx;
        public ReportesController(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        private void CargarFiltros(int? empleadoSel = null, int? grupoSel = null, int? turnoSel = null)
        {
            var empleados = _ctx.Empleado.Where(e => e.RegStatus == "A")
                .Select(e => new SelectListItem {
                    Value = e.Idempleado.ToString(),
                    Text = e.Primernombre + " " + e.Apellidopaterno + " " + e.Apellidomaterno,
                    Selected = empleadoSel.HasValue && e.Idempleado == empleadoSel.Value
                }).ToList();
            empleados.Insert(0, new SelectListItem { Value = "", Text = "Todos" });
            ViewBag.Empleados = empleados;

            var grupos = _ctx.Grupo.Where(g => g.RegStatus == "A")
                .Select(g => new SelectListItem {
                    Value = g.Idgrupo.ToString(),
                    Text = g.Descripcion, // Mostrar la descripción en vez del nombre
                    Selected = grupoSel.HasValue && g.Idgrupo == grupoSel.Value
                }).ToList();
            grupos.Insert(0, new SelectListItem { Value = "", Text = "Todos" });
            ViewBag.Grupos = grupos;

            var turnos = _ctx.Turno.Where(t => t.RegStatus == "A")
                .Select(t => new SelectListItem {
                    Value = t.Idturno.ToString(),
                    Text = t.Nombre,
                    Selected = turnoSel.HasValue && t.Idturno == turnoSel.Value
                }).ToList();
            turnos.Insert(0, new SelectListItem { Value = "", Text = "Todos" });
            ViewBag.Turnos = turnos;
        }

        [HttpGet]
        public IActionResult Semanal()
        {
            CargarFiltros();
            return View(new List<ReporteSemanalViewModel>());
        }

        [HttpPost]
        public IActionResult Semanal(int semana, int? empleado, int? grupo, int? turno)
        {
            CargarFiltros(empleado, grupo, turno);
            // Calcular el rango de fechas de la semana
            var year = DateTime.Now.Year;
            var firstDayOfYear = new DateTime(year, 1, 1);
            var daysOffset = DayOfWeek.Monday - firstDayOfYear.DayOfWeek;
            var firstMonday = firstDayOfYear.AddDays(daysOffset);
            var startDate = firstMonday.AddDays((semana - 1) * 7);
            var endDate = startDate.AddDays(6);

            // Agregar la fecha de inicio al ViewBag para que la vista pueda mostrar las fechas
            ViewBag.StartDate = startDate;
            ViewBag.Semana = semana;

            // Obtener empleados activos y filtrar
            var empleadosQuery = _ctx.Empleado.Where(e => e.RegStatus == "A");
            if (empleado.HasValue)
                empleadosQuery = empleadosQuery.Where(e => e.Idempleado == empleado.Value);
            if (grupo.HasValue)
                empleadosQuery = empleadosQuery.Where(e => e.IdsubgrupoNavigation != null && e.IdsubgrupoNavigation.Idgrupo == grupo.Value);
            if (turno.HasValue)
                empleadosQuery = empleadosQuery.Where(e => e.IdsubgrupoNavigation != null && e.IdsubgrupoNavigation.Idturno == turno.Value);
            var empleados = empleadosQuery.Include(e => e.IdsubgrupoNavigation).AsNoTracking().ToList();

            // Obtener grupos y turnos para referencia
            var grupos = _ctx.Grupo.ToDictionary(g => g.Idgrupo, g => g.Nombre);
            var turnos = _ctx.Turno.ToDictionary(t => t.Idturno, t => t);

            // Obtener punches de la semana
            var punchesSemana = _ctx.Punches
                .Where(p => p.Fechapunch >= startDate && p.Fechapunch <= endDate)
                .ToList();

            var resultado = new List<ReporteSemanalViewModel>();
            foreach (var emp in empleados)
            {
                var subgrupo = emp.IdsubgrupoNavigation;
                var grupoNombre = subgrupo != null && grupos.ContainsKey((uint)subgrupo.Idgrupo) ? grupos[(uint)subgrupo.Idgrupo] : "";
                var turnoObj = subgrupo != null && turnos.ContainsKey((uint)subgrupo.Idturno) ? turnos[(uint)subgrupo.Idturno] : null;
                var turnoNombre = turnoObj?.Nombre ?? "";
                var horaEntrada = turnoObj?.Horaentrada.TimeOfDay ?? TimeSpan.Zero;

                // Verificar si el empleado tiene incidencias activas en el rango de fechas
                var incidencia = _ctx.Incidenciaxempleado
                    .Where(ie => ie.Idempleado == emp.Idempleado && 
                                 ie.RegStatus == "A" &&
                                 ie.Fechainicio <= endDate && 
                                 ie.Fechafin >= startDate)
                    .Include(ie => ie.IdincidenciaNavigation)
                    .FirstOrDefault();

                var incidenciaNombre = incidencia != null ? incidencia.IdincidenciaNavigation?.Descripcion ?? "" : "";

                var vm = new ReporteSemanalViewModel
                {
                    IdEmpleado =(uint) emp.Credencial,
                    NombreEmpleado = $"{emp.Primernombre} {emp.Apellidopaterno} {emp.Apellidomaterno}",
                    Grupo = grupoNombre,
                    Turno = turnoNombre,
                    Incidencia = incidenciaNombre
                };
                // Procesar cada día de la semana
                for (int i = 0; i < 7; i++)
                {
                    var fecha = startDate.AddDays(i).Date;
                    // Buscar incidencia activa para este empleado en este día
                    var incidenciaDia = _ctx.Incidenciaxempleado
                        .Where(ie => ie.Idempleado == emp.Idempleado &&
                                     ie.RegStatus == "A" &&
                                     ie.Fechainicio.Date <= fecha &&
                                     ie.Fechafin.Date >= fecha)
                        .Include(ie => ie.IdincidenciaNavigation)
                        .FirstOrDefault();
                    var dia = new DiaReporte();
                    if (incidenciaDia != null)
                    {
                        dia.IncidenciaDia = incidenciaDia.IdincidenciaNavigation?.Descripcion ?? "Incidencia";
                        dia.Estado = dia.IncidenciaDia;
                    }
                    else
                    {
                        var punchesDia = punchesSemana.Where(p => p.Idempleado == emp.Credencial && p.Fechapunch.Date == fecha).ToList();
                        var entrada = punchesDia.Where(p => p.Softkey.Trim().ToUpper() == "IN").OrderBy(p => p.Fechapunch).FirstOrDefault();
                        var salida = punchesDia.Where(p => p.Softkey.Trim().ToUpper() == "OU").OrderByDescending(p => p.Fechapunch).FirstOrDefault();
                        if (entrada == null && salida == null)
                        {
                            dia.Estado = "Inasistencia";
                        }
                        else if (entrada == null || salida == null)
                        {
                            dia.Estado = "Omisión de registro";
                            dia.Omision = true;
                            dia.Entrada = entrada?.Fechapunch.ToString("HH:mm:ss", CultureInfo.InvariantCulture) ?? "-";
                            dia.Salida = salida?. Fechapunch.ToString("HH:mm:ss", CultureInfo.InvariantCulture) ?? "-";
                        }
                        else
                        {
                            dia.Entrada = entrada.Fechapunch.ToString("HH:mm:ss", CultureInfo.InvariantCulture);
                            dia.Salida = salida.Fechapunch.ToString("HH:mm:ss", CultureInfo.InvariantCulture);
                            bool esRetardo = punchesDia.Any(p => p.Penalizacion == 1);
                            if (esRetardo)
                            {
                                dia.Estado = "Retardo";
                                dia.Retardo = true;
                            }
                            else
                            {
                                dia.Estado = $"{entrada.Fechapunch:HH:mm:ss} - {salida.Fechapunch:HH:mm:ss}";
                            }
                        }
                    }
                    switch (i)
                    {
                        case 0: vm.Lunes = dia; break;
                        case 1: vm.Martes = dia; break;
                        case 2: vm.Miercoles = dia; break;
                        case 3: vm.Jueves = dia; break;
                        case 4: vm.Viernes = dia; break;
                        case 5: vm.Sabado = dia; break;
                        case 6: vm.Domingo = dia; break;
                    }
                }
                resultado.Add(vm);
            }
            return View(resultado);
        }

        [HttpPost]
        public IActionResult ExportarExcel(string jsonData)
        {
            var data = System.Text.Json.JsonSerializer.Deserialize<List<Dictionary<string, string>>>(jsonData);
            using (var workbook = new ClosedXML.Excel.XLWorkbook())
            {
                var ws = workbook.Worksheets.Add("Reporte Semanal");
                
                // Calcular fechas de la semana (usar la semana actual o la que corresponda)
                var year = DateTime.Now.Year;
                var firstDayOfYear = new DateTime(year, 1, 1);
                var daysOffset = DayOfWeek.Monday - firstDayOfYear.DayOfWeek;
                var firstMonday = firstDayOfYear.AddDays(daysOffset);
                var startDate = firstMonday.AddDays((DateTime.Now.DayOfYear / 7) * 7); // Semana actual aproximada
                
                string GetDiaYFecha(int offset, string nombreDia)
                {
                    var fecha = startDate.AddDays(offset);
                    return $"{nombreDia}\n{fecha:dd-MMM-yyyy}";
                }
                
                // Encabezados con fechas
                ws.Cell(1, 1).Value = "Empleado";
                ws.Cell(1, 2).Value = "Nombre";
                ws.Cell(1, 3).Value = "Incidencia";
                ws.Cell(1, 4).Value = GetDiaYFecha(0, "Lunes");
                ws.Cell(1, 5).Value = GetDiaYFecha(1, "Martes");
                ws.Cell(1, 6).Value = GetDiaYFecha(2, "Miércoles");
                ws.Cell(1, 7).Value = GetDiaYFecha(3, "Jueves");
                ws.Cell(1, 8).Value = GetDiaYFecha(4, "Viernes");
                ws.Cell(1, 9).Value = GetDiaYFecha(5, "Sábado");
                ws.Cell(1, 10).Value = GetDiaYFecha(6, "Domingo");
                
                // Aplicar estilo azul a los encabezados
                var headerRange = ws.Range(1, 1, 1, 10);
                headerRange.Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.FromArgb(68, 114, 196); // Azul
                headerRange.Style.Font.FontColor = ClosedXML.Excel.XLColor.White;
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
                headerRange.Style.Alignment.Vertical = ClosedXML.Excel.XLAlignmentVerticalValues.Center;
                headerRange.Style.Alignment.WrapText = true;
                headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                headerRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                
                // Ajustar ancho de columnas
                ws.Column(1).Width = 13; // Empleado
                ws.Column(2).Width = 25; // Nombre
                ws.Column(3).Width = 20; // Incidencia
                ws.Column(4).Width = 20; // Lunes
                ws.Column(5).Width = 20; // Martes
                ws.Column(6).Width = 20; // Miércoles
                ws.Column(7).Width = 20; // Jueves
                ws.Column(8).Width = 20; // Viernes
                ws.Column(9).Width = 20; // Sábado
                ws.Column(10).Width = 20; // Domingo
                
                int row = 2;
                foreach (var item in data)
                {
                    string FormatCell(string valor, out string tipo)
                    {
                        tipo = "";
                        if(valor == null) return "";
                        if(valor.Contains("Inasistencia")) { tipo = "inasistencia"; return "Inasistencia"; }
                        if(valor.Contains("Omisión de registro")) { tipo = "omision"; }
                        if(valor.Contains("Retardo")) { tipo = "retardo"; }
                        // Si contiene 'Entrada:' y 'Salida:'
                        if(valor.Contains("Entrada:") && valor.Contains("Salida:"))
                        {
                            var entrada = "";
                            var salida = "";
                            var partes = valor.Split(',');
                            foreach(var parte in partes)
                            {
                                if(parte.Contains("Entrada:")) entrada = parte.Replace("Entrada:", "").Trim();
                                if(parte.Contains("Salida:")) salida = parte.Replace("Salida:", "").Trim();
                            }
                            if(!string.IsNullOrEmpty(entrada) && !string.IsNullOrEmpty(salida))
                                return $"{entrada}\n{salida}";
                            if(!string.IsNullOrEmpty(entrada)) return entrada;
                            if(!string.IsNullOrEmpty(salida)) return salida;
                        }
                        return valor;
                    }
                    ws.Cell(row, 1).Value = item["Empleado"];
                    ws.Cell(row, 2).Value = item["Nombre"];
                    //ws.Cell(row, 3).Value = item["Incidencia"];
                    string tipo;
                    var celdas = new[]{"Lunes","Martes","Miercoles","Jueves","Viernes","Sabado","Domingo"};
                    for(int i=0;i<7;i++)
                    {
                        var valor = FormatCell(item[celdas[i]], out tipo);
                        var cell = ws.Cell(row, 4+i);
                        cell.Value = valor;
                        // Colores pastel
                        if(tipo=="inasistencia")
                            cell.Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.FromArgb(255, 182, 193); // Rosa pastel
                        else if(tipo=="retardo")
                            cell.Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.FromArgb(255, 255, 153); // Amarillo pastel
                        else if(tipo=="omision")
                            cell.Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.FromArgb(230, 230, 250); // Lavanda pastel
                        else
                            cell.Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.White;
                        // Agregar bordes a cada celda
                        cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        // Centrar la información
                        cell.Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
                        cell.Style.Alignment.Vertical = ClosedXML.Excel.XLAlignmentVerticalValues.Center;
                    }
                    // Agregar bordes a las celdas de empleado, nombre e incidencia, y centrar
                    ws.Cell(row, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Cell(row, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Cell(row, 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Cell(row, 1).Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
                    ws.Cell(row, 2).Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
                    ws.Cell(row, 3).Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
                    ws.Cell(row, 1).Style.Alignment.Vertical = ClosedXML.Excel.XLAlignmentVerticalValues.Center;
                    ws.Cell(row, 2).Style.Alignment.Vertical = ClosedXML.Excel.XLAlignmentVerticalValues.Center;
                    ws.Cell(row, 3).Style.Alignment.Vertical = ClosedXML.Excel.XLAlignmentVerticalValues.Center;
                    row++;
                }
                ws.Rows().Style.Alignment.WrapText = true; // Habilitar wrap para que se vean los saltos de línea
                using (var stream = new System.IO.MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Position = 0;
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"ReporteSemanal_{DateTime.Now:yyyyMMddHHmmss}.xlsx");
                }
            }
        }
    }
} 