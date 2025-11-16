using IAsesoria_Bancaria.Models.Dto_s.UserDto_s;

namespace IAsesoria_Bancaria.Models.Dto_s.User
{
    public class UserLoginResponseDTO
    {
        public UserDTO User { get; set; }
        public string Token { get; set; }
    }
}
