using EducationHub.Alunos.Application.ViewModels;

namespace EducationHub.Alunos.Application.Services
{
    public interface IAlunoAppService : IDisposable
    {
        Task<IEnumerable<AlunoViewModel>> ObterTodosAsync();
        Task<AlunoViewModel?> ObterPorIdAsync(Guid id);
        Task<AlunoViewModel?> CriarAsync(AlunoViewModel aluno);
        Task<bool> AtualizarAsync(AlunoViewModel aluno);

        Task<MatriculaViewModel?> ObterMatriculaPorIdAsync(Guid matriculaId);
        Task<IEnumerable<MatriculaViewModel>> ObterMatriculasPorAlunoAsync(Guid alunoId);
        Task<IEnumerable<CertificadoViewModel>> ObterCertificadosPorAlunoAsync(Guid alunoId);
    }
}