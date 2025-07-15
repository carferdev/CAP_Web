using System.ComponentModel.DataAnnotations;

namespace Polimerida_CAP.Models
{
    public class PuestoViewModel
    {
        public int IdPuesto { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [StringLength(250)]
        [Display(Name = "Descripción del puesto")]
        public string Descripcion { get; set; } = string.Empty;

        [Display(Name = "Estado")]
        public string RegStatus { get; set; } = "A";
    }
} 