using EducationHub.Conteudo.Domain.Entidades;
using EducationHub.Core.Data;

namespace EducationHub.Conteudo.Domain.Interfaces
{
    public interface ICursoRepositorio: IRepository<Curso>
    {
        Task<IEnumerable<Curso>> ObterTodosAsync();
        Task<Curso> ObterPorIdAsync(Guid id);
        Task AdicionarAsync(Curso curso);
        Task AtualizarAsync(Curso curso);
        Task<Curso> ObterPorId(Guid id);
        void Adicionar(Curso curso);
        void Atualizar(Curso curso);
    }
}
