using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Tienda_NetCore.Models.Data;
using Tienda_NetCore.Models.Entidades;
using Tienda_NetCore.Services;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

namespace Tienda_NetCore.Controllers
{
    public class CarritoController : Controller
    {
        private readonly TiendaContext _context;
        private readonly IUsuarioService _usuarioService;
        private readonly UserManager<Usuario> _userManager;
        private readonly ILogger<CarritoController> _logger;
        private readonly ICarritoService _carritoService;



        public CarritoController(TiendaContext context, IUsuarioService usuarioService, UserManager<Usuario> userManager, ILogger<CarritoController> logger, ICarritoService carritoService)
        {
            _context = context;
            _usuarioService = usuarioService;
            _userManager = userManager;
            _logger = logger;
            _carritoService = carritoService;
        }

        [HttpPost]
        public async Task<IActionResult> AgregarAlCarrito(int productoId)
        {
            try
            {
                var usuarioId = await GetUsuarioIdActual();
                if (string.IsNullOrEmpty(usuarioId))
                {
                    return Json(new { success = false, message = "Usuario no autenticado" });
                }

                var producto = await _context.Productos.FindAsync(productoId);
                if (producto == null)
                {
                    return Json(new { success = false, message = "Producto no encontrado" });
                }

                var carrito = await _context.Carritos
                    .Include(c => c.CarritoItems)
                    .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);

                if (carrito == null)
                {
                    carrito = new Carrito { UsuarioId = usuarioId };
                    _context.Carritos.Add(carrito);
                    await _context.SaveChangesAsync(); // Guardar para obtener el Id del carrito
                }

                var carritoItem = carrito.CarritoItems.FirstOrDefault(ci => ci.ProductoId == productoId);
                if (carritoItem == null)
                {
                    carritoItem = new CarritoItem { ProductoId = productoId, Cantidad = 1 };
                    carrito.CarritoItems.Add(carritoItem);
                }
                else
                {
                    carritoItem.Cantidad++;
                }

                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al agregar al carrito: {ex.Message}");
                return Json(new { success = false, message = "Error al agregar el producto al carrito" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> MostrarCarrito()
        {
            var usuarioId = await GetUsuarioIdActual();
            if (string.IsNullOrEmpty(usuarioId))
            {
                return RedirectToAction("Login", "Cuenta");
            }

            var carrito = await _context.Carritos
                .Include(c => c.CarritoItems)
                .ThenInclude(ci => ci.Producto)
                .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);

            if (carrito == null || !carrito.CarritoItems.Any())
            {
                return View("CarritoVacio");
            }

            return View(carrito);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerCantidadCarrito()
        {
            var usuarioId = await GetUsuarioIdActual();
            if (string.IsNullOrEmpty(usuarioId))
            {
                return Json(0);
            }

            var cantidad = await _context.Carritos
                .Where(c => c.UsuarioId == usuarioId)
                .SelectMany(c => c.CarritoItems)
                .SumAsync(ci => ci.Cantidad);

            return Json(cantidad);
        }

        private async Task<string> GetUsuarioIdActual()
        {
            if (!User.Identity.IsAuthenticated)
            {
                _logger.LogWarning("Usuario no autenticado");
                return null;
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Obtener el User ID del Claim
            if (!string.IsNullOrEmpty(userId))
            {
                _logger.LogInformation($"Usuario obtenido: {userId}");
                return userId;
            }

            _logger.LogWarning("No se pudo obtener el ID del usuario actual");
            return null;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                _carritoService.EliminarDelCarrito(id);
                return Json(new { success = true, message = "Producto eliminado del carrito" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar del carrito: {ex.Message}");
                return Json(new { success = false, message = "Error al eliminar el producto del carrito" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ActualizarCantidad(int id, int cantidad)
        {
            try
            {
                var usuarioId = await GetUsuarioIdActual();
                if (string.IsNullOrEmpty(usuarioId))
                {
                    return Json(new { success = false, message = "Usuario no autenticado" });
                }

                var carritoItem = await _context.CarritoItems
                    .Include(ci => ci.Producto)
                    .FirstOrDefaultAsync(ci => ci.Id == id && ci.Carrito.UsuarioId == usuarioId);

                if (carritoItem == null)
                {
                    return Json(new { success = false, message = "Ítem no encontrado en el carrito" });
                }

                if (cantidad < 1)
                {
                    return Json(new { success = false, message = "La cantidad debe ser al menos 1" });
                }

                if (cantidad > carritoItem.Producto.Stock)
                {
                    return Json(new { success = false, message = $"Solo hay {carritoItem.Producto.Stock} unidades disponibles" });
                }

                carritoItem.Cantidad = cantidad;
                await _context.SaveChangesAsync();

                var nuevoTotal = carritoItem.Cantidad * carritoItem.Producto.Precio;
                var nuevoSubtotal = await _context.CarritoItems
                    .Where(ci => ci.Carrito.UsuarioId == usuarioId)
                    .SumAsync(ci => ci.Cantidad * ci.Producto.Precio);

                return Json(new
                {
                    success = true,
                    message = "Cantidad actualizada",
                    nuevoTotal = nuevoTotal.ToString("C"),
                    nuevoSubtotal = nuevoSubtotal.ToString("C")
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar la cantidad: {ex.Message}");
                return Json(new { success = false, message = "Error al actualizar la cantidad" });
            }
        }

        [HttpPost]
        public IActionResult EliminarDelCarrito([FromBody] int id)
        {
            if (ModelState.IsValid)
            {
                _carritoService.EliminarDelCarrito(id);
                return Json(new { success = true, message = "Ítem eliminado" });
            }
            return Json(new { success = false, message = "No se pudo eliminar el ítem" });
        }

        public async Task<IActionResult> ConfirmarPedido()
        {
            var usuarioId = await GetUsuarioIdActual();
            if (string.IsNullOrEmpty(usuarioId))
            {
                return RedirectToAction("Login", "Cuenta");
            }

            var carrito = await _context.Carritos
                .Include(c => c.CarritoItems)
                .ThenInclude(ci => ci.Producto)
                .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);

            if (carrito == null || !carrito.CarritoItems.Any())
            {
                return View("CarritoVacio");
            }

            var total = carrito.CarritoItems.Sum(ci => ci.Cantidad * ci.Producto.Precio);

            var venta = new Venta
            {
                UsuarioId = usuarioId,
                Fecha = DateTime.Now,
                Cantidad = carrito.CarritoItems.Sum(ci => ci.Cantidad),
                Total = (decimal)total // Convertir explícitamente a decimal
            };

            _context.Ventas.Add(venta);
            _context.Carritos.Remove(carrito);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home"); // Redirige a la vista donde se muestran las ventas
        }

        public IActionResult CarritoVacio()
        {
            return View();
        }

    }
}
