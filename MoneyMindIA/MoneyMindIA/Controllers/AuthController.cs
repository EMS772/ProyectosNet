using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoneyMindIA.Models.Data;
using MoneyMindIA.Models.Entidades;
using MoneyMindIA.Models.ViewModels;
using MoneyMindIA.Services;
using System.Security.Claims;
using System.Security.Cryptography;

namespace MoneyMindIA.Controllers
{
    public class AuthController : Controller
    {
        private readonly MoneyMindDbContext _context;
        private readonly ILogger<AuthController> _logger;
        private readonly IEmailService _emailService;

        public AuthController(
            MoneyMindDbContext context,
            ILogger<AuthController> logger,
            IEmailService emailService
        )
        {
            _context = context;
            _logger = logger;
            _emailService = emailService;
        }


        // Acciones de Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == model.Email);

                if (usuario != null && BCrypt.Net.BCrypt.Verify(model.Password, usuario.PasswordHash))
                {
                    // Verificar si el email está confirmado
                    if (!usuario.EmailConfirmed)
                    {
                        TempData["EmailNotConfirmed"] = "Debes confirmar tu correo electrónico antes de iniciar sesión.";
                        ModelState.AddModelError("", "Correo no confirmado");
                        return View(model);
                    }

                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.Nombre),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim("UsuarioId", usuario.UsuarioId.ToString())
            };

                    var identity = new ClaimsIdentity(claims, "CookieAuth");
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync("CookieAuth", principal);
                    return RedirectToAction("Dashboard", "Home");
                }

                ModelState.AddModelError("", "Credenciales inválidas");
            }

            return View(model);
        }


        // Acciones de Registro
        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registro(RegistroViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // Verificar si el email ya existe (doble verificación)
                if (await _context.Usuarios.AnyAsync(u => u.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "Este email ya está registrado");
                    return View(model);
                }

                // Crear el usuario
                var usuario = new Usuario
                {
                    Nombre = model.Nombre,
                    Email = model.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                    EsRegistroNormal = true,
                    EmailConfirmed = false,
                    EmailConfirmationToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32)),
                    ConfirmationTokenExpiry = DateTime.UtcNow.AddHours(24)
                };

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                // Generar link de confirmación
                var confirmationLink = Url.Action("ConfirmarEmail", "Auth",
                    new { userId = usuario.UsuarioId, token = usuario.EmailConfirmationToken },
                    Request.Scheme);

                try
                {
                    await _emailService.SendConfirmationEmailAsync(usuario, confirmationLink);
                    TempData["Mensaje"] = "Registro exitoso. Por favor, revisa tu correo para confirmar tu cuenta.";
                }
                catch (Exception emailEx)
                {
                    _logger.LogError(emailEx, "Error enviando email de confirmación");
                    TempData["Mensaje"] = "Registro exitoso, pero hubo un problema enviando el email de confirmación.";
                }

                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante el registro de usuario");
                ModelState.AddModelError("", "Ocurrió un error durante el registro. Por favor, intenta nuevamente.");
                return View(model);
            }
        }
        //private string GenerateToken()
        //{
        //    return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        //}


        [HttpGet]
        public async Task<IActionResult> VerifyEmail(string email)
        {
            var emailExists = await _context.Usuarios.AnyAsync(u => u.Email == email);
            return Json(!emailExists);
        }


        [HttpGet]
        public async Task<IActionResult> ConfirmarEmail(int userId, string token)
        {
            var usuario = await _context.Usuarios.FindAsync(userId);

            if (usuario == null ||
                usuario.EmailConfirmationToken != token ||
                usuario.ConfirmationTokenExpiry < DateTime.UtcNow)
            {
                return View("ErrorConfirmacion");
            }

            usuario.EmailConfirmed = true;
            usuario.EmailConfirmationToken = null;
            await _context.SaveChangesAsync();

            return View("ConfirmacionExitosa");
        }



        [HttpPost]
        public async Task<IActionResult> ResendConfirmationEmail([FromQuery] string email)
        {
            _logger.LogInformation($"Intento de reenvío de correo para: {email}");

            try
            {
                var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);

                if (usuario == null)
                {
                    _logger.LogWarning($"Usuario no encontrado: {email}");
                    return BadRequest("Usuario no encontrado");
                }

                if (usuario.EmailConfirmed)
                {
                    _logger.LogInformation($"Email ya confirmado: {email}");
                    return BadRequest("El email ya está confirmado");
                }

                // Generar nuevo token
                usuario.EmailConfirmationToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
                usuario.ConfirmationTokenExpiry = DateTime.UtcNow.AddHours(24);

                var confirmationLink = Url.Action("ConfirmarEmail", "Auth",
                    new { userId = usuario.UsuarioId, token = usuario.EmailConfirmationToken },
                    Request.Scheme);

                if (string.IsNullOrEmpty(confirmationLink))
                {
                    throw new InvalidOperationException("Error generando el link de confirmación");
                }

                _logger.LogInformation($"Enviando correo de confirmación a: {email}");
                await _emailService.SendConfirmationEmailAsync(usuario, confirmationLink);

                await _context.SaveChangesAsync();

                _logger.LogInformation($"Correo reenviado exitosamente a: {email}");
                return Ok("Correo de confirmación reenviado exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en reenvío de correo para: {email}");
                return StatusCode(500, $"Error al enviar el correo de confirmación: {ex.Message}");
            }
        }






        // Añade estas acciones de Registro y Login con Google
        [HttpGet]
        public IActionResult LoginWithGoogle()
        {
            var redirectUrl = Url.Action("GoogleResponse", "Auth");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync("CookieAuth");

            if (!result.Succeeded)
            {
                return RedirectToAction("Login", new { error = "Error en autenticación con Google" });
            }

            // Obtener datos de Google
            var email = result.Principal.FindFirst(ClaimTypes.Email)?.Value;
            var googleId = result.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var name = result.Principal.FindFirst(ClaimTypes.Name)?.Value;
            var googlePhoto = result.Principal.FindFirst("picture")?.Value; // <-- Foto de Google

            var user = _context.Usuarios.FirstOrDefault(u => u.GoogleId == googleId || u.Email == email);

            if (user == null)
            {
                user = new Usuario
                {
                    Nombre = name,
                    Email = email,
                    GoogleId = googleId,
                    EsRegistroNormal = false,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(Guid.NewGuid().ToString())
                };

                _context.Usuarios.Add(user);
                await _context.SaveChangesAsync();
            }

            // Crear claims incluyendo la foto de Google
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Nombre),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("UsuarioId", user.UsuarioId.ToString()),
                new Claim("google_photo", googlePhoto ?? "") // <-- Añadir foto como claim
            };

            var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");
            await HttpContext.SignInAsync("CookieAuth", new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Dashboard", "Home");
        }

        // Acciones de contraseña olvidada y reset contraseña

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == model.Email);

            if (usuario != null)
            {
                usuario.ResetPasswordToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
                usuario.ResetPasswordTokenExpiry = DateTime.UtcNow.AddHours(1);

                await _context.SaveChangesAsync();

                var resetLink = Url.Action("ResetPassword", "Auth",
                    new { email = usuario.Email, token = usuario.ResetPasswordToken },
                    Request.Scheme);

                try
                {
                    await _emailService.SendPasswordResetEmailAsync(usuario, resetLink);
                    TempData["SuccessMessage"] = "Se ha enviado un enlace de recuperación a tu correo electrónico.";
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error enviando email de reseteo de contraseña");
                    TempData["ErrorMessage"] = "Error al enviar el correo de recuperación.";
                }
            }
            else
            {
                // Siempre mostrar el mismo mensaje aunque el email no exista (seguridad)
                TempData["SuccessMessage"] = "Si el correo existe en nuestra base de datos, recibirás un enlace de recuperación.";
            }

            return RedirectToAction("Login");
        }

        [HttpGet]
        public async Task<IActionResult> ResetPassword(string email, string token)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u =>
                u.Email == email &&
                u.ResetPasswordToken == token &&
                u.ResetPasswordTokenExpiry > DateTime.UtcNow);

            if (usuario == null)
                return View("ErrorResetPassword");

            var model = new ResetPasswordViewModel
            {
                Email = email,
                Token = token
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u =>
                u.Email == model.Email &&
                u.ResetPasswordToken == model.Token &&
                u.ResetPasswordTokenExpiry > DateTime.UtcNow);

            if (usuario == null)
                return View("ErrorResetPassword");

            usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
            usuario.ResetPasswordToken = null;
            usuario.ResetPasswordTokenExpiry = null;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Tu contraseña ha sido actualizada correctamente.";
            return RedirectToAction("Login");
        }
    }
}