using MoneyMindIA.Models.Entidades;
using System.ComponentModel.DataAnnotations;

public class ChatMensaje
{
    [Key]
    public int MensajeId { get; set; }

    [Required]
    [StringLength(4000)]
    public string Contenido { get; set; } = string.Empty; // Inicialización

    [Required]
    public DateTime FechaEnvio { get; set; } = DateTime.UtcNow;

    [Required]
    public bool EsUsuario { get; set; }

    // Relaciones
    public int RecomendacionId { get; set; }
    public Recomendacion Recomendacion { get; set; }

    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; }
}