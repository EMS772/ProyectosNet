using System.ComponentModel.DataAnnotations;

namespace MoneyMindIA.Models.Entidades
{
    public class Billetera
    {
        [Key]
        public int BilleteraId { get; set; }

        [Required]
        public string PayPalEmail { get; set; } // Correo de la cuenta PayPal vinculada

        [Required]
        public string PayPalAccessToken { get; set; } // Token de acceso para la API de PayPal

        public decimal BalanceActual { get; set; } // Balance actual de la billetera

        [Required]
        public DateTime FechaVinculacion { get; set; } = DateTime.UtcNow;

        // Relaciones
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        public ICollection<Transaccion> Transacciones { get; set; } = new List<Transaccion>();

        // Nueva relación con Tarjeta
        public ICollection<Tarjeta> Tarjetas { get; set; } = new List<Tarjeta>();
    }
}
