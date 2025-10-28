using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EducationHub.Alunos.Application.ViewModels
{
    public class AlunoViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public Guid UsuarioId { get; set; }

        [Required, StringLength(150, MinimumLength = 3)]
        public string Nome { get; set; }

        [Required, EmailAddress, StringLength(150)]
        public string Email { get; set; }

        [Required]
        public DateTime DataNascimento { get; set; }

        //public ICollection<MatriculaViewModel> Matriculas { get; set; } = new List<MatriculaViewModel>();
        //public ICollection<CertificadoViewModel> Certificados { get; set; } = new List<CertificadoViewModel>();
    }
}
