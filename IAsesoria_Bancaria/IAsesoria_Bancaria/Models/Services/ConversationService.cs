using IAsesoria_Bancaria.Models.Data;
using IAsesoria_Bancaria.Models.Dto_s.Conversation;
using IAsesoria_Bancaria.Models.Entities;
using IAsesoria_Bancaria.Models.Interfaces;

namespace IAsesoria_Bancaria.Models.Services
{
    public class ConversationService : IConversationService
    {
        private readonly IAB_DbContext _context;
        public ConversationService( IAB_DbContext context)
        {
            _context = context;
        }

        public ConversationDTO CreateConversation(ConversationCupDTO conversation)
        {
            var con = new Conversation
            {
                UserId = conversation.UserId,
                Title = conversation.Title,
                CreatedAt = DateTime.UtcNow
            };

            _context.Conversations.Add(con);
            _context.SaveChanges();

            return new ConversationDTO
            {
                Id = con.Id,
                UserId = con.UserId,
                Title = con.Title,
                CreatedAt = con.CreatedAt,
                UpdatedAt = con.UpdatedAt
            };
        }

        public void DeleteConversation(int conversationId)
        {
            var conver= _context.Conversations.FirstOrDefault(c => c.Id == conversationId);

            if (conver == null)
            {
                throw new Exception("Conversation not found");
            }   

            _context.Conversations.Remove(conver);
            _context.SaveChanges();

        }

        public ConversationDTO GetConversationByID(int conversationId)
        {
            var conver = _context.Conversations.FirstOrDefault(c => c.Id == conversationId);

            if (conver == null)
            {
                throw new Exception("Conversation not found");
            }

            return new ConversationDTO
            {
                Id = conver.Id,
                UserId = conver.UserId,
                Title = conver.Title,
                CreatedAt = conver.CreatedAt,
                UpdatedAt = conver.UpdatedAt
            };
        }

        public async Task UpdateConversationTimestampAsync(int conversationId)
        {
            var conversation = await _context.Conversations.FindAsync(conversationId);
            if (conversation != null)
            {
                conversation.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public List<ConversationDTO> GetConversations()
        {
            var conversations = _context.Conversations.Select(c => new ConversationDTO
            {
                Id = c.Id,
                UserId = c.UserId,
                Title = c.Title,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            }).ToList();

            return conversations;
        }
        public List<ConversationDTO> GetConversationsByUserId(int userId)
        {
            var conversations = _context.Conversations
                .Where(c => c.UserId == userId)  
                .OrderByDescending(c => c.UpdatedAt != default(DateTime) ? c.UpdatedAt : c.CreatedAt)
                .ToList();

            return conversations.Select(c => new ConversationDTO
            {
                Id = c.Id,
                UserId = c.UserId,
                Title = c.Title,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            }).ToList();
        }

        public ConversationCupDTO UpdateConversationTitle(int conversationId, ConversationCupDTO cTitulo)
        {
            var conversation = _context.Conversations.FirstOrDefault(c => c.Id == conversationId);
            if (conversation == null)
            {
                throw new Exception("Conversation not found");
            }
            conversation.Title = cTitulo.Title;
            conversation.UpdatedAt = DateTime.UtcNow;
            _context.SaveChanges();

            return new ConversationCupDTO
            {
                UserId = conversation.UserId,
                Title = conversation.Title
            };

        }
    }
}
