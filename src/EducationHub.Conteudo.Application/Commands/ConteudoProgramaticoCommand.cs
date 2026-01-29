using System.ComponentModel.DataAnnotations;

namespace EducationHub.Conteudo.Application.Commands
{
    public class ConteudoProgramaticoCommand
    {
        [Required(ErrorMessage = "O objetivo é obrigatório.")]
        [StringLength(250, MinimumLength = 3, ErrorMessage = "O objetivo deve ter entre 3 e 250 caracteres.")]
        public string Objetivo { get; set; }

        [Required(ErrorMessage = "O conteúdo é obrigatório.")]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "O conteúdo deve ter entre 10 e 1000 caracteres.")]
        public string Conteudo { get; set; }

        [StringLength(500, ErrorMessage = "A metodologia deve ter no máximo 500 caracteres.")]
        public string Metodologia { get; set; }

        [StringLength(500, ErrorMessage = "A bibliografia deve ter no máximo 500 caracteres.")]
        public string Bibliografia { get; set; }

        public ConteudoProgramaticoCommand()
        {
            Objetivo = string.Empty;
            Conteudo = string.Empty;
            Metodologia = string.Empty;
            Bibliografia = string.Empty;
        }
    }
}
