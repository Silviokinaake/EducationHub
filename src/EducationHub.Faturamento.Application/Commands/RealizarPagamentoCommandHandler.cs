using EducationHub.Faturamento.Domain.Entidades;
using EducationHub.Faturamento.Domain.Enums;
using EducationHub.Faturamento.Domain.Interfaces;
using MediatR;

namespace EducationHub.Faturamento.Application.Commands
{
    public class RealizarPagamentoCommandHandler : IRequestHandler<RealizarPagamentoCommand, bool>
    {
        private readonly IPagamentoRepositorio _pagamentoRepositorio;

        public RealizarPagamentoCommandHandler(IPagamentoRepositorio pagamentoRepositorio)
        {
            _pagamentoRepositorio = pagamentoRepositorio;
        }

        public async Task<bool> Handle(RealizarPagamentoCommand request, CancellationToken cancellationToken)
        {
            if (!request.EhValido())
                return false;

            var dadosCartao = new DadosCartao(
                request.NumeroCartao,
                request.NomeTitular,
                request.ValidadeCartao,
                request.CodigoSeguranca
            );

            var pagamento = new Pagamento(
                request.AlunoId,
                request.PreMatriculaId,
                request.Valor,
                dadosCartao
            );

            // Simular processamento do pagamento
            // Em produção, aqui seria a integração com gateway de pagamento
            pagamento.Confirmar();

            _pagamentoRepositorio.Adicionar(pagamento);
            return await _pagamentoRepositorio.UnitOfWork.Commit();
        }
    }
}
