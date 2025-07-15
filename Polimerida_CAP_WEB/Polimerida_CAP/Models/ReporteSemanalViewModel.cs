using System;
using System.Collections.Generic;

namespace Polimerida_CAP.Models
{
    public class ReporteSemanalViewModel
    {
        public uint IdEmpleado { get; set; }
        public string NombreEmpleado { get; set; } = string.Empty;
        public string Grupo { get; set; } = string.Empty;
        public string Turno { get; set; } = string.Empty;
        public string Incidencia { get; set; } = string.Empty; // Nueva propiedad para incidencias
        // Un campo por cada día de la semana
        public DiaReporte Lunes { get; set; } = new DiaReporte();
        public DiaReporte Martes { get; set; } = new DiaReporte();
        public DiaReporte Miercoles { get; set; } = new DiaReporte();
        public DiaReporte Jueves { get; set; } = new DiaReporte();
        public DiaReporte Viernes { get; set; } = new DiaReporte();
        public DiaReporte Sabado { get; set; } = new DiaReporte();
        public DiaReporte Domingo { get; set; } = new DiaReporte();
    }

    public class DiaReporte
    {
        public string Estado { get; set; } = ""; // "Inasistencia", "Retardo", "Omisión", "OK"
        public string? Entrada { get; set; }
        public string? Salida { get; set; }
        public bool Omision { get; set; } = false;
        public bool Retardo { get; set; } = false;
        public string? IncidenciaDia { get; set; } // Nueva propiedad para incidencia por día
    }
} 