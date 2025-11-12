using AutoMapper;
using EducationHub.Faturamento.Application.ViewModels;
using EducationHub.Faturamento.Domain.Entidades;
using EducationHub.Faturamento.Domain.Interfaces;

namespace EducationHub.Faturamento.Application.Services
{
    public class PagamentoAppService : IPagamentoAppService
    {
        private readonly IPagamentoServico _pagamentoServico;
        private readonly IMapper _mapper;

        public PagamentoAppService(IPagamentoServico pagamentoServico, IMapper mapper)
        {
            _pagamentoServico = pagamentoServico;
            _mapper = mapper;
        }

        public async Task<PagamentoViewModel> RealizarPagamentoAsync(Guid alunoId, Guid preMatriculaId, decimal valor, DadosCartaoViewModel dadosCartao)
        {
            var vo = new DadosCartao(dadosCartao.NomeTitular, dadosCartao.Numero, dadosCartao.Validade, dadosCartao.Cvv);
            var pagamento = await _pagamentoServico.RealizarPagamentoAsync(alunoId, preMatriculaId, valor, vo);
            return _mapper.Map<PagamentoViewModel>(pagamento);
        }

        public async Task<PagamentoViewModel?> ObterPorIdAsync(Guid id)
        {
            var pagamento = await _pagamentoServico.ObterPorIdAsync(id);
            return _mapper.Map<PagamentoViewModel?>(pagamento);
        }

        public async Task<IEnumerable<PagamentoViewModel>> ObterPorAlunoAsync(Guid alunoId)
        {
            var pagamentos = await _pagamentoServico.ObterPorAlunoAsync(alunoId);
            return _mapper.Map<IEnumerable<PagamentoViewModel>>(pagamentos);
        }

        public void Dispose()
        {
            _pagamentoServico?.Dispose();
        }
    }
}
