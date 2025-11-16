using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenAI.Chat;
using IAsesoria_Bancaria.Models.Dto_s.ChatDto_s;
using IAsesoria_Bancaria.Models.Services;
using IAsesoria_Bancaria.Models.Interfaces;

namespace IAsesoria_Bancaria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly LoanAdvisorChatService _chatService;

        public MessageController(
            Models.Interfaces.IMessageService messageService,
            LoanAdvisorChatService chatService)
        {
            _messageService = messageService;
            _chatService = chatService;
        }

        [HttpPost]
        [Route("AddMessage")]
        public async Task<IActionResult> AddMessage([FromBody] Models.Dto_s.MessageDto_s.MessageDTO messageDTO)
        {
            try
            {
                var createdMessage = await _messageService.AddMessageAsync(messageDTO);
                return Ok(createdMessage);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetMessagesByConversation/{conversationId}")]
        public async Task<IActionResult> GetMessagesByConversation(int conversationId)
        {
            try
            {
                var messages = await _messageService.GetMessagesByConversationIdAsync(conversationId);
                return Ok(messages);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("ChatWithAI")]
        public async Task<IActionResult> ChatWithAI([FromBody] ChatRequestDTO request)
        {
            try
            {
                var historyMessages = await _messageService.GetMessagesByConversationIdAsync(request.ConversationId);

                var history = historyMessages.Select(m =>
                    m.Role == "user"
                        ? new UserChatMessage(m.Content) as ChatMessage
                        : new AssistantChatMessage(m.Content) as ChatMessage
                ).ToList();

                var aiResponse = await _chatService.ChatAsync(request.UserMessage, history);

                await _messageService.AddMessageAsync(new Models.Dto_s.MessageDto_s.MessageDTO
                {
                    ConversationId = request.ConversationId,
                    Role = "user",
                    Content = request.UserMessage,
                    Timestamp = DateTime.UtcNow
                });

                await _messageService.AddMessageAsync(new Models.Dto_s.MessageDto_s.MessageDTO
                {
                    ConversationId = request.ConversationId,
                    Role = "assistant",
                    Content = aiResponse.Response,
                    Timestamp = DateTime.UtcNow
                });

                return Ok(aiResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}