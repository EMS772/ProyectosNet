namespace IAsesoria_Bancaria.Models.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public int ConversationId { get; set; }
        public string Role { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }

        // Navegación
        public Conversation Conversation { get; set; }
    }
}
