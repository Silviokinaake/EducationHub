using EducationHub.Alunos.Domain.Entidades;

namespace EducationHub.Alunos.Domain.Repositorio
{
    public interface IAlunoRepositorio: IDisposable
    {
        Task<Aluno> ObterPorIdAsync(Guid id);
        Task<Aluno> ObterPorEmailAsync(string email);
        Task<IEnumerable<Aluno>> ObterTodosAsync();
        Task AdicionarAsync(Aluno aluno);
        Task AtualizarAsync(Aluno aluno);
        Task RemoverAsync(Guid id);

        Task<Matricula> ObterMatriculaPorIdAsync(Guid matriculaId);
        Task<IEnumerable<Matricula>> ObterMatriculasPorAlunoAsync(Guid alunoId);
        Task<IEnumerable<Certificado>> ObterCertificadosPorAlunoAsync(Guid alunoId);
    }
}

