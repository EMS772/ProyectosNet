using Microsoft.AspNetCore.Mvc;
using MoneyMindIA.Models.Data;
using MoneyMindIA.Models.Entidades;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using MoneyMindIA.Models;

namespace MoneyMindIA.Controllers
{
    [Authorize]
    public class MetaController : Controller
    {
        private readonly MoneyMindDbContext _context;
        private readonly ILogger<MetaController> _logger;

        public MetaController(MoneyMindDbContext context, ILogger<MetaController> logger)
        {
            _context = context;
            _logger = logger;

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearMeta([FromBody] MetaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Datos inválidos", errors = ModelState.Values.SelectMany(v => v.Errors) });
            }

            try
            {
                // Obtener el ID del usuario autenticado
                var usuarioId = int.Parse(User.FindFirstValue("UsuarioId"));

                // Crear la meta
                var meta = new Meta
                {
                    Descripcion = model.Descripcion,
                    MontoObjetivo = model.MontoObjetivo,
                    MontoActual = model.MontoActual,
                    FechaCreacion = DateTime.UtcNow,
                    FechaCumplimiento = model.FechaCumplimiento,
                    UsuarioId = usuarioId
                };

                // Guardar la meta en la base de datos
                _context.Metas.Add(meta);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Meta creada exitosamente", metaId = meta.MetaId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la meta");
                return StatusCode(500, new { success = false, message = "Error interno del servidor", error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerMetas()
        {
            try
            {
                var usuarioId = int.Parse(User.FindFirstValue("UsuarioId"));

                var metas = await _context.Metas
                    .Where(m => m.UsuarioId == usuarioId)
                    .Select(m => new
                    {
                        m.MetaId,
                        m.Descripcion,
                        m.MontoObjetivo,
                        m.MontoActual,
                        FechaCumplimiento = m.FechaCumplimiento.HasValue ? m.FechaCumplimiento.Value.ToString("yyyy-MM-dd") : null,
                        Progreso = (m.MontoActual / m.MontoObjetivo) * 100,
                        Completada = m.MontoActual >= m.MontoObjetivo
                    })
                    .ToListAsync();

                return Ok(new { success = true, metas });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error interno del servidor", error = ex.Message });
            }
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarMeta(int id)
        {
            try
            {
                // Obtener el ID del usuario autenticado
                var usuarioId = int.Parse(User.FindFirstValue("UsuarioId"));

                // Buscar la meta por ID y asegurarse que pertenezca al usuario actual
                var meta = await _context.Metas
                    .FirstOrDefaultAsync(m => m.MetaId == id && m.UsuarioId == usuarioId);

                if (meta == null)
                {
                    return NotFound(new { success = false, message = "Meta no encontrada" });
                }

                // Eliminar la meta de la base de datos
                _context.Metas.Remove(meta);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Meta eliminada exitosamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la meta");
                return StatusCode(500, new { success = false, message = "Error interno del servidor", error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerMetaPorId(int id)
        {
            try
            {
                var usuarioId = int.Parse(User.FindFirstValue("UsuarioId"));

                var meta = await _context.Metas
                    .Where(m => m.MetaId == id && m.UsuarioId == usuarioId)
                    .Select(m => new
                    {
                        m.MetaId,
                        m.Descripcion,
                        m.MontoObjetivo,
                        m.MontoActual,
                        m.FechaCumplimiento
                    })
                    .FirstOrDefaultAsync();

                if (meta == null)
                {
                    return NotFound(new { success = false, message = "Meta no encontrada" });
                }

                return Ok(new { success = true, meta });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la meta por ID");
                return StatusCode(500, new { success = false, message = "Error interno del servidor", error = ex.Message });
            }
        }

        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActualizarMeta([FromBody] MetaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Datos inválidos", errors = ModelState.Values.SelectMany(v => v.Errors) });
            }

            try
            {
                var usuarioId = int.Parse(User.FindFirstValue("UsuarioId"));

                var meta = await _context.Metas
                    .FirstOrDefaultAsync(m => m.MetaId == model.MetaId && m.UsuarioId == usuarioId);

                if (meta == null)
                {
                    return NotFound(new { success = false, message = "Meta no encontrada" });
                }

                // Actualizar los campos de la meta
                meta.Descripcion = model.Descripcion;
                meta.MontoObjetivo = model.MontoObjetivo;
                meta.MontoActual = model.MontoActual;
                meta.FechaCumplimiento = model.FechaCumplimiento;

                _context.Metas.Update(meta);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Meta actualizada exitosamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la meta");
                return StatusCode(500, new { success = false, message = "Error interno del servidor", error = ex.Message });
            }
        }
    }
}
