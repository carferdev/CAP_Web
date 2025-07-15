using System.ComponentModel.DataAnnotations;

namespace Polimerida_CAP.Models
{
    public class TurnosViewModel
    {
        public int IdTurno { get; set; }

        [Required(ErrorMessage = "La hora de entrada es obligatoria.")]
        [DataType(DataType.Time)]
        [Display(Name = "Hora de entrada")]
        public DateTime HoraEntrada { get; set; }

        [Required(ErrorMessage = "La hora de salida es obligatoria.")]
        [DataType(DataType.Time)]
        [Display(Name = "Hora de salida")]
        public DateTime HoraSalida { get; set; }

        [Required(ErrorMessage = "El nombre del turno es obligatorio.")]
        [StringLength(45, ErrorMessage = "Máximo 45 caracteres.")]
        [Display(Name = "Nombre del turno")]
        public string Nombre { get; set; } = string.Empty;

        [Display(Name = "Estado")]
        public string RegStatus { get; set; } = "A";

        [Required(ErrorMessage = "La hora de entrada a comida es obligatoria.")]
        [Display(Name = "Entrada comida")]
        [DataType(DataType.Time)]
        public DateTime Entradacomida { get; set; }

        [Required(ErrorMessage = "La hora de salida de comida es obligatoria.")]
        [Display(Name = "Salida comida")]
        [DataType(DataType.Time)]
        public DateTime Salidacomida { get; set; }
    }
}
