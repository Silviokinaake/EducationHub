using EducationHub.Conteudo.Application.ViewModels;

namespace EducationHub.Conteudo.Application.Services
{
    public interface ICursoAppService : IDisposable
    {
        Task<IEnumerable<CursoViewModel>> ObterTodos();
        Task<CursoViewModel> ObterPorId(Guid id);
        Task Adicionar(CursoViewModel cursoViewModel);
        Task Atualizar(CursoViewModel cursoViewModel);
    }
}
