using System.ComponentModel.DataAnnotations;

namespace MoneyMindIA.Models.Entidades
{
    public class Categoria
    {
        [Key]
        public int? CategoriaId { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } // Ejemplo: "Alimentación", "Transporte"

        // Relaciones
        public ICollection<Transaccion> Transacciones { get; set; } = new List<Transaccion>();
    }
}
