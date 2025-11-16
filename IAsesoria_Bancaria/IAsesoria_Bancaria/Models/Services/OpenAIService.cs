using OpenAI.Chat;
using OpenAI.Embeddings;
using Pgvector;


namespace IAsesoria_Bancaria.Models.Services
{
    public class OpenAIService
    {
        private readonly string _apiKey;
        private readonly EmbeddingClient _embeddingClient;
        private readonly ChatClient _chatClient;

        public OpenAIService(IConfiguration config)
        {
            _apiKey = config["OpenAI:ApiKey"];
            _embeddingClient = new EmbeddingClient("text-embedding-3-small", _apiKey);
            _chatClient = new ChatClient("gpt-4o-mini", _apiKey);
        }

        public async Task<float[]> GenerateEmbeddingAsync(string text)
        {
            var response = await _embeddingClient.GenerateEmbeddingAsync(text);
            return response.Value.ToFloats().ToArray();
        }

        public async Task<string> GenerateChatResponseAsync(
            string systemPrompt,
            string userMessage,
            List<ChatMessage> history = null)
        {
            var messages = new List<ChatMessage>
            {
                new SystemChatMessage(systemPrompt)
            };

            if (history != null && history.Count > 0)
            {
                messages.AddRange(history);
            }

            messages.Add(new UserChatMessage(userMessage));

            var response = await _chatClient.CompleteChatAsync(messages);
            return response.Value.Content[0].Text;
        }
    }
}
