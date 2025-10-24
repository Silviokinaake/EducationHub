using EducationHub.Conteudo.Application.ViewModels;

namespace EducationHub.Conteudo.Application.Services
{
    public interface IAulaAppService : IDisposable
    {
        Task<AulaViewModel?> ObterPorIdAsync(Guid id);
        Task<IEnumerable<AulaViewModel>> ObterPorCursoAsync(Guid cursoId);
        Task AdicionarAsync(AulaViewModel aulaViewModel);
        Task AtualizarAsync(AulaViewModel aulaViewModel);
    }
}
