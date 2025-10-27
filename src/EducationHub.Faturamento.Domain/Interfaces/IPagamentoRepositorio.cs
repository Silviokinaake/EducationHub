using EducationHub.Core.Data;
using EducationHub.Faturamento.Domain.Entidades;

namespace EducationHub.Faturamento.Domain.Interfaces
{
    public interface IPagamentoRepositorio: IDisposable
    {
        IUnitOfWork UnitOfWork { get; }

        Task<IEnumerable<Pagamento>> ObterPagamentosPorAlunoAsync(Guid alunoId);
        Task<Pagamento?> ObterPorIdAsync(Guid id);
        Task AdicionarAsync(Pagamento pagamento);
        Task AtualizarAsync(Pagamento pagamento);
    }
}
