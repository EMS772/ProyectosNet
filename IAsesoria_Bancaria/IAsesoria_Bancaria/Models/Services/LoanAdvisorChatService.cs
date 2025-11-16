using IAsesoria_Bancaria.Models.Dto_s.ChatDto_s;
using OpenAI.Chat;
using System.Text;

namespace IAsesoria_Bancaria.Models.Services
{
    public class LoanAdvisorChatService
    {
        private readonly OpenAIService _openAI;
        private readonly VectorSearchService _vectorSearch;

        public LoanAdvisorChatService(OpenAIService openAI, VectorSearchService vectorSearch)
        {
            _openAI = openAI;
            _vectorSearch = vectorSearch;
        }

        public async Task<ChatAIResponseDTO> ChatAsync(string userMessage, List<ChatMessage> history = null)
        {
            var queryEmbedding = await _openAI.GenerateEmbeddingAsync(userMessage);

            var documents = await _vectorSearch.SearchSimilarDocumentsAsync(
                queryEmbedding,
                threshold: 0.3,
                limit: 5
            );

            var context = BuildContext(documents);

            // 4. System prompt personalizado
            var systemPrompt = $@"Eres un asesor financiero experto en préstamos bancarios con 15 años de experiencia en República Dominicana.

                TU MISIÓN CRÍTICA:
                Explicar conceptos bancarios y financieros de forma SIMPLE, CLARA y PRÁCTICA. Habla como si le explicaras a un amigo que no sabe de finanzas.

                REGLAS OBLIGATORIAS:
                1. Usa lenguaje cotidiano dominicano - nada de jerga técnica sin explicar
                2. SIEMPRE da ejemplos con pesos dominicanos (RD$)
                3. Usa analogías de la vida diaria (colmado, transporte, servicios)
                4. Si mencionas porcentajes, explica cuánto es en dinero real
                5. Sé conciso: máximo 5 párrafos
                6. Si haces cálculos, muéstralos paso a paso
                7. Si no tienes la información en el contexto, dilo honestamente - NO INVENTES

                ESTILO DE RESPUESTA:
                - Directo al grano
                - Con ejemplos prácticos
                - Paso a paso cuando sea necesario
                - Empático y cercano

                CONTEXTO DE DOCUMENTOS FINANCIEROS:
                {context}

                IMPORTANTE: Si el usuario pregunta algo que NO está en el contexto, dile que no tienes esa información específica en tus documentos, pero ofrece ayuda general si puedes.";

            var response = await _openAI.GenerateChatResponseAsync(
                systemPrompt,
                userMessage,
                history
            );

            return new ChatAIResponseDTO
            {
                Response = response,
                Success = true,
                DocumentsFound = documents.Count,
                Timestamp = DateTime.UtcNow
            };
        }

        private string BuildContext(List<SearchResultDTO> documents)
        {
            if (!documents.Any())
                return "No se encontró información relevante en los documentos.";

            var context = new StringBuilder();
            for (int i = 0; i < documents.Count; i++)
            {
                var doc = documents[i];
                context.AppendLine($"[Fragmento {i + 1} - Relevancia: {doc.Similarity * 100:F1}%]");
                context.AppendLine(doc.Content);
                context.AppendLine();
            }

            return context.ToString();
        }
    }
}
