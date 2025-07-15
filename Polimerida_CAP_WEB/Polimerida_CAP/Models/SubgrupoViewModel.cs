using System.ComponentModel.DataAnnotations;

namespace Polimerida_CAP.Models;

public class SubgrupoViewModel
{
    public int IdSubgrupo { get; set; }

    [Required]
    [StringLength(250)]
    [Display(Name = "Nombre del subgrupo")]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(250)]
    [Display(Name = "Descripción")]
    public string? Descripcion { get; set; }

    [Display(Name = "Grupo")]
    public int? IdGrupo { get; set; }

    [Display(Name = "Turno")]
    public int? IdTurno { get; set; }

    [Display(Name = "Estado")]
    public string? RegStatus { get; set; } = "A"; // "A" activo, "B" inactivo
} 