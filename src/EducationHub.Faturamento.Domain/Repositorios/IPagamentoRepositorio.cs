using EducationHub.Faturamento.Domain.Entidades;

namespace EducationHub.Faturamento.Domain.Repositorios
{
    public interface IPagamentoRepositorio: IDisposable
    {
        Task<IEnumerable<Pagamento>> ObterPagamentosPorAlunoAsync(Guid alunoId);
        Task AdicionarAsync(Pagamento pagamento);
        Task AtualizarAsync(Pagamento pagamento);
    }
}
