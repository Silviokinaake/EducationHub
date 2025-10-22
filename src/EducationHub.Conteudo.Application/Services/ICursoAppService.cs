using EducationHub.Conteudo.Application.ViewModels;

namespace EducationHub.Conteudo.Application.Services
{
    public interface ICursoAppService : IDisposable
    {
        Task<IEnumerable<CursoViewModel>> ObterTodosAsync();
        Task<CursoViewModel> ObterPorIdAsync(Guid id);
        Task AdicionarAsync(CursoViewModel cursoViewModel);
        Task AtualizarAsync(CursoViewModel cursoViewModel);
    }
}
