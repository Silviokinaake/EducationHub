using EducationHub.Core.Messages;

namespace EducationHub.Faturamento.Domain.Events;

public class PagamentoRealizadoEvent : Event
{
    public Guid PagamentoId { get; private set; }
    public Guid AlunoId { get; private set; }
    public Guid PreMatriculaId { get; private set; }
    public decimal Valor { get; private set; }
    public DateTime DataPagamento { get; private set; }

    public PagamentoRealizadoEvent(Guid pagamentoId, Guid alunoId, Guid preMatriculaId, decimal valor)
    {
        PagamentoId = pagamentoId;
        AlunoId = alunoId;
        PreMatriculaId = preMatriculaId;
        Valor = valor;
        DataPagamento = DateTime.UtcNow;
        AggregateId = pagamentoId;
    }
}
