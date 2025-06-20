using System.ComponentModel.DataAnnotations;

namespace MoneyMindIA.Models.Entidades
{
    public class Tarjeta
    {
        [Key]
        public int TarjetaId { get; set; }

        [Required(ErrorMessage = "El nombre del titular es requerido")]
        [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
        public string NombreTitular { get; set; }

        [Required(ErrorMessage = "El número de tarjeta es requerido")]
        [CreditCard(ErrorMessage = "Número de tarjeta inválido")]
        public string NumeroTarjeta { get; set; }

        [Required(ErrorMessage = "La fecha de expiración es requerida")]
        [RegularExpression(@"^(0[1-9]|1[0-2])\/?([0-9]{2})$",
            ErrorMessage = "Formato MM/AA")]
        public string FechaExpiracion { get; set; }

        [Required(ErrorMessage = "El CVV es requerido")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Debe tener 3 dígitos")]
        public string CVV { get; set; }

        // Nuevo campo: Saldo inicial
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "El saldo no puede ser negativo")]
        public decimal Saldo { get; set; } = 5000.00m; // Saldo inicial de $1000

        public bool Activa { get; set; } // Nueva propiedad para representar el estado de la tarjeta

        // Relación con Billetera
        public int BilleteraId { get; set; }
        public Billetera Billetera { get; set; }
    }
}