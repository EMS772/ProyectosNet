using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MoneyMindIA.Models.Entidades
{
    public class Usuario
    {
        [Key]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "Debe ser un email válido")]
        [Remote(action: "VerifyEmail", controller: "Auth", ErrorMessage = "El email ya está registrado")]
        public string Email { get; set; }

        public string? GoogleId { get; set; } // Nullable

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres")]
        public string PasswordHash { get; set; } = ""; // Valor por defecto

        public bool EsRegistroNormal { get; set; } // True si es registro normal, False si es con Google

        // ========== NUEVAS PROPIEDADES PARA CONFIRMACIÓN DE EMAIL ==========
        public bool EmailConfirmed { get; set; } // Indica si el email está confirmado
        public string? EmailConfirmationToken { get; set; } // Token único de confirmación
        public DateTime? ConfirmationTokenExpiry { get; set; } // Fecha de expiración del token

        // Agregar estas propiedades a la clase Usuario
        public string? ResetPasswordToken { get; set; }
        public DateTime? ResetPasswordTokenExpiry { get; set; }


        // ===================================================================

        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        // Relaciones (MANTENER TODAS EXISTENTES)
        public Billetera Billetera { get; set; }
        public ICollection<Transaccion> Transacciones { get; set; } = new List<Transaccion>();
        public ICollection<Meta> Metas { get; set; } = new List<Meta>();
        public ICollection<ChatMensaje> ChatMensajes { get; set; } = new List<ChatMensaje>();
        public ICollection<Recomendacion> Recomendaciones { get; set; } = new List<Recomendacion>();
    }
}