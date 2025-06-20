using System;
using System.ComponentModel.DataAnnotations;

namespace MoneyMindIA.Models
{
    public class MetaViewModel
    {
        public int MetaId { get; set; }  // Agrega MetaId para identificar cada meta de manera única.

        [Required(ErrorMessage = "La descripción de la meta es obligatoria.")]
        [StringLength(200, ErrorMessage = "La descripción no puede exceder los 200 caracteres.")]
        public string Descripcion { get; set; } // Asegúrate de que este campo esté correctamente definido

        [Required(ErrorMessage = "El monto objetivo es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto objetivo debe ser mayor que 0.")]
        public decimal MontoObjetivo { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "El monto actual no puede ser negativo.")]
        public decimal MontoActual { get; set; } = 0;

        [Required(ErrorMessage = "La fecha de cumplimiento es obligatoria.")]
        [DataType(DataType.Date, ErrorMessage = "Formato de fecha inválido.")]
        public DateTime FechaCumplimiento { get; set; }
    }
}