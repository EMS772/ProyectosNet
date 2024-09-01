using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Pelis_web.Models.Entidades
{
    [Table("Opciones")]
    public class Opcion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string TextoOpcion { get; set; }

        public int PreguntaId { get; set; }

        [ForeignKey("PreguntaId")]
        public Pregunta Pregunta { get; set; }
    }

}
