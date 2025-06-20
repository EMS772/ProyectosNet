using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MoneyMindIA.Models;
using MoneyMindIA.Models.Data;
using System.Diagnostics;
using System.Security.Claims;

namespace MoneyMindIA.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MoneyMindDbContext _context;

        public HomeController(ILogger<HomeController> logger, MoneyMindDbContext context)
        {
            _logger = logger;
            _context = context;

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Wallet()
        {
            // Obtiene el ID del usuario y lo pasa a la vista
            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.UsuarioId = usuarioId;

            return View();
        }
        public IActionResult Chat()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            // Cargar las categorías desde la base de datos
            var categorias = await _context.Categorias.ToListAsync();
            ViewBag.Categorias = new SelectList(categorias, "CategoriaId", "Nombre");

            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Registro()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
