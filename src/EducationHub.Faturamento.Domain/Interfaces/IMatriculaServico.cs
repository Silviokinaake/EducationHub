namespace EducationHub.Faturamento.Domain.Interfaces
{
    public interface IMatriculaServico
    {
        Task<bool> AtivarMatriculaAsync(Guid preMatriculaId);
    }
}
