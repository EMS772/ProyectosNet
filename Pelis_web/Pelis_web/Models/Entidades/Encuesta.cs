using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Pelis_web.Models.Entidades
{
    [Table("Encuestas")]
    public class Encuesta
    {
        [Key]
        public int Id { get; set; }

        public List<Pregunta> Preguntas { get; set; }
    }
}
