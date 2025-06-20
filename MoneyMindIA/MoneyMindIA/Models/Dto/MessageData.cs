using System.ComponentModel.DataAnnotations;

namespace MoneyMindIA.Models.Dto
{
    public class MessageData
    {
        [Required]
        public bool IsUser { get; set; }
        [Required]
        public string Content { get; set; }
    }
}
