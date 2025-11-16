namespace IAsesoria_Bancaria.Models.Dto_s.ChatDto_s
{
    public class ChatAIResponseDTO
    {
        public string Response { get; set; }
        public bool Success { get; set; }
        public int DocumentsFound { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
