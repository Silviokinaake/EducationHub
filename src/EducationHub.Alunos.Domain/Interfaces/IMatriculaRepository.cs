using EducationHub.Alunos.Domain.Entidades;
using EducationHub.Core.Data;

namespace EducationHub.Alunos.Domain.Interfaces
{
    public interface IMatriculaRepository : IRepository<Matricula>
    {
        Task<Matricula> ObterPorId(Guid id);
        Task AdicionarAsync(Matricula matricula);
        void Atualizar(Matricula matricula);
        Task CommitAsync();
    }
}
