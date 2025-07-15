using System.ComponentModel.DataAnnotations;

namespace Polimerida_CAP.Models;

public class FestivoViewModel
{
    public int IdFestivo { get; set; }

    [Required, Display(Name = "Fecha")]
    [DataType(DataType.Date)]
    public DateTime Fecha { get; set; } = DateTime.Today;

    [Required, Display(Name = "Descripción")]
    public string Descripcion { get; set; } = string.Empty;

    [Display(Name = "Estado")]
    public string RegStatus { get; set; } = "A";
} 