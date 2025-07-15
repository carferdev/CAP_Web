using System.ComponentModel.DataAnnotations;

namespace Polimerida_CAP.Models
{
    public class UserRequest
    {
        //add propertis for user name and password

        //add rquired attribute for user name
        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        public string UserName { get; set; }
        // add properties for password
        public string Password { get; set; }

    }
}
