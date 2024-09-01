using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Pelis_web.Models.Entidades
{
    [Table("Preguntas")]
    public class Pregunta
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        public string TextoPregunta { get; set; }

        public List<Opcion> Opciones { get; set; }
    }
}
