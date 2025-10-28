using System;

namespace EducationHub.Alunos.Application.ViewModels
{
    public class CertificadoViewModel
    {
        public Guid Id { get; set; }
        public Guid AlunoId { get; set; }
        public Guid CursoId { get; set; }
        public string TituloCurso { get; set; }
        public DateTime DataEmissao { get; set; }
        public string Codigo { get; set; }
    }
}
