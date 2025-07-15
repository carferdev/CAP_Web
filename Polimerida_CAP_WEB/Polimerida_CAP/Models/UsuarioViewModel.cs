using System.ComponentModel.DataAnnotations;

namespace Polimerida_CAP.Models
{
    public class UsuarioViewModel
    {
        public uint Idusuario { get; set; }
        [Required]
        public string Usuario { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        public string RegStatus { get; set; } = "A";
    }
} 