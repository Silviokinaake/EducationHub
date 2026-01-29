namespace EducationHub.Faturamento.Domain.Interfaces
{
    public interface IMatriculaServico
    {
        Task<bool> AtivarMatriculaAsync(Guid preMatriculaId);
        Task<bool> AtivarMatriculaAsync(Guid preMatriculaId, decimal valorPago);
        Task ValidarPagamentoMatriculaAsync(Guid preMatriculaId, decimal valorPago);
    }
}
