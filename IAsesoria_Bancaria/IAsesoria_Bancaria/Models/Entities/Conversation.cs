namespace IAsesoria_Bancaria.Models.Entities
{
    public class Conversation
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navegación
        public User User { get; set; }
        public List<Message> Messages { get; set; }
    }
}
