using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Tienda_NetCore.Models.Entidades;
using Tienda_NetCore.Services;
using Tienda_NetCore.Models;
using Microsoft.AspNetCore.Identity;

namespace Tienda_NetCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductoService _productoService;
        private readonly UserManager<Usuario> _userManager;

        public HomeController(ILogger<HomeController> logger, IProductoService productoService, UserManager<Usuario> userManager)
        {
            _logger = logger;
            _productoService = productoService;
            _userManager = userManager;
        }

        public IActionResult Index(int? categoria)
        {
            IEnumerable<Producto> productos;
            if (categoria.HasValue)
            {
                productos = _productoService.ObtenerPorCategoria(categoria.Value);
                ViewBag.SelectedCategory = categoria.ToString();
            }
            else
            {
                productos = _productoService.ObtenerTodos();
                ViewBag.SelectedCategory = null;
            }
            var userName = User.Identity.IsAuthenticated ? User.Identity.Name : "Guest";
            ViewBag.UserName = userName;
            return View(productos);
        }

        public IActionResult Detalle(int id)
        {
            var producto = _productoService.ObtenerporID(id);

            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
