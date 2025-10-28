using System;
using System.ComponentModel.DataAnnotations;

namespace EducationHub.Alunos.Application.ViewModels
{
    public class MatriculaViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public Guid CursoId { get; set; }

        [Required]
        public Guid AlunoId { get; set; }

        [Required]
        public decimal Valor { get; set; }

        [Required]
        public DateTime DataMatricula { get; set; }

        public string Status { get; set; }
    }
}
