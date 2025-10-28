using EducationHub.Alunos.Domain.Entidades;
using EducationHub.Core.Data;

namespace EducationHub.Alunos.Domain.Repositorio
{
    public interface IAlunoRepositorio: IDisposable
    {
        IUnitOfWork UnitOfWork { get; }
        Task<Aluno> ObterPorIdAsync(Guid id);
        Task<IEnumerable<Aluno>> ObterTodosAsync();
        Task AdicionarAsync(Aluno aluno);
        Task AtualizarAsync(Aluno aluno);


        Task<Matricula> ObterMatriculaPorIdAsync(Guid matriculaId);
        Task<IEnumerable<Matricula>> ObterMatriculasPorAlunoAsync(Guid alunoId);
        Task<IEnumerable<Certificado>> ObterCertificadosPorAlunoAsync(Guid alunoId);
    }
}

