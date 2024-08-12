﻿using System.ComponentModel.DataAnnotations;

namespace Tienda_NetCore.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [DataType(DataType.Password, ErrorMessage = "El formato de la contraseña no es válido.")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
