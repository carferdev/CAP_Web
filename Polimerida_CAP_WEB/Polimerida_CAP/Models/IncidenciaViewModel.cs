using System.ComponentModel.DataAnnotations;

namespace Polimerida_CAP.Models;

public class IncidenciaViewModel
{
    public int IdIncidencia { get; set; }

    [Required, Display(Name = "Código")]
    public string Codigo { get; set; } = string.Empty;

    [Required, Display(Name = "Descripción")]
    public string Descripcion { get; set; } = string.Empty;

    [Display(Name = "Estado")]
    public string RegStatus { get; set; } = "A";
    
    // Campos adicionales para el panel
    public string NombreEmpleado { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public DateTime Fecha { get; set; } = DateTime.Now;
    public string Estado { get; set; } = "Pendiente";
} 