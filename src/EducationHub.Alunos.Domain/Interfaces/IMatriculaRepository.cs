using EducationHub.Alunos.Domain.Entidades;

namespace EducationHub.Alunos.Domain.Interfaces
{
    public interface IMatriculaRepository
    {
        Task AdicionarAsync(Matricula mtricula);
        void AtualizarAsync(Matricula mtricula);
        Task CommitAsync();
    }
}
