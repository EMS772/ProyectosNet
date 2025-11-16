using IAsesoria_Bancaria.Models.Dto_s.User;
using IAsesoria_Bancaria.Models.Dto_s.UserDto_s;

namespace IAsesoria_Bancaria.Models.Interfaces
{
    public interface IAuthService
    {
        public UserLoginResponseDTO LoginUser(LoginDTO loginDTO);
        public UserDTO CreateUserDTO(CreateUserDTO createUserDTO);
    }
}
