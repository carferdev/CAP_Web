using System.ComponentModel.DataAnnotations;

namespace Polimerida_CAP.Models;

public class DispositivoViewModel
{
    public int IdDispositivo { get; set; }

    [Required, StringLength(250)]
    [Display(Name="Descripción")]
    public string Descripcion { get; set; } = string.Empty;

    [StringLength(250)]
    public string Clase { get; set; } = string.Empty;

    [StringLength(250)]
    public string Division { get; set; } = string.Empty;

    [StringLength(250)]
    public string Tipo { get; set; } = string.Empty;

    [Display(Name="IP")]
    [StringLength(250)]
    public string Ip { get; set; } = string.Empty;

    [Display(Name="Estado")]
    public string RegStatus { get; set; } = "A";
} 