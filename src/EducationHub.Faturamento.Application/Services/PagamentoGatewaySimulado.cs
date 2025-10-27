using EducationHub.Faturamento.Domain.Entidades;
using EducationHub.Faturamento.Domain.Interfaces;

namespace EducationHub.Faturamento.Application.Services
{
    public class PagamentoGatewaySimulado : IPagamentoGateway
    {
        public Task<bool> ProcessarPagamentoAsync(Pagamento pagamento, string? cardToken = null)
        {
            return Task.FromResult(true);
        }
    }
}
