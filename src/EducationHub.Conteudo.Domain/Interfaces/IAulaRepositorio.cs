using EducationHub.Conteudo.Domain.Entidades;

namespace EducationHub.Conteudo.Domain.Interfaces
{
    public interface IAulaRepositorio: IDisposable
    {
        Task<IEnumerable<Aula>> ObterTodosAsync();
        Task<Aula> ObterPorIdAsync(Guid id);
        Task<IEnumerable<Aula>> ObterTodosAsync(Guid curso);
        Task AdicionarAsync(Aula aula);
        Task AtualizarAsync(Aula aula);
        Task RemoverAsync(Guid id);
    }
}
