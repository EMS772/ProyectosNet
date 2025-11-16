using IAsesoria_Bancaria.Models.Data;
using IAsesoria_Bancaria.Models.Dto_s.MessageDto_s;
using IAsesoria_Bancaria.Models.Entities;
using IAsesoria_Bancaria.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.Json;

namespace IAsesoria_Bancaria.Models.Services
{
    public class MessageService : IMessageService
    {
        private readonly IAB_DbContext _context;
        public MessageService(IAB_DbContext context)
        {
            _context = context;
        }

        public async Task<MessageDTO> AddMessageAsync(MessageDTO messageDTO)
        {
            var message= new Message
            {
                ConversationId = messageDTO.ConversationId,
                Role = messageDTO.Role,
                Content = messageDTO.Content,
                Timestamp = DateTime.UtcNow
            };

            _context.Messages.Add(message);
            _context.SaveChanges();


            return new MessageDTO
            {
                Id = message.Id,
                ConversationId = message.ConversationId,
                Role = message.Role,
                Content = message.Content,
                Timestamp = message.Timestamp
            };
        }

        public async Task<List<MessageDTO>> GetMessagesByConversationIdAsync(int conversationId)
        {
            var messages = await _context.Messages
                .Where(m => m.ConversationId == conversationId)
                .Select(m => new MessageDTO
                {
                    Id = m.Id,
                    ConversationId = m.ConversationId,
                    Role = m.Role,
                    Content = m.Content,
                    Timestamp = m.Timestamp
                })
                .ToListAsync();

            return messages;

        }
    }
}
