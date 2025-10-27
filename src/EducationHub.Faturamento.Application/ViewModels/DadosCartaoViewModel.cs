using System.ComponentModel.DataAnnotations;

namespace EducationHub.Faturamento.Application.ViewModels
{
    public class DadosCartaoViewModel
    {
        [Required] 
        public string NomeTitular { get; set; }
        [Required] 
        public string Numero { get; set; }
        [Required] 
        public string Validade { get; set; } 
        [Required] 
        public string Cvv { get; set; }
    }
}
