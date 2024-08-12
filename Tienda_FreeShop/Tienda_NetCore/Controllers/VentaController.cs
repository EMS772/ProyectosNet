using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Tienda_NetCore.Models.Data;

namespace Tienda_NetCore.Controllers
{
    public class VentaController : Controller
    {
        private readonly TiendaContext _context;

        public VentaController(TiendaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(usuarioId))
            {
                return RedirectToAction("Login", "Cuenta");
            }

            var ventas = await _context.Ventas
                .Where(v => v.UsuarioId == usuarioId) // Filtrar por el usuario actual
                .ToListAsync();

            return View(ventas);
        }
    }
}
