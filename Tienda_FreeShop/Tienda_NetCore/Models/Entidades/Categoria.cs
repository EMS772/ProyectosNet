using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tienda_NetCore.Models.Entidades
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la categoría es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        [Display(Name = "Nombre de la Categoría")]
        public string Nombre { get; set; }

        // Relación uno a muchos con Producto
        public ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}