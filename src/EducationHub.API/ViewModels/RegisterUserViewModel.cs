using System.ComponentModel.DataAnnotations;

namespace EducationHub.API.ViewModels
{
    public class RegisterUserViewModel
    {
        [Required(ErrorMessage = "Atributo email é obrigatório")]
        [EmailAddress(ErrorMessage = "Atributo email está em formato inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Atributo senha é obrigatório")]
        [StringLength(maximumLength: 25, ErrorMessage = "Atributo senha precisa ter entre {2} e {1} caracteres", MinimumLength = 8)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "As senhas não conferem.")]
        public string ConfirmPassword { get; set; }

        public bool EhAdministrador { get; set; }
    }
}
