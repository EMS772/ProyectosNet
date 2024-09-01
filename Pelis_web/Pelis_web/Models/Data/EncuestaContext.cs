using Microsoft.EntityFrameworkCore;
using Pelis_web.Models.Entidades;

namespace Pelis_web.Models.Data
{
    public class EncuestaContext : DbContext
    {
        public EncuestaContext(DbContextOptions<EncuestaContext> options)
            : base(options)
        {
        }

        public DbSet<Pregunta> Preguntas { get; set; }
        public DbSet<Opcion> Opciones { get; set; }
        public DbSet<Encuesta> Encuestas { get; set; }
        public DbSet<Pelicula> Peliculas { get; set; }
    }
}
