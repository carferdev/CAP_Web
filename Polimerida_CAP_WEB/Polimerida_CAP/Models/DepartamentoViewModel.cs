using System.ComponentModel.DataAnnotations;

namespace Polimerida_CAP.Models
{
    public class DepartamentoViewModel
    {
        public int IdDepartamento { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [StringLength(250, ErrorMessage = "Máximo 250 caracteres.")]
        [Display(Name = "Descripción del departamento")]
        public string Descripcion { get; set; } = string.Empty;

        [Display(Name = "Estado")]
        public string RegStatus { get; set; } = "A";
    }
} 