using EducationHub.Faturamento.Domain.Interfaces;

namespace EducationHub.Faturamento.Application.Services
{
    public class MatriculaServicoSimulado : IMatriculaServico
    {
        public Task<bool> AtivarMatriculaAsync(Guid preMatriculaId)
        {
            return Task.FromResult(true);
        }

        public Task<bool> AtivarMatriculaAsync(Guid preMatriculaId, decimal valorPago)
        {
            return Task.FromResult(true);
        }

        public Task ValidarPagamentoMatriculaAsync(Guid preMatriculaId, decimal valorPago)
        {
            return Task.CompletedTask;
        }
    }
}
