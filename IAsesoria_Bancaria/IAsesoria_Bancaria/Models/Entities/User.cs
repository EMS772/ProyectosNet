using Microsoft.VisualBasic;

namespace IAsesoria_Bancaria.Models.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navegación
        public List<Conversation> Conversations { get; set; }
    }
}
