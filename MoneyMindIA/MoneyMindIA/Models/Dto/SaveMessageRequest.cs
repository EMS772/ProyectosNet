using System.ComponentModel.DataAnnotations;

namespace MoneyMindIA.Models.Dto
{
    public class SaveMessageRequest
    {

        [Required]
        public int ChatId { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public bool IsUser { get; set; }
    }
}
