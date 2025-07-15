using Polimerida_CAP.Models;

namespace Polimerida_CAP.Services
{
    public interface IAuthService
    {
       // add method for login using user and passrd for UserRequest class where return jwt token
        string? Login(UserRequest userRequest);
        // add method for register using user and password for UserRequest class where return jwt token
      

    }
}
