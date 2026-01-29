namespace EducationHub.Alunos.Application.ViewModels
{
    public class AlunoComMatriculasViewModel
    {
        public Guid AlunoId { get; set; }
        public string NomeAluno { get; set; }
        public string EmailAluno { get; set; }
        public List<MatriculaDetalhadaViewModel> Matriculas { get; set; }

        public AlunoComMatriculasViewModel()
        {
            NomeAluno = string.Empty;
            EmailAluno = string.Empty;
            Matriculas = new List<MatriculaDetalhadaViewModel>();
        }
    }

    public class MatriculaDetalhadaViewModel
    {
        public Guid MatriculaId { get; set; }
        public Guid CursoId { get; set; }
        public string TituloCurso { get; set; }
        public string DescricaoCurso { get; set; }
        public string InstrutorCurso { get; set; }
        public string NivelCurso { get; set; }
        public TimeSpan CargaHorariaCurso { get; set; }
        public decimal ValorMatricula { get; set; }
        public DateTime DataMatricula { get; set; }
        public string Status { get; set; }

        public MatriculaDetalhadaViewModel()
        {
            TituloCurso = string.Empty;
            DescricaoCurso = string.Empty;
            InstrutorCurso = string.Empty;
            NivelCurso = string.Empty;
            Status = string.Empty;
        }
    }
}
