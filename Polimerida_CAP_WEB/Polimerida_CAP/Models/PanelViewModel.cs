namespace Polimerida_CAP.Models
{
    public class PanelViewModel
    {
        public int EmpleadosPresentes { get; set; }
        public int EmpleadosAusentes { get; set; }
        public int EmpleadosJustificados { get; set; }
        public int TotalEmpleados { get; set; }
        public double PorcentajeAsistencia { get; set; }
        
        // Datos por departamentos
        public List<DepartamentoEstadistica> EstadisticasPorDepartamento { get; set; } = new List<DepartamentoEstadistica>();
        
        // Datos por grupos
        public List<GrupoEstadistica> EstadisticasPorGrupo { get; set; } = new List<GrupoEstadistica>();
        
        // Dispositivos activos
        public List<DispositivoEstadistica> DispositivosActivos { get; set; } = new List<DispositivoEstadistica>();
        
        // Últimas incidencias de empleados
        public List<IncidenciaEmpleadoViewModel> UltimasIncidenciasEmpleados { get; set; } = new List<IncidenciaEmpleadoViewModel>();
    }

    public class DepartamentoEstadistica
    {
        public int IdDepartamento { get; set; }
        public string NombreDepartamento { get; set; } = string.Empty;
        public int TotalEmpleados { get; set; }
        public int Presentes { get; set; }
        public int Ausentes { get; set; }
        public int Justificados { get; set; }
        public double PorcentajeAsistencia { get; set; }
    }

    public class GrupoEstadistica
    {
        public int IdGrupo { get; set; }
        public string NombreGrupo { get; set; } = string.Empty;
        public int TotalEmpleados { get; set; }
        public int Presentes { get; set; }
        public int Ausentes { get; set; }
        public int Justificados { get; set; }
        public double PorcentajeAsistencia { get; set; }
    }

    public class DispositivoEstadistica
    {
        public int IdDispositivo { get; set; }
        public string NombreDispositivo { get; set; } = string.Empty;
        public string Ip { get; set; } = string.Empty;
        public bool Estado { get; set; }
        public int EmpleadosRegistrados { get; set; }
    }
}
