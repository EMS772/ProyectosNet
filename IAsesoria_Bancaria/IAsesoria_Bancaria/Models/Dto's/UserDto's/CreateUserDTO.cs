namespace IAsesoria_Bancaria.Models.Dto_s.UserDto_s
{
    public class CreateUserDTO
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
