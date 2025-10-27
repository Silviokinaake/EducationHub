using EducationHub.Faturamento.Domain.Entidades;

namespace EducationHub.Faturamento.Domain.Interfaces
{
    public interface IPagamentoGateway
    {
        Task<bool> ProcessarPagamentoAsync(Pagamento pagamento, string? cardToken = null);
    }
}
