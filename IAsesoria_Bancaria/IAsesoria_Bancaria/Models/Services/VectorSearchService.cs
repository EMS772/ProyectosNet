using IAsesoria_Bancaria.Models.Dto_s.ChatDto_s;
using Microsoft.OpenApi.Services;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace IAsesoria_Bancaria.Models.Services
{
    public class VectorSearchService
    {
        private readonly string _supabaseUrl;
        private readonly string _supabaseKey;
        private readonly HttpClient _httpClient;

        public VectorSearchService(IConfiguration config)
        {
            _supabaseUrl = config["Supabase:Url"];
            _supabaseKey = config["Supabase:ServiceRoleKey"];
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("apikey", _supabaseKey);
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _supabaseKey);
        }

        public async Task<List<SearchResultDTO>> SearchSimilarDocumentsAsync(
            float[] queryEmbedding,
            double threshold = 0.3,
            int limit = 5)
        {
            var requestBody = new
            {
                query_embedding = queryEmbedding,
                match_threshold = threshold,
                match_count = limit
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(
                $"{_supabaseUrl}/rest/v1/rpc/match_documents",
                content
            );

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Supabase error: {error}");
            }

            var resultJson = await response.Content.ReadAsStringAsync();
            var results = JsonSerializer.Deserialize<List<SearchResultDTO>>(
                resultJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            return results ?? new List<SearchResultDTO>();
        }
    }
}