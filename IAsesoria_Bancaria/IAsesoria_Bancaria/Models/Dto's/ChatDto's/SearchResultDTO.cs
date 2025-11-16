using System.Text.Json;

namespace IAsesoria_Bancaria.Models.Dto_s.ChatDto_s
{
    public class SearchResultDTO
    {
        public long Id { get; set; }
        public string Content { get; set; }
        public JsonElement? Metadata { get; set; } 
        public double Similarity { get; set; }
    }
}
