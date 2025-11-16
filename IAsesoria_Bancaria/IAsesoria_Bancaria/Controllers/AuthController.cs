using IAsesoria_Bancaria.Models.Dto_s.UserDto_s;
using IAsesoria_Bancaria.Models.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IAsesoria_Bancaria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login([FromBody] LoginDTO loginDTO)
        {
            try
            {
                var result = _authService.LoginUser(loginDTO);
                return Ok(new {success=true , Informacion=result, Message="Login Exitoso" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult Register([FromBody] CreateUserDTO createUserDTO)
        {
            try
            {
                var result = _authService.CreateUserDTO(createUserDTO);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
