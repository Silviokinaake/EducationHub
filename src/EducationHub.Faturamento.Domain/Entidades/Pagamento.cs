using System;
using EducationHub.Core.DomainObjects;
using EducationHub.Faturamento.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationHub.Faturamento.Domain.Entidades
{
    public class Pagamento : Entity, IAggregateRoot
    {
        public Guid AlunoId { get; private set; }
        public Guid PreMatriculaId { get; private set; }
        public decimal Valor { get; private set; }
        public DateTime DataPagamento { get; private set; }
        public StatusPagamentoEnum Status { get; private set; }

        // Não persistir o VO completo
        [NotMapped]
        public DadosCartao DadosCartao { get; private set; }

        public string TokenCartao { get; private set; } = string.Empty;
        public string NumeroCartaoMascarado { get; private set; } = string.Empty;

        protected Pagamento() { }

        public Pagamento(Guid alunoId, Guid preMatriculaId, decimal valor, DadosCartao dadosCartao)
        {
            AlunoId = alunoId;
            PreMatriculaId = preMatriculaId;
            Valor = valor;
            DadosCartao = dadosCartao ?? throw new DomainException("Os dados do cartão são obrigatórios.");
            DataPagamento = DateTime.UtcNow;
            Status = StatusPagamentoEnum.Pendente;

            Validar();
        }

        private void Validar()
        {
            Validacoes.ValidarSeIgual(AlunoId, Guid.Empty, "AlunoId inválido.");
            Validacoes.ValidarSeIgual(PreMatriculaId, Guid.Empty, "PreMatriculaId inválido.");
            Validacoes.ValidarSeMenorQue(Valor, 0, "Valor do pagamento não pode ser negativo.");
        }

        public void Confirmar()
        {
            if (Status == StatusPagamentoEnum.Confirmado)
                throw new DomainException("O pagamento já foi confirmado.");

            Status = StatusPagamentoEnum.Confirmado;
            DataPagamento = DateTime.UtcNow;
        }

        public void Rejeitar(string motivo)
        {
            if (Status == StatusPagamentoEnum.Rejeitado)
                throw new DomainException("O pagamento já foi rejeitado.");

            Status = StatusPagamentoEnum.Rejeitado;
        }

        public void AplicarTokenCartao(string token, string numeroMascarado)
        {
            if (string.IsNullOrWhiteSpace(token)) throw new DomainException("Token do cartão inválido.");
            TokenCartao = token;
            NumeroCartaoMascarado = numeroMascarado ?? string.Empty;
        }

        public override string ToString()
        {
            return $"Pagamento [{Id}] - Aluno: {AlunoId} | Valor: {Valor:C} | Status: {Status}";
        }
    }
}
