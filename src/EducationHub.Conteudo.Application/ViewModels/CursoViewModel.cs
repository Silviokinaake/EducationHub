using System.ComponentModel.DataAnnotations;

namespace EducationHub.Conteudo.Application.ViewModels
    {
        public class CursoViewModel
        {
            public Guid Id { get; set; }

            [Required(ErrorMessage = "O campo Título é obrigatório.")]
            [StringLength(250, MinimumLength = 3, ErrorMessage = "O campo Título deve ter entre 3 e 250 caracteres.")]
            public string Titulo { get; set; }

            [Required(ErrorMessage = "O campo Descrição é obrigatório.")]
            [StringLength(500, MinimumLength = 10, ErrorMessage = "O campo Descrição deve ter entre 10 e 500 caracteres.")]
            public string Descricao { get; set; }

            [Required(ErrorMessage = "A carga horária é obrigatória.")]
            public TimeSpan CargaHoraria { get; set; }

            [Required(ErrorMessage = "O campo Instrutor é obrigatório.")]
            [StringLength(100, MinimumLength = 3, ErrorMessage = "O campo Instrutor deve ter entre 3 e 100 caracteres.")]
            public string Instrutor { get; set; }

            public bool Ativo { get; set; }

            [Required(ErrorMessage = "O campo Nível é obrigatório.")]
            [StringLength(50, MinimumLength = 3, ErrorMessage = "O campo Nível deve ter entre 3 e 50 caracteres.")]
            public string Nivel { get; set; }

            [Required(ErrorMessage = "O conteúdo programático é obrigatório.")]
            public ConteudoProgramaticoViewModel ConteudoProgramatico { get; set; }

            public ICollection<AulaViewModel> Aulas { get; set; } = new List<AulaViewModel>();
        }

        public class ConteudoProgramaticoViewModel
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
        }
    }
