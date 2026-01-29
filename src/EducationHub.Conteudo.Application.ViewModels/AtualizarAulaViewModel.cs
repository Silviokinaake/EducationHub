using System;
using System.ComponentModel.DataAnnotations;

namespace EducationHub.Conteudo.Application.ViewModels
{
    public class AtualizarAulaViewModel
    {
        [Required(ErrorMessage = "O campo CursoId é obrigatório.")]
        public Guid CursoId { get; set; }

        [Required(ErrorMessage = "O campo Título é obrigatório.")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "O campo Título deve ter entre 3 e 150 caracteres.")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "O campo Conteúdo da aula é obrigatório.")]
        [StringLength(5000, MinimumLength = 10, ErrorMessage = "O campo Conteúdo deve ter entre 10 e 5000 caracteres.")]
        public string ConteudoAula { get; set; }

        [StringLength(1000, MinimumLength = 5, ErrorMessage = "O campo Material de Apoio deve ter entre 5 e 1000 caracteres.")]
        public string MaterialDeApoio { get; set; }

        [Required(ErrorMessage = "A duração é obrigatória.")]
        public TimeSpan Duracao { get; set; }
    }
}
