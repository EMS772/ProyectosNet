using IAsesoria_Bancaria.Models.Dto_s.Conversation;
using IAsesoria_Bancaria.Models.Dto_s.ConversationDto_s;
using IAsesoria_Bancaria.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IAsesoria_Bancaria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConversationController : ControllerBase
    {
        private readonly IConversationService _conversationService;
        public ConversationController(IConversationService conversationService)
        {
            _conversationService = conversationService;
        }

        [HttpPost]
        [Route("GetConversationsByUser")]
        [Authorize]
        public IActionResult GetConversationsByUser([FromBody] GetConversationsByUserDTO dto)
        {
            try
            {
                var result = _conversationService.GetConversationsByUserId(dto.UserId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet]
        [Route("Conversation/{id}")]
        public IActionResult GetConversationById([FromBody] int id)
        {
            try
            {
                var result = _conversationService.GetConversationByID(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

       

        [HttpPost]
        [Route("CreateConversation")]
        public IActionResult CreateConversation([FromBody] ConversationCupDTO conversationDTO)
        {
            try
            {
                var result = _conversationService.CreateConversation(conversationDTO);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete]
        [Route("DeleteConversation/{id}")]
        public IActionResult DeleteConversation([FromBody] int id)
        {
            try
            {
                _conversationService.DeleteConversation(id);
                return Ok(new { message = "Conversation deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut]
        [Route("UpdateConversationTimestamp")]

        public IActionResult UpdateConversationTimestamp([FromBody] int conversationId)
        {
            try
            {
                var result = _conversationService.UpdateConversationTimestampAsync(conversationId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut]
        [Route("UpdateConversationTitle/{id}")]
        public IActionResult UpdateConversationTitle( int id, [FromBody] ConversationCupDTO cTitulo)
        {
            try
            {
                var result = _conversationService.UpdateConversationTitle(id, cTitulo);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
