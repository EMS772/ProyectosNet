using Microsoft.AspNetCore.Mvc;
using MoneyMindIA.Models.Data;
using MoneyMindIA.Models.Entidades;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using MoneyMindIA.Models;
using MoneyMindIA.Models.ViewModels;
using System.Text.Json;
using MoneyMindIA.Models.TransaccionDto;
using MoneyMindIA.Models.Enum;

namespace MoneyMindIA.Controllers
{
    [Authorize]
    public class WalletController : Controller
    {
        private readonly MoneyMindDbContext _context;
        private readonly ILogger<WalletController> _logger;

        public WalletController(MoneyMindDbContext context, ILogger<WalletController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AgregarTarjeta([FromBody] TarjetaViewModel viewModel)
        {
            try
            {
                // Verificar si el usuario está autenticado
                var isAuthenticated = User.Identity.IsAuthenticated;
                if (!isAuthenticated)
                {
                    return Unauthorized(new { success = false, message = "Usuario no autenticado" });
                }

                // Obtener el usuarioId del claim
                var usuarioId = User.FindFirstValue("UsuarioId");
                if (string.IsNullOrEmpty(usuarioId) || !int.TryParse(usuarioId, out int usuarioIdNumerico))
                {
                    return BadRequest(new { success = false, message = "ID de usuario inválido" });
                }

                // Buscar la billetera asociada al usuario
                var billetera = await _context.Billeteras.FirstOrDefaultAsync(b => b.UsuarioId == usuarioIdNumerico);

                if (billetera == null)
                {
                    // Si no existe una billetera, crear una nueva
                    billetera = new Billetera
                    {
                        UsuarioId = usuarioIdNumerico,
                        PayPalEmail = "",
                        PayPalAccessToken = "",
                        BalanceActual = 0,
                        FechaVinculacion = DateTime.UtcNow
                    };

                    _context.Billeteras.Add(billetera);
                    await _context.SaveChangesAsync();
                }

                // Crear la tarjeta
                var tarjeta = new Tarjeta
                {
                    NombreTitular = viewModel.NombreTitular,
                    NumeroTarjeta = viewModel.NumeroTarjeta,
                    FechaExpiracion = viewModel.FechaExpiracion,
                    CVV = viewModel.CVV,
                    Saldo = 1000.00m, // Asignar saldo inicial de $1000
                    BilleteraId = billetera.BilleteraId
                };

                // Agregar la tarjeta a la base de datos
                _context.Tarjetas.Add(tarjeta);
                await _context.SaveChangesAsync();

                // Retornar una respuesta exitosa con los datos de la tarjeta
                return Ok(new
                {
                    success = true,
                    message = "Tarjeta agregada exitosamente",
                    tarjeta = new
                    {
                        nombreTitular = tarjeta.NombreTitular,
                        numeroTarjeta = tarjeta.NumeroTarjeta,
                        fechaExpiracion = tarjeta.FechaExpiracion,
                        saldo = tarjeta.Saldo // Incluir el saldo en la respuesta
                    }
                });
            }
            catch (Exception ex)
            {
                // Registrar el error en la consola
                Console.WriteLine($"Error en AgregarTarjeta: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");

                // Retornar un mensaje de error en formato JSON
                return StatusCode(500, new { success = false, message = "Error interno del servidor", error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTarjetaVinculada()
        {
            try
            {
                // Verificar si el usuario está autenticado
                var isAuthenticated = User.Identity.IsAuthenticated;
                if (!isAuthenticated)
                {
                    return Unauthorized(new { success = false, message = "Usuario no autenticado" });
                }

                // Obtener el usuarioId del claim
                var usuarioId = User.FindFirstValue("UsuarioId");
                if (string.IsNullOrEmpty(usuarioId) || !int.TryParse(usuarioId, out int usuarioIdNumerico))
                {
                    return BadRequest(new { success = false, message = "ID de usuario inválido" });
                }

                // Buscar la billetera del usuario
                var billetera = await _context.Billeteras
                    .Include(b => b.Tarjetas) // Incluir las tarjetas asociadas
                    .FirstOrDefaultAsync(b => b.UsuarioId == usuarioIdNumerico);

                if (billetera == null || !billetera.Tarjetas.Any())
                {
                    return Ok(new { success = true, tarjetaVinculada = false });
                }

                // Obtener la primera tarjeta vinculada
                var tarjeta = billetera.Tarjetas.First();

                return Ok(new
                {
                    success = true,
                    tarjetaVinculada = true,
                    tarjeta = new
                    {
                        nombreTitular = tarjeta.NombreTitular,
                        numeroTarjeta = tarjeta.NumeroTarjeta, // Ocultar parte del número para seguridad
                        fechaExpiracion = tarjeta.FechaExpiracion,
                        cvv = "***", // No mostrar el CVV por seguridad
                        saldo = tarjeta.Saldo // Incluir el saldo
                    }
                });
            }
            catch (Exception ex)
            {
                // Registrar el error en la consola
                Console.WriteLine($"Error en ObtenerTarjetaVinculada: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");

                // Retornar un mensaje de error en formato JSON
                return StatusCode(500, new { success = false, message = "Error interno del servidor", error = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DesvincularTarjeta()
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return Unauthorized(new { success = false, message = "Usuario no autenticado" });
                }

                // Obtener el usuarioId del claim
                var usuarioId = User.FindFirstValue("UsuarioId");
                if (string.IsNullOrEmpty(usuarioId) || !int.TryParse(usuarioId, out int usuarioIdNumerico))
                {
                    return BadRequest(new { success = false, message = "ID de usuario inválido" });
                }

                // Buscar la tarjeta vinculada al usuario
                var tarjetaVinculada = await _context.Tarjetas
                    .Include(t => t.Billetera)
                    .FirstOrDefaultAsync(t => t.Billetera.UsuarioId == usuarioIdNumerico);

                if (tarjetaVinculada == null)
                {
                    return NotFound(new { success = false, message = "No se encontró una tarjeta vinculada" });
                }

                // Buscar las transacciones asociadas a la tarjeta
                var transacciones = await _context.Transacciones
                    .Where(t => t.TarjetaId == tarjetaVinculada.TarjetaId)
                    .ToListAsync();

                // Desvincular las transacciones
                foreach (var transaccion in transacciones)
                {
                    transaccion.TarjetaId = null;
                }
                await _context.SaveChangesAsync();

                // Ahora eliminar la tarjeta
                _context.Tarjetas.Remove(tarjetaVinculada);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Tarjeta desvinculada exitosamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en DesvincularTarjeta: {Message}", ex.Message);
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error interno del servidor",
                    error = ex.Message,
                    innerException = ex.InnerException?.Message
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AgregarTransaccion([FromBody] TransaccionViewModel viewModel)
        {
            try
            {
                // Verificar si el modelo es válido
                if (viewModel == null)
                {
                    return BadRequest(new { success = false, message = "El modelo no puede ser nulo" });
                }

                // Verificar si el usuario está autenticado
                var isAuthenticated = User.Identity.IsAuthenticated;
                if (!isAuthenticated)
                {
                    return Unauthorized(new { success = false, message = "Usuario no autenticado" });
                }

                // Obtener el usuarioId del claim
                var usuarioId = User.FindFirstValue("UsuarioId");
                if (string.IsNullOrEmpty(usuarioId) || !int.TryParse(usuarioId, out int usuarioIdNumerico))
                {
                    return BadRequest(new { success = false, message = "ID de usuario inválido" });
                }

                // Buscar la billetera asociada al usuario
                var billetera = await _context.Billeteras
                    .Include(b => b.Tarjetas)
                    .FirstOrDefaultAsync(b => b.UsuarioId == usuarioIdNumerico);

                if (billetera == null)
                {
                    return NotFound(new { success = false, message = "Billetera no encontrada" });
                }

                // Obtener la tarjeta vinculada
                var tarjetaVinculada = billetera.Tarjetas.FirstOrDefault();
                if (tarjetaVinculada == null)
                {
                    return BadRequest(new { success = false, message = "No hay tarjeta vinculada" });
                }

                // Crear la transacción
                var transaccion = new Transaccion
                {
                    Monto = viewModel.Monto,
                    Fecha = viewModel.Fecha,
                    Descripcion = viewModel.Descripcion,
                    Tipo = viewModel.Tipo,
                    BilleteraId = billetera.BilleteraId,
                    CategoriaId = viewModel.CategoriaId,
                    TarjetaId = tarjetaVinculada.TarjetaId,
                    UsuarioId = usuarioIdNumerico // Asignar el UsuarioId
                };

                // Actualizar el saldo de la tarjeta según el tipo de transacción
                if (viewModel.Tipo == TipoTransaccion.Gasto)
                {
                    // Verificar si hay saldo suficiente para el gasto
                    if (tarjetaVinculada.Saldo < viewModel.Monto)
                    {
                        return BadRequest(new { success = false, message = "Saldo insuficiente en la tarjeta" });
                    }

                    tarjetaVinculada.Saldo -= viewModel.Monto; // Restar el monto del saldo
                }
                else if (viewModel.Tipo == TipoTransaccion.Ingreso)
                {
                    tarjetaVinculada.Saldo += viewModel.Monto; // Sumar el monto al saldo
                }

                // Agregar la transacción a la base de datos
                _context.Transacciones.Add(transaccion);
                await _context.SaveChangesAsync(); // Guardar cambios en la base de datos

                return Ok(new
                {
                    success = true,
                    message = "Transacción agregada exitosamente",
                    saldoActual = tarjetaVinculada.Saldo // Devolver el saldo actualizado de la tarjeta
                });
            }
            catch (Exception ex)
            {
                // Registrar el error en los logs
                _logger.LogError(ex, "Error en AgregarTransaccion");

                // Retornar un mensaje de error en formato JSON
                return StatusCode(500, new { success = false, message = "Error interno del servidor", error = ex.Message });
            }
        }


        [HttpGet]
        public async Task<IActionResult> ObtenerTransacciones()
        {
            try
            {
                // Verificar si el usuario está autenticado
                var isAuthenticated = User.Identity.IsAuthenticated;
                if (!isAuthenticated)
                {
                    return Unauthorized(new { success = false, message = "Usuario no autenticado" });
                }

                // Obtener el usuarioId del claim
                var usuarioId = User.FindFirstValue("UsuarioId");
                if (string.IsNullOrEmpty(usuarioId) || !int.TryParse(usuarioId, out int usuarioIdNumerico))
                {
                    return BadRequest(new { success = false, message = "ID de usuario inválido" });
                }

                // Buscar la tarjeta vinculada al usuario
                var tarjetaVinculada = await _context.Tarjetas
                    .Include(t => t.Billetera) // Incluir la billetera
                    .FirstOrDefaultAsync(t => t.Billetera.UsuarioId == usuarioIdNumerico);

                if (tarjetaVinculada == null)
                {
                    // No hay tarjeta vinculada, devolver mensaje
                    return Ok(new
                    {
                        success = true,
                        message = "No hay tarjeta vinculada",
                        transacciones = new List<TransaccionDto>()
                    });
                }

                // Obtener transacciones de la TARJETA vinculada
                var transacciones = await _context.Transacciones
                    .Where(t => t.TarjetaId == tarjetaVinculada.TarjetaId) // Filtrar por TarjetaId
                    .Include(t => t.Categoria)
                    .Select(t => new TransaccionDto
                    {
                        TransaccionId = t.TransaccionId,
                        Descripcion = t.Descripcion,
                        Monto = t.Monto,
                        Fecha = t.Fecha,
                        Tipo = (int)t.Tipo,
                        CategoriaNombre = t.Categoria.Nombre ?? "Sin categoría",
                        BilleteraNombre = "Sin nombre"
                    })
                    .ToListAsync();

                return Ok(new { success = true, transacciones });
            }
            catch (Exception ex)
            {
                // Registrar el error en los logs
                _logger.LogError(ex, "Error en ObtenerTransacciones: {Message}\n{StackTrace}", ex.Message, ex.StackTrace);

                // Retornar un mensaje de error en formato JSON
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error interno del servidor",
                    error = ex.Message,
                    stackTrace = ex.StackTrace // Incluir el stack trace para depuración
                });
            }
        }
    }
}