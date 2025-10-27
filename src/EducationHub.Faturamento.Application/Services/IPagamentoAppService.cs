using EducationHub.Faturamento.Application.ViewModels;

namespace EducationHub.Faturamento.Application.Services
{
    public interface IPagamentoAppService: IDisposable
    {
        Task<PagamentoViewModel> RealizarPagamentoAsync(Guid alunoId, Guid preMatriculaId, decimal valor, DadosCartaoViewModel dadosCartao);
        Task<PagamentoViewModel?> ObterPorIdAsync(Guid id);
        Task<IEnumerable<PagamentoViewModel>> ObterPorAlunoAsync(Guid alunoId);
    }
}
