using System;
using System.ComponentModel.DataAnnotations;

namespace EducationHub.Conteudo.Application.ViewModels
{
    public class AulaViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo CursoId � obrigat�rio.")]
        public Guid CursoId { get; set; }

        [Required(ErrorMessage = "O campo T�tulo � obrigat�rio.")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "O campo T�tulo deve ter entre 3 e 150 caracteres.")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "O campo Conte�do da aula � obrigat�rio.")]
        [StringLength(5000, MinimumLength = 10, ErrorMessage = "O campo Conte�do deve ter entre 10 e 5000 caracteres.")]
        public string ConteudoAula { get; set; }

        [StringLength(1000, MinimumLength = 5, ErrorMessage = "O campo Material de Apoio deve ter entre 5 e 1000 caracteres.")]
        public string MaterialDeApoio { get; set; }

        [Required(ErrorMessage = "A dura��o � obrigat�ria.")]
        public TimeSpan Duracao { get; set; }
    }
}