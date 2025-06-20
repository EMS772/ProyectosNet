using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using MoneyMindIA.Models.Entidades;

namespace MoneyMindIA.Services
{
    public class DeepSeekService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly ILogger<DeepSeekService> _logger;

        public DeepSeekService(HttpClient httpClient, IConfiguration configuration, ILogger<DeepSeekService> logger)
        {
            _httpClient = httpClient;
            _apiKey = configuration["DeepSeek:ApiKey"] ?? throw new ArgumentNullException(nameof(configuration), "La API Key de DeepSeek no está configurada.");
            _logger = logger;
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
        }

        public async Task<string> GetFinancialAdviceAsync(Usuario usuario, string pregunta)
        {
            // Construir el prompt para DeepSeek
            var prompt = BuildPrompt(usuario, pregunta);

            // Usa el nombre correcto del modelo según la documentación de DeepSeek
            var requestBody = new
            {
                model = "deepseek-chat", // Verifica que este sea el modelo correcto
                messages = new[]
                {
                    new { role = "system", content = "Eres un experto en finanzas personales." },
                    new { role = "user", content = prompt }
                },
                max_tokens = 2000, // Aumentar significativamente el límite de tokens
                temperature = 0.7
            };

            try
            {
                // Configurar un tiempo de espera más largo si es necesario
                _httpClient.Timeout = TimeSpan.FromMinutes(2);

                _logger.LogInformation("Enviando solicitud a DeepSeek API");
                var response = await _httpClient.PostAsJsonAsync("https://api.deepseek.com/v1/chat/completions", requestBody);

                // Obtener el contenido completo para diagnóstico
                var responseContent = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"Respuesta recibida: {responseContent}");

                response.EnsureSuccessStatusCode();

                var responseData = JsonSerializer.Deserialize<JsonDocument>(responseContent);
                if (responseData == null)
                {
                    throw new Exception("No se pudo deserializar la respuesta");
                }

                var content = responseData.RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString();

                if (string.IsNullOrEmpty(content))
                {
                    throw new Exception("La respuesta de la API no contiene contenido.");
                }

                return content;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error en la solicitud a DeepSeek API");
                throw new Exception($"Error en la solicitud a DeepSeek API: {ex.Message}", ex);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Error al procesar la respuesta JSON");
                throw new Exception("Error al procesar la respuesta de la API", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error general en GetFinancialAdviceAsync");
                throw;
            }
        }

        private string BuildPrompt(Usuario usuario, string pregunta)
        {
            var sb = new StringBuilder();

            // Agregar el contexto inicial
            sb.AppendLine("Eres un experto en finanzas personales. A continuación, te proporciono información sobre un usuario:");
            sb.AppendLine($"- Nombre: {usuario.Nombre}");
            sb.AppendLine($"- Pregunta: {pregunta}");

            // Agregar transacciones si existen
            if (usuario.Transacciones.Any())
            {
                sb.AppendLine("Transacciones recientes:");
                foreach (var transaccion in usuario.Transacciones)
                {
                    sb.AppendLine($"- {transaccion.Categoria}: {transaccion.Monto} ({transaccion.Descripcion})");
                }
            }

            // Agregar metas si existen
            if (usuario.Metas.Any())
            {
                sb.AppendLine("Metas financieras:");
                foreach (var meta in usuario.Metas)
                {
                    sb.AppendLine($"- {meta.Descripcion}: {meta.MontoActual} / {meta.MontoObjetivo}");
                }
            }

            // Si no hay datos, agregar un mensaje
            if (!usuario.Transacciones.Any() && !usuario.Metas.Any())
            {
                sb.AppendLine("El usuario no ha ingresado transacciones ni metas.");
            }

            // Agregar instrucciones específicas según la pregunta
            switch (pregunta)
            {
                case "¿Cuáles son mis categorías de gasto más altas este mes?":
                    sb.AppendLine("Analiza las categorías de gasto y determina en cuáles ha gastado más dinero. Responde con un resumen de las categorías de gasto más altas y el monto total en cada una. No menciones las metas.");
                    break;

                case "¿Qué ajustes puedo hacer para alcanzar mi meta de ahorro más rápido?":
                    sb.AppendLine("Analiza sus gastos actuales y su progreso hacia las metas. Sugiere ajustes específicos que el usuario puede hacer para alcanzar sus metas de ahorro más rápido.");
                    break;

                case "¿Qué gastos puedo reducir sin afectar mi calidad de vida?":
                    sb.AppendLine("Identifica los gastos que pueden ser reducidos o eliminados sin afectar significativamente su calidad de vida. Proporciona recomendaciones concretas y justifica por qué estos gastos son prescindibles. No menciones las metas.");
                    break;

                default:
                    sb.AppendLine("Responde la pregunta del usuario basándote en sus datos financieros.");
                    break;
            }

            return sb.ToString();
        }
    }
}