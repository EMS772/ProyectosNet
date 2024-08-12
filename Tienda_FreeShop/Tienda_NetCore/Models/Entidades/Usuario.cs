using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Tienda_NetCore.Models.Enums;

namespace Tienda_NetCore.Models.Entidades
{
    public class Usuario : IdentityUser
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public TipoUsuario Rol { get; set; } = TipoUsuario.Cliente;
    }
}
