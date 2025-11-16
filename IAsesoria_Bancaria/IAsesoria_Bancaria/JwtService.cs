using IAsesoria_Bancaria.Models.Dto_s.UserDto_s;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace IAsesoria_Bancaria
{
    public class JwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;

        }

        public string GenerateToken(LoginDTO login)
        {
            var jwtSettings = _configuration.GetSection("Jwt").Get<Jwt>();

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, jwtSettings.Subject),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
              new Claim(JwtRegisteredClaimNames.Iat,
            new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(),
            ClaimValueTypes.Integer64),
                new Claim(ClaimTypes.Email, login.Email),
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSettings.Key));
            var signinCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings.Issuer,
                audience: jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: signinCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
