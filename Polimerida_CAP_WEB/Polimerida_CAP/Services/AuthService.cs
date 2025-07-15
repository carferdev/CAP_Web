using Polimerida_CAP.Models;
using Polimerida_CAP.Services.Data;
using Polimerida_CAP.Helpers;
using Microsoft.Extensions.Configuration;

namespace Polimerida_CAP.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public string? Login(UserRequest userRequest)
        {
            // Encriptar el password recibido antes de validar
            var encryptedPassword = JwtHelper.EncryptPassword(userRequest.Password, "Polimerida_CAP");
            var user = _context.Usuario .FirstOrDefault(u => u.Usuario1 == userRequest.UserName && u.Password == encryptedPassword && u.RegStatus == "A");
            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid user or password.");
            }
            // Obtener los valores de configuración para JWT
            var key = _configuration["Jwt:Key"];
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            return JwtHelper.GenerateToken(user.Usuario1, (int)user.Idusuario, key, issuer, audience);
        }
    }
}
