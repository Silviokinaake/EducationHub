using EducationHub.Core.Messages;

namespace EducationHub.Alunos.Domain.Events;

public class CursoConcluidoEvent : Event
{
    public Guid MatriculaId { get; private set; }
    public Guid AlunoId { get; private set; }
    public Guid CursoId { get; private set; }
    public DateTime DataConclusao { get; private set; }

    public CursoConcluidoEvent(Guid matriculaId, Guid alunoId, Guid cursoId)
    {
        MatriculaId = matriculaId;
        AlunoId = alunoId;
        CursoId = cursoId;
        DataConclusao = DateTime.UtcNow;
        AggregateId = matriculaId;
    }
}
