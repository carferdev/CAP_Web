using System.ComponentModel.DataAnnotations;

namespace Polimerida_CAP.Models
{
    public class GrupoViewModel
    {
        public int IdGrupo { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(250)]
        [Display(Name = "Nombre del grupo")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [StringLength(250)]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; } = string.Empty;

        [Display(Name = "Estado")]
        public string RegStatus { get; set; } = "A";
    }
} 