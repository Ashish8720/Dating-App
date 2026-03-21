using Dating_App.DTOs;
using Dating_App.Entities;

namespace Dating_App.Interfaces
{
    public interface IAccountService
    {
        Task<UserDTO> Register(RegisterDTO register);

        Task<UserDTO> Login(LoginDTO login);
    }
}
