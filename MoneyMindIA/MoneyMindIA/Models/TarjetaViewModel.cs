using System.ComponentModel.DataAnnotations;

namespace MoneyMindIA.Models
{
    public class TarjetaViewModel
    {
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
    }
}
