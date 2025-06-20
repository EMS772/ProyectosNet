using MoneyMindIA.Models.Enum;
using System;
using System.ComponentModel.DataAnnotations;

namespace MoneyMindIA.Models.ViewModels
{
    public class TransaccionViewModel
    {
        [Required(ErrorMessage = "El monto es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor que 0")]
        public decimal Monto { get; set; }

        [Required(ErrorMessage = "La fecha es requerida")]
        public DateTime Fecha { get; set; }

        [StringLength(200, ErrorMessage = "La descripción no puede exceder los 200 caracteres")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El tipo de transacción es requerido")]
        public TipoTransaccion Tipo { get; set; } // Enum: Ingreso o Gasto

        [Required(ErrorMessage = "La categoría es requerida")]
        public int CategoriaId { get; set; }
    }
}