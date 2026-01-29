namespace EducationHub.Alunos.Application.ViewModels
{
    public class CertificadoEmitidoViewModel
    {
        public Guid CertificadoId { get; set; }
        public string Codigo { get; set; }
        public DateTime DataEmissao { get; set; }
        public string MensagemCertificado { get; set; }
        public AlunoMatriculaViewModel Aluno { get; set; }
        public CursoMatriculaViewModel Curso { get; set; }

        public CertificadoEmitidoViewModel()
        {
            Codigo = string.Empty;
            MensagemCertificado = string.Empty;
            Aluno = new AlunoMatriculaViewModel();
            Curso = new CursoMatriculaViewModel();
        }
    }
}
