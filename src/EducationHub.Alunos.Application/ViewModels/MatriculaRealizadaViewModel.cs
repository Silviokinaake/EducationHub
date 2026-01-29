namespace EducationHub.Alunos.Application.ViewModels
{
    public class MatriculaRealizadaViewModel
    {
        public string Message { get; set; }
        public Guid MatriculaId { get; set; }
        public AlunoMatriculaViewModel Aluno { get; set; }
        public CursoMatriculaViewModel Curso { get; set; }

        public MatriculaRealizadaViewModel()
        {
            Message = string.Empty;
            Aluno = new AlunoMatriculaViewModel();
            Curso = new CursoMatriculaViewModel();
        }
    }

    public class AlunoMatriculaViewModel
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public DateTime DataNascimento { get; set; }

        public AlunoMatriculaViewModel()
        {
            Nome = string.Empty;
            Email = string.Empty;
        }
    }

    public class CursoMatriculaViewModel
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; }
        public TimeSpan CargaHoraria { get; set; }
        public string Descricao { get; set; }
        public string Instrutor { get; set; }
        public string Nivel { get; set; }
        public decimal Valor { get; set; }

        public CursoMatriculaViewModel()
        {
            Titulo = string.Empty;
            Descricao = string.Empty;
            Instrutor = string.Empty;
            Nivel = string.Empty;
        }
    }
}
