using EducationHub.Core.DomainObjects;
using EducationHub.Faturamento.Domain.Enums;

namespace EducationHub.Faturamento.Domain.Entidades
{
    public class Pagamento : Entity, IAggregateRoot
    {
        public Guid AlunoId { get; private set; }
        public Guid MatriculaId { get; private set; }
        public decimal Valor { get; private set; }
        public DateTime DataPagamento { get; private set; }
        public StatusPagamentoEnum Status { get; private set; }
        public DadosCartao DadosCartao { get; private set; }

        protected Pagamento() { }

        public Pagamento(Guid alunoId, Guid matriculaId, decimal valor, DadosCartao dadosCartao)
        {
            AlunoId = alunoId;
            MatriculaId = matriculaId;
            Valor = valor;
            DadosCartao = dadosCartao ?? throw new DomainException("Os dados do cartão são obrigatórios.");
            DataPagamento = DateTime.UtcNow;
            Status = StatusPagamentoEnum.Pendente;

            Validar();
        }

        private void Validar()
        {
            Validacoes.ValidarSeIgual(AlunoId, Guid.Empty, "AlunoId inválido.");
            Validacoes.ValidarSeIgual(MatriculaId, Guid.Empty, "MatriculaId inválido.");
            Validacoes.ValidarSeMenorQue(Valor, 0, "Valor do pagamento não pode ser negativo.");
        }

        public void Confirmar()
        {
            if (Status == StatusPagamentoEnum.Confirmado)
                throw new DomainException("O pagamento já foi confirmado.");

            Status = StatusPagamentoEnum.Confirmado;
            DataPagamento = DateTime.UtcNow;

            // Aqui você poderia disparar um evento de domínio (ex: PagamentoConfirmadoEvent)
        }

        public void Rejeitar(string motivo)
        {
            if (Status == StatusPagamentoEnum.Rejeitado)
                throw new DomainException("O pagamento já foi rejeitado.");

            Status = StatusPagamentoEnum.Rejeitado;
            // Pode armazenar log/motivo no evento de rejeição
        }

        public override string ToString()
        {
            return $"Pagamento [{Id}] - Aluno: {AlunoId} | Valor: {Valor:C} | Status: {Status}";
        }
    }
}
