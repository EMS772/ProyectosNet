using System.ComponentModel.DataAnnotations;

namespace MoneyMindIA.Models.Entidades
{
    public class Meta
    {
        [Key]
        public int MetaId { get; set; } // Cambiado de MetaAhorroId a MetaId

        [Required]
        [StringLength(200)]
        public string Descripcion { get; set; } // Ejemplo: "Ahorrar para un viaje"

        [Required]
        public decimal MontoObjetivo { get; set; } // Cantidad que el usuario desea ahorrar

        public decimal MontoActual { get; set; } = 0; // Cantidad ahorrada hasta el momento

        [Required]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public DateTime? FechaCumplimiento { get; set; } // Fecha límite para alcanzar la meta

        // Relaciones
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
    }
}
