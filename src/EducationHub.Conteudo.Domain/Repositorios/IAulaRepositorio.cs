using EducationHub.Conteudo.Domain.Entidades;

namespace EducationHub.Conteudo.Domain.Repositorios
{
    public interface IAulaRepositorio: IDisposable
    {
        Task<Aula> ObterPorIdAsync(Guid id);
        Task<IEnumerable<Aula>> ObterPorCursoAsync(Guid cursoId);
        Task AdicionarAsync(Aula aula);
        Task AtualizarAsync(Aula aula);

    }
}
