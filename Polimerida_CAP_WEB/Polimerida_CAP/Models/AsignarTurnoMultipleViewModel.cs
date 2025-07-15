using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Polimerida_CAP.Models;

public class AsignarTurnoMultipleViewModel
{
    public List<EmpleadoTurnoSeleccionViewModel> Empleados { get; set; } = new();
    public List<TurnosViewModel> TurnosDisponibles { get; set; } = new();
}

public class EmpleadoTurnoSeleccionViewModel
{
    public int IdEmpleado { get; set; }
    public string NombreCompleto { get; set; } = string.Empty;
    public bool Seleccionado { get; set; }
    [Display(Name = "Turno a asignar")]
    public int? IdTurnoSeleccionado { get; set; }
    public string? TurnoActual { get; set; }
} 