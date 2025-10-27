using EducationHub.Faturamento.Domain.Entidades;

namespace EducationHub.Faturamento.Domain.Interfaces
{
    public interface IPagamentoServico : IDisposable
    {
        Task<Pagamento> RealizarPagamentoAsync(Guid alunoId, Guid preMatriculaId, decimal valor, DadosCartao dadosCartao);
        Task<bool> ConfirmarPagamentoAsync(Guid pagamentoId);
        Task<bool> RejeitarPagamentoAsync(Guid pagamentoId, string motivo);
        Task<Pagamento?> ObterPorIdAsync(Guid pagamentoId);
        Task<IEnumerable<Pagamento>> ObterPorAlunoAsync(Guid alunoId);
    }
}
