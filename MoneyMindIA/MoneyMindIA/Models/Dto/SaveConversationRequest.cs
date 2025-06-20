using System.ComponentModel.DataAnnotations;

namespace MoneyMindIA.Models.Dto
{
    public class SaveConversationRequest
    {
        [Required]
        public List<MessageData> Messages { get; set; }
    }
}
