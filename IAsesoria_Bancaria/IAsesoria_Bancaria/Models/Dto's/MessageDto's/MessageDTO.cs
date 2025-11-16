namespace IAsesoria_Bancaria.Models.Dto_s.MessageDto_s
{
    public class MessageDTO
    {
        public int Id { get; set; }
        public int ConversationId { get; set; }
        public string Role { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
