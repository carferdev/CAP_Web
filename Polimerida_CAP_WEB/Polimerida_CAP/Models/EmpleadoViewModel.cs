using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Polimerida_CAP.Models;

public class EmpleadoViewModel
{
    public int IdEmpleado { get; set; }

    [Required, Display(Name="Primer nombre")]
    public string PrimerNombre { get; set; } = string.Empty;

    [Required, Display(Name="Apellido paterno")]
    public string ApellidoPaterno { get; set; } = string.Empty;

    [Display(Name="Apellido materno")]
    public string? ApellidoMaterno { get; set; }

    [EmailAddress, Display(Name="Correo electrónico")]
    public string? Email { get; set; }

    [Display(Name="Departamento")]
    public int? IdDepartamento { get; set; }
    [Display(Name="Dispositivo")]
    public int? IdDispositivo { get; set; }
    [Display(Name="Subgrupo")]
    public int? IdSubgrupo { get; set; }
    [Display(Name="Puesto")]
    public int? IdPuesto { get; set; }

    [Display(Name="Foto"), DataType(DataType.Upload)]
    public IFormFile? Foto { get; set; }

    public string? UrlFoto { get; set; }

    [Display(Name="Estado")]
    public string RegStatus { get; set; } = "A";

    [Display(Name="Sexo")]
    public string Sexo { get; set; } = "male"; // 'male' o 'female'

    [Display(Name="Credencial")]
    public int Credencial { get; set; }

    [Display(Name = "Fecha de ingreso")]
    [DataType(DataType.Date)]
    public DateTime? FechaIngreso { get; set; }

    [Display(Name = "Fecha de Baja")]
    [DataType(DataType.Date)]
    public DateTime? FechaBaja { get; set; }

    [Display(Name="Departamento")]
    public string? DepartamentoNombre { get; set; }

    [Display(Name="Puesto")]
    public string? PuestoNombre { get; set; }

    [Display(Name="Dispositivo")]
    public string? DispositivoNombre { get; set; }

    [Display(Name="Turno")]
    public string? TurnoNombre { get; set; }
} 