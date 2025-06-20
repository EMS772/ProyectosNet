using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoneyMindIA.Models.Data;
using MoneyMindIA.Models.Dto;
using MoneyMindIA.Models.Entidades;
using MoneyMindIA.Services;
using System.Security.Claims;

namespace MoneyMindIA.Controllers
{
    [Authorize] // Asegúrate que el controlador o acciones requieran autenticación

    public class FinancialAdviceController : Controller
    {
        private readonly DeepSeekService _deepSeekService;
        private readonly MoneyMindDbContext _context;
        private readonly ILogger<FinancialAdviceController> _logger;

        public FinancialAdviceController(DeepSeekService deepSeekService, MoneyMindDbContext context, ILogger<FinancialAdviceController> logger)
        {
            _deepSeekService = deepSeekService;
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult AskQuestion()
        {
            // Mostrar las preguntas predeterminadas
            var preguntas = new List<string>
            {
                "¿Cuáles son mis categorías de gasto más altas este mes?",
                "¿Qué ajustes puedo hacer para alcanzar mi meta de ahorro más rápido?",
                "¿Qué gastos puedo reducir sin afectar mi calidad de vida?"
            };
            return View(preguntas);
        }

        [HttpGet]
        public IActionResult GetRecomendationHistory()
        {
            try
            {
                var usuarioId = int.Parse(User.FindFirstValue("UsuarioId"));
                var recomendaciones = _context.Recomendaciones
                    .Where(r => r.UsuarioId == usuarioId)
                    .OrderByDescending(r => r.FechaGeneracion)
                    .Select(r => new {
                        recomendacionId = r.RecomendacionId,
                        preview = r.Mensaje,
                        fechaGeneracion = r.FechaGeneracion.ToString("yyyy-MM-dd HH:mm")
                    })
                    .ToList();

                return Json(new
                {
                    success = true,
                    recomendations = recomendaciones // Corregido: usar el nombre correcto de la variable
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener historial");
                return Json(new { success = false, message = "Error al cargar el historial" });
            }
        }

        [HttpGet]
        public IActionResult GetRecomendationMessages(int recomendacionId)
        {
            try
            {
                var usuarioId = int.Parse(User.FindFirstValue("UsuarioId"));
                var mensajes = _context.ChatMensajes
                    .Where(m => m.RecomendacionId == recomendacionId && m.UsuarioId == usuarioId)
                    .OrderBy(m => m.FechaEnvio)
                    .Select(m => new {
                        contenido = m.Contenido,
                        esUsuario = m.EsUsuario,
                        fechaEnvio = m.FechaEnvio
                    })
                    .ToList();

                return Json(new { success = true, messages = mensajes });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener mensajes de recomendación");
                return Json(new { success = false, message = "Error al cargar la conversación" });
            }
        }

        [HttpPost]
        public IActionResult StartNewRecomendation()
        {
            try
            {
                var usuarioId = int.Parse(User.FindFirstValue("UsuarioId"));
                var nuevaRecomendacion = new Recomendacion
                {
                    UsuarioId = usuarioId,
                    Mensaje = $"Nuevo chat - {DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm")}", // Mensaje inicial claro
                    FechaGeneracion = DateTime.UtcNow
                };

                _context.Recomendaciones.Add(nuevaRecomendacion);
                _context.SaveChanges();

                _logger.LogInformation($"Nueva recomendación creada: ID {nuevaRecomendacion.RecomendacionId}");

                return Json(new
                {
                    success = true,
                    recomendacionId = nuevaRecomendacion.RecomendacionId,
                    message = nuevaRecomendacion.Mensaje // Para debug
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nueva recomendación");
                return Json(new
                {
                    success = false,
                    message = "Error al iniciar nueva conversación",
                    error = ex.Message
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetAdvice([FromBody] ChatRequest request)
        {
            try
            {
                if (request == null || string.IsNullOrWhiteSpace(request.Pregunta))
                {
                    return Json(new { success = false, message = "La pregunta no puede estar vacía" });
                }

                var usuarioId = int.Parse(User.FindFirstValue("UsuarioId"));
                var usuario = ObtenerUsuarioActual();

                // Guardar mensaje del usuario (versión corregida)
                await SaveMessage(new SaveMessageRequest
                {
                    ChatId = request.ChatId,
                    Content = request.Pregunta,
                    IsUser = true
                });

                // Obtener respuesta de la IA
                var respuesta = await _deepSeekService.GetFinancialAdviceAsync(usuario, request.Pregunta);

                if (string.IsNullOrWhiteSpace(respuesta))
                {
                    return Json(new { success = false, message = "No se recibió respuesta de la IA" });
                }

                // Guardar respuesta de la IA (versión corregida)
                await SaveMessage(new SaveMessageRequest
                {
                    ChatId = request.ChatId,
                    Content = respuesta,
                    IsUser = false
                });

                // Actualizar título de la recomendación con la primera pregunta
                var recomendacion = await _context.Recomendaciones
                    .FirstOrDefaultAsync(r => r.RecomendacionId == request.ChatId);

                if (recomendacion != null && recomendacion.Mensaje == "Nueva conversación")
                {
                    recomendacion.Mensaje = $"Chat del {DateTime.Now:dd/MM/yyyy} - {request.Pregunta.Truncate(50)}";
                    await _context.SaveChangesAsync();
                }

                return Json(new
                {
                    success = true,
                    message = respuesta,
                    chatId = request.ChatId
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GetAdvice");
                return Json(new { success = false, message = ex.Message });
            }
        }
        [HttpPost]
        public async Task<IActionResult> SaveMessage([FromBody] SaveMessageRequest request)
        {
            try
            {
                var usuarioId = int.Parse(User.FindFirstValue("UsuarioId"));
                var message = new ChatMensaje
                {
                    RecomendacionId = request.ChatId,
                    Contenido = request.Content,
                    EsUsuario = request.IsUser,
                    FechaEnvio = DateTime.UtcNow,
                    UsuarioId = usuarioId
                };
                _context.ChatMensajes.Add(message);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar mensaje");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveConversation([FromBody] SaveConversationRequest request)
        {
            try
            {
                // Validación adicional
                if (request?.Messages == null || !request.Messages.Any())
                {
                    _logger.LogWarning("Intento de guardar conversación vacía");
                    return Json(new { success = false, message = "No hay mensajes para guardar" });
                }

                var usuarioId = int.Parse(User.FindFirstValue("UsuarioId"));

                // Crear título descriptivo
                var primeraPregunta = request.Messages.FirstOrDefault(m => m.IsUser)?.Content ?? "Consulta";
                var titulo = $"{DateTime.Now:dd/MM/yyyy HH:mm} - {primeraPregunta.Truncate(50)}";

                // Crear nueva recomendación en una transacción
                using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    var recomendacion = new Recomendacion
                    {
                        UsuarioId = usuarioId,
                        Mensaje = titulo,
                        FechaGeneracion = DateTime.UtcNow
                    };

                    _context.Recomendaciones.Add(recomendacion);
                    await _context.SaveChangesAsync();

                    // Guardar mensajes
                    foreach (var msg in request.Messages)
                    {
                        var chatMsg = new ChatMensaje
                        {
                            RecomendacionId = recomendacion.RecomendacionId,
                            Contenido = msg.Content,
                            EsUsuario = msg.IsUser,
                            FechaEnvio = DateTime.UtcNow,
                            UsuarioId = usuarioId
                        };
                        _context.ChatMensajes.Add(chatMsg);
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    _logger.LogInformation($"Conversación guardada. ID: {recomendacion.RecomendacionId}");

                    return Json(new
                    {
                        success = true,
                        recomendacionId = recomendacion.RecomendacionId
                    });
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(ex, "Error en transacción al guardar conversación");
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar conversación completa");
                return Json(new
                {
                    success = false,
                    message = "Error interno al guardar la conversación"
                });
            }
        }

        private Usuario ObtenerUsuarioActual()
        {
            // Obtener el ID del usuario autenticado
            var usuarioId = int.Parse(User.FindFirstValue("UsuarioId"));

            // Obtener el usuario desde la base de datos, incluyendo sus transacciones y metas
            var usuario = _context.Usuarios
                .Include(u => u.Transacciones) // Incluir las transacciones
                            .ThenInclude(t => t.Categoria) // Incluir la categoría de cada transacción
                .Include(u => u.Metas) // Incluir las metas
                .FirstOrDefault(u => u.UsuarioId == usuarioId);

            if (usuario == null)
            {
                throw new Exception("Usuario no encontrado.");
            }

            return usuario;
        }
    }
}