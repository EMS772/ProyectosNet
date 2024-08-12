using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tienda_NetCore.Models.Entidades
{
    public class Carrito
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UsuarioId { get; set; }

        // Relación uno a muchos con CarritoItem
        public List<CarritoItem> CarritoItems { get; set; } = new List<CarritoItem>();
        public Usuario Usuario { get; set; }


    }
}