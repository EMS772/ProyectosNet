using System.ComponentModel.DataAnnotations;
using MoneyMindIA.Models.Enum;

namespace MoneyMindIA.Models.Entidades
{
    public class Transaccion
    {
        [Key]
        public int TransaccionId { get; set; }

        [Required]
        public decimal Monto { get; set; } // Positivo (ingreso) o negativo (gasto)

        [Required]
        public DateTime Fecha { get; set; }

        [StringLength(200)]
        public string Descripcion { get; set; }

        [Required]
        public TipoTransaccion Tipo { get; set; } // Ingreso o Gasto

        // Relaciones
        public int BilleteraId { get; set; }
        public Billetera Billetera { get; set; }

        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }

        // Nueva relación con Tarjeta
        public int? TarjetaId { get; set; } // Puede ser nulo si no está asociada a una tarjeta
        public Tarjeta Tarjeta { get; set; }
        // Nueva propiedad para la relación con Usuario
        public int UsuarioId { get; set; } // Asegúrate de que esta propiedad exista
        public Usuario Usuario { get; set; } // Relación con el usuario
    }
}