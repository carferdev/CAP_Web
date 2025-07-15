using System.ComponentModel.DataAnnotations;

namespace Polimerida_CAP.Models;

public class IncidenciaEmpleadoViewModel
{
    public int IdIncidenciaEmpleado { get; set; }

    [Required, Display(Name = "Empleado")]
    public int IdEmpleado { get; set; }

    [Display(Name = "Nombre del Empleado")]
    public string NombreEmpleado { get; set; } = string.Empty;

    [Required, Display(Name = "Incidencia")]
    public int IdIncidencia { get; set; }

    [Display(Name = "Descripción de la Incidencia")]
    public string DescripcionIncidencia { get; set; } = string.Empty;

    [Required, Display(Name = "Fecha de Inicio")]
    [DataType(DataType.Date)]
    public DateTime FechaInicio { get; set; } = DateTime.Today;

    [Required, Display(Name = "Fecha de Fin")]
    [DataType(DataType.Date)]
    public DateTime FechaFin { get; set; } = DateTime.Today;

    [Display(Name = "Estado")]
    public string RegStatus { get; set; } = "A";

    // Propiedades para el dropdown de empleados
    public List<EmpleadoViewModel> Empleados { get; set; } = new();
    
    // Propiedades para el dropdown de incidencias
    public List<IncidenciaViewModel> Incidencias { get; set; } = new();
} 