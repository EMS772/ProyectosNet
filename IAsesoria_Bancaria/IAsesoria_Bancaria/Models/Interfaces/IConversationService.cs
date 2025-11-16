using IAsesoria_Bancaria.Models.Dto_s.Conversation;

namespace IAsesoria_Bancaria.Models.Interfaces
{
    public interface IConversationService
    {
        public List<ConversationDTO> GetConversations();
        List<ConversationDTO> GetConversationsByUserId(int userId);
        public ConversationDTO GetConversationByID(int conversationId);
        public ConversationDTO CreateConversation(ConversationCupDTO conversation);
        public Task UpdateConversationTimestampAsync(int conversationId);
        public ConversationCupDTO UpdateConversationTitle(int conversationId, ConversationCupDTO cTitulo);
        public void DeleteConversation(int conversationId);
    }
}
