using System.ComponentModel.DataAnnotations;

namespace Tienda_NetCore.Models.Entidades
{
    public class CarritoItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProductoId { get; set; }
        public Producto Producto { get; set; }

        [Required]
        public int CarritoId { get; set; }
        public Carrito Carrito { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1.")]
        [Display(Name = "Cantidad")]
        public int Cantidad { get; set; }


    }
}
