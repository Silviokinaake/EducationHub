using EducationHub.Core.Messages;
using MediatR;

namespace EducationHub.Faturamento.Application.Commands
{
    public class RealizarPagamentoCommand : Command, IRequest<bool>
    {
        public Guid Id { get; set; }
        public Guid AlunoId { get; set; }
        public Guid PreMatriculaId { get; set; }
        public decimal Valor { get; set; }
        public string NumeroCartao { get; set; }
        public string NomeTitular { get; set; }
        public string ValidadeCartao { get; set; }
        public string CodigoSeguranca { get; set; }

        public RealizarPagamentoCommand()
        {
            Id = Guid.NewGuid();
            NumeroCartao = string.Empty;
            NomeTitular = string.Empty;
            ValidadeCartao = string.Empty;
            CodigoSeguranca = string.Empty;
        }

        public override bool EhValido()
        {
            return AlunoId != Guid.Empty &&
                   PreMatriculaId != Guid.Empty &&
                   Valor > 0 &&
                   !string.IsNullOrWhiteSpace(NumeroCartao) &&
                   !string.IsNullOrWhiteSpace(NomeTitular);
        }
    }
}
