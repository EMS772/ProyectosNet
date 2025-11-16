namespace IAsesoria_Bancaria.Models.Dto_s.Conversation
{
    public class ConversationDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
