using EducationHub.Faturamento.Domain.Entidades;

namespace EducationHub.Faturamento.Domain.Repositorios
{
    public interface IPagamentoRepositorio: IDisposable
    {
        Task AdicionarAsync(Pagamento pagamento);
        Task<Pagamento> ObterPorIdAsync(Guid id);
        Task AtualizarAsync(Pagamento pagamento);
        Task<IEnumerable<Pagamento>> ObterPagamentosPorAlunoAsync(Guid alunoId);
        Task SaveChangesAsync();
    }
}
