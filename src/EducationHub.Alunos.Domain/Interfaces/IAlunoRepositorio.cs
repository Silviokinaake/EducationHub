using EducationHub.Alunos.Domain.Entidades;
using EducationHub.Core.Data;

namespace EducationHub.Alunos.Domain.Repositorio
{
    public interface IAlunoRepositorio: IDisposable
    {
        IUnitOfWork UnitOfWork { get; }
        Task<Aluno> ObterPorIdAsync(Guid id);
        Task<Aluno> ObterPorUsuarioIdAsync(Guid usuarioId);
        Task<IEnumerable<Aluno>> ObterTodosAsync();
        Task AdicionarAsync(Aluno aluno);
        void Atualizar(Aluno aluno);
        
        Task<Aluno?> ObterPorCpf(string cpf);
        void Adicionar(Aluno aluno);

        Task<Matricula> ObterMatriculaPorIdAsync(Guid matriculaId);
        Task<IEnumerable<Matricula>> ObterMatriculasPorAlunoAsync(Guid alunoId);
        Task<IEnumerable<Certificado>> ObterCertificadosPorAlunoAsync(Guid alunoId);
    }
}

