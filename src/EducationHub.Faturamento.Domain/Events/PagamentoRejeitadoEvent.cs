using EducationHub.Core.Messages;

namespace EducationHub.Faturamento.Domain.Events;

public class PagamentoRejeitadoEvent : Event
{
    public Guid PagamentoId { get; private set; }
    public Guid AlunoId { get; private set; }
    public Guid PreMatriculaId { get; private set; }
    public string Motivo { get; private set; }

    public PagamentoRejeitadoEvent(Guid pagamentoId, Guid alunoId, Guid preMatriculaId, string motivo)
    {
        PagamentoId = pagamentoId;
        AlunoId = alunoId;
        PreMatriculaId = preMatriculaId;
        Motivo = motivo;
        AggregateId = pagamentoId;
    }
}
