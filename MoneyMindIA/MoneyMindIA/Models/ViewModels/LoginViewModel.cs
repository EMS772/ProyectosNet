using System.ComponentModel.DataAnnotations;


namespace MoneyMindIA.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "Debe ser un email válido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        //[Display(Name = "Recordarme")]
        //public bool RememberMe { get; set; }
    }
}
