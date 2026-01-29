using EducationHub.Faturamento.Domain.Entidades;
using EducationHub.Faturamento.Domain.Interfaces;

namespace EducationHub.Faturamento.Domain.Services
{
    public class PagamentoServico : IPagamentoServico
    {
        private readonly IPagamentoRepositorio _pagamentoRepositorio;
        private readonly IPagamentoGateway _pagamentoGateway;
        private readonly IMatriculaServico _matriculaServico;
        private readonly ICardTokenizationServico _cardTokenService;

        public PagamentoServico(
            IPagamentoRepositorio pagamentoRepositorio,
            IPagamentoGateway pagamentoGateway,
            IMatriculaServico matriculaServico,
            ICardTokenizationServico cardTokenService)
        {
            _pagamentoRepositorio = pagamentoRepositorio ?? throw new ArgumentNullException(nameof(pagamentoRepositorio));
            _pagamentoGateway = pagamentoGateway ?? throw new ArgumentNullException(nameof(pagamentoGateway));
            _matriculaServico = matriculaServico ?? throw new ArgumentNullException(nameof(matriculaServico));
            _cardTokenService = cardTokenService ?? throw new ArgumentNullException(nameof(cardTokenService));
        }

        public async Task<Pagamento> RealizarPagamentoAsync(Guid alunoId, Guid preMatriculaId, decimal valor, DadosCartao dadosCartao)
        {
            // VALIDAÇÃO ANTECIPADA: Verificar status e valor da matrícula ANTES de processar o pagamento
            // Apenas valida, NÃO ativa a matrícula ainda
            await _matriculaServico.ValidarPagamentoMatriculaAsync(preMatriculaId, valor);

            var pagamento = new Pagamento(alunoId, preMatriculaId, valor, dadosCartao);

            await _pagamentoRepositorio.AdicionarAsync(pagamento);
            await _pagamentoRepositorio.UnitOfWork.Commit();

            string token;
            try
            {
                token = await _cardTokenService.TokenizarAsync(dadosCartao);
            }
            catch (Exception)
            {
                pagamento.Rejeitar("Falha na tokenização do cartão");
                await _pagamentoRepositorio.AtualizarAsync(pagamento);
                await _pagamentoRepositorio.UnitOfWork.Commit();
                return pagamento;
            }

            var numeroMascarado = _cardTokenService.MascararNumero(dadosCartao.Numero);
            pagamento.AplicarTokenCartao(token, numeroMascarado);
            await _pagamentoRepositorio.AtualizarAsync(pagamento);
            await _pagamentoRepositorio.UnitOfWork.Commit();

            bool confirmado;
            try
            {
                confirmado = await _pagamentoGateway.ProcessarPagamentoAsync(pagamento, token);
            }
            catch (Exception)
            {
                pagamento.Rejeitar("Erro no processamento do gateway");
                await _pagamentoRepositorio.AtualizarAsync(pagamento);
                await _pagamentoRepositorio.UnitOfWork.Commit();
                return pagamento;
            }

            if (confirmado)
            {
                // ATIVAR MATRÍCULA ANTES DE CONFIRMAR O PAGAMENTO
                // Se a ativação falhar (ex: matrícula já ativa), o pagamento não será confirmado
                await _matriculaServico.AtivarMatriculaAsync(preMatriculaId, valor);

                // Somente após ativação bem-sucedida, confirma o pagamento
                pagamento.Confirmar();
                await _pagamentoRepositorio.AtualizarAsync(pagamento);
                await _pagamentoRepositorio.UnitOfWork.Commit();
            }
            else
            {
                pagamento.Rejeitar("Pagamento rejeitado pelo provedor");
                await _pagamentoRepositorio.AtualizarAsync(pagamento);
                await _pagamentoRepositorio.UnitOfWork.Commit();
            }

            return pagamento;
        }

        public async Task<bool> ConfirmarPagamentoAsync(Guid pagamentoId)
        {
            var pagamento = await _pagamentoRepositorio.ObterPorIdAsync(pagamentoId);
            if (pagamento is null) return false;

            pagamento.Confirmar();
            await _pagamentoRepositorio.AtualizarAsync(pagamento);
            var committed = await _pagamentoRepositorio.UnitOfWork.Commit();
            if (!committed) return false;

            await _matriculaServico.AtivarMatriculaAsync(pagamento.PreMatriculaId);
            return true;
        }

        public async Task<bool> RejeitarPagamentoAsync(Guid pagamentoId, string motivo)
        {
            var pagamento = await _pagamentoRepositorio.ObterPorIdAsync(pagamentoId);
            if (pagamento is null) return false;

            pagamento.Rejeitar(motivo);
            await _pagamentoRepositorio.AtualizarAsync(pagamento);
            return await _pagamentoRepositorio.UnitOfWork.Commit();
        }

        public async Task<Pagamento?> ObterPorIdAsync(Guid pagamentoId)
        {
            return await _pagamentoRepositorio.ObterPorIdAsync(pagamentoId);
        }

        public async Task<IEnumerable<Pagamento>> ObterPorAlunoAsync(Guid alunoId)
        {
            return await _pagamentoRepositorio.ObterPagamentosPorAlunoAsync(alunoId);
        }

        public void Dispose()
        {
            _pagamentoRepositorio?.Dispose();
        }
    }
}