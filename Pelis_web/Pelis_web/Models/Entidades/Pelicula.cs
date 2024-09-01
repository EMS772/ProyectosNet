using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Pelis_web.Models.Entidades
{
    [Table("Peliculas")]
    public class Pelicula
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Titulo { get; set; }

        [Required]
        [StringLength(100)]
        public string Genero { get; set; }


        [Required]
        [StringLength(100)]
        public string TipoHistoria { get; set; }

        [Required]
        [StringLength(100)]
        public string Epoca { get; set; }


        [Required]
        [StringLength(100)]
        public string TipoFinal { get; set; }

        [Required]
        [StringLength(100)]
        public string TipoAmbientacion { get; set; }

        [Required]
        [StringLength(100)]
        public string TipoTrama { get; set; }

        [Required]
        [StringLength(100)]
        public string EstructuraNarrativa { get; set; }

        [Required]
        [StringLength(100)]
        public string TipoMusica { get; set; }

        [Required]
        [StringLength(100)]
        public string TipoRitmo { get; set; }

        [StringLength(255)]
        public string ImagenUrl { get; set; }

        [StringLength(1000)]
        public string Descripcion { get; set; } 

        [Range(0, 10)]
        public double? CalificacionImdb { get; set; } 
    }
}
