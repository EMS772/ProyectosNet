using BCrypt.Net;
using IAsesoria_Bancaria.Models.Data;
using IAsesoria_Bancaria.Models.Dto_s.User;
using IAsesoria_Bancaria.Models.Dto_s.UserDto_s;
using IAsesoria_Bancaria.Models.Entities;
using IAsesoria_Bancaria.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IAsesoria_Bancaria.Models.Services
{
    public class AuthServices : IAuthService
    {
        private readonly IAB_DbContext _context;
        private readonly JwtService _jwtService; 

        public AuthServices(IAB_DbContext iAB_DbContext, JwtService jwtService)
        {
            _context = iAB_DbContext;
            _jwtService = jwtService;
        }

        public UserDTO CreateUserDTO(CreateUserDTO createUserDTO)
        {
            if (string.IsNullOrWhiteSpace(createUserDTO.Username) ||
                string.IsNullOrWhiteSpace(createUserDTO.Email) ||
                string.IsNullOrWhiteSpace(createUserDTO.PasswordHash))
            {
                throw new ArgumentException("Todos los campos son requeridos");
            }

            var user = new User
            {
                Username = createUserDTO.Username,
                Email = createUserDTO.Email,
                PasswordHash = createUserDTO.PasswordHash.ToBcryptHash(),
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                CreatedAt = user.CreatedAt
            };
        }

        public UserLoginResponseDTO LoginUser(LoginDTO login)
        {
            if (string.IsNullOrWhiteSpace(login.Email) || string.IsNullOrWhiteSpace(login.PasswordHash))
            {
                throw new ArgumentException("Email y contraseña son requeridos");
            }

            var user = _context.Users.FirstOrDefault(u => u.Email == login.Email);

            if (user == null)
            {
                throw new Exception("Usuario no encontrado");
            }

            if (!BCrypt.Net.BCrypt.Verify(login.PasswordHash, user.PasswordHash))
            {
                throw new Exception("Contraseña inválida");
            }

            var token = _jwtService.GenerateToken(login);

            return new UserLoginResponseDTO
            {
                User = new UserDTO
                {
                    Id = user.Id, 
                    Username = user.Username, 
                    Email = user.Email,
                    CreatedAt = user.CreatedAt
                },
                Token = token
            };
        }
    }
}