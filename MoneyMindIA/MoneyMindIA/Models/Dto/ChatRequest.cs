using System.ComponentModel.DataAnnotations;

namespace MoneyMindIA.Models.Dto
{
    public class ChatRequest
    {
        [Required]
        public string Pregunta { get; set; }
        [Required] public int ChatId { get; set; }

    }
}
