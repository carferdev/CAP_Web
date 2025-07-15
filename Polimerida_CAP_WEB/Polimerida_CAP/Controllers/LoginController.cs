using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Polimerida_CAP.Models;
using Polimerida_CAP.Services;

namespace Polimerida_CAP.Controllers
{
    public class LoginController : Controller
    {
        //agregar la variable de la interface IAuthService
        private readonly IAuthService _authService;
        //agregar el constructor para inyectar la interface IAuthService
        public LoginController(IAuthService authService)
        {
            _authService = authService;
        }

        //add action for login using user and password for class UserRequest add validacion for credenntiasls add session for token and redirect to index view add model state for erro to user and passor  
        [HttpPost]
        public IActionResult Login(UserRequest userRequest)
        {
            if (userRequest == null || string.IsNullOrEmpty(userRequest.UserName ) || string.IsNullOrEmpty(userRequest.Password))
            {
                return BadRequest(" ingresa usuario y contraseña");
            }
            try
            {
                var token = _authService.Login(userRequest);
                if (token == null)
                {
                    ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos.");
                    return View("Index", userRequest);
                    // return Unauthorized("Invalid username or password.");
                }
                HttpContext.Session.SetString("Token", token);

                HttpContext.Session.SetString("Usuario", userRequest.UserName );// Solo si usas sesión

                return RedirectToAction("Panel", "Home");
            }
            catch (UnauthorizedAccessException ex)
            {
                ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos.");
                return View("Index", userRequest);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Logout()
        {
            // Limpiar la sesión
            HttpContext.Session.Clear();
            
            // Redireccionar al login
            return RedirectToAction("Index", "Login");
        }
    }
}
