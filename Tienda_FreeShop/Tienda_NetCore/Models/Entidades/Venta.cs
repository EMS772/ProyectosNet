using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tienda_NetCore.Models.Entidades
{
    public class Venta
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UsuarioId { get; set; } // ID del usuario que realizó la compra

        public Usuario Usuario { get; set; } // Relación con el usuario

        [Required]
        public DateTime Fecha { get; set; } // Fecha de la venta

        [Required]
        public int Cantidad { get; set; } // Cantidad total de ítems comprados

        [Required]
        [Column(TypeName = "decimal(18, 2)")] // Asegúrate de usar decimal con precisión adecuada
        public decimal Total { get; set; } // Total de la venta

    }
}
