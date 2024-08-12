using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tienda_NetCore.Models.Enums;

namespace Tienda_NetCore.Models.Entidades
{
    public class Pedido
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UsuarioId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha del Pedido")]
        public DateTime FechaPedido { get; set; }

        [Required]
        public EstadoPedido Estado { get; set; }

        // Relación uno a muchos con PedidoItem
        public ICollection<PedidoItem> PedidoItems { get; set; } = new List<PedidoItem>();
    }
}