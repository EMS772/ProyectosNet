using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tienda_NetCore.Models.Entidades;
using Tienda_NetCore.Models;
using Tienda_NetCore.Models.Enums;

namespace Tienda_NetCore.Controllers
{
    public class CuentaController : Controller
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;

        public CuentaController(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }



        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { isValid = false, errorMessage = "Datos de entrada no válidos." });
            }

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return Json(new { isValid = true, redirectUrl = Url.Action("Index", "Home") });
            }

            ModelState.AddModelError(string.Empty, "Intento de inicio de sesión inválido. Verifique su Email y Contraseña.");
            return Json(new { isValid = false, errorMessage = "Intento de inicio de sesión inválido. Verifique su Email y Contraseña." });
        }



        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistroViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new Usuario
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Nombre = model.Nombre,
                    Rol = TipoUsuario.Cliente // Esto establece el rol como Cliente
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // Añadir roles adicionales si es necesario
                    await _userManager.AddToRoleAsync(user, "Cliente");

                    // Código adicional para iniciar sesión al usuario o redirigirlo
                    return RedirectToAction("Login", "Cuenta");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            // Si llegamos aquí, algo falló, vuelve a mostrar el formulario.
            return View(model);
        }



        // Acción para cerrar sesión
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Cuenta");
        }
    }
}
