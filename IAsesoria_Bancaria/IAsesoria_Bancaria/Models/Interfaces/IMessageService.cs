using IAsesoria_Bancaria.Models.Dto_s.MessageDto_s;
using IAsesoria_Bancaria.Models.Entities;

namespace IAsesoria_Bancaria.Models.Interfaces
{
    public  interface IMessageService
    {
        Task<MessageDTO> AddMessageAsync(MessageDTO message);
        Task<List<MessageDTO>> GetMessagesByConversationIdAsync(int conversationId);
    }
}
