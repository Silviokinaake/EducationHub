using EducationHub.Core.Messages;

namespace EducationHub.Alunos.Domain.Events;

public class MatriculaConfirmadaEvent : Event
{
    public Guid MatriculaId { get; private set; }
    public Guid AlunoId { get; private set; }
    public Guid CursoId { get; private set; }
    public DateTime DataConfirmacao { get; private set; }

    public MatriculaConfirmadaEvent(Guid matriculaId, Guid alunoId, Guid cursoId)
    {
        MatriculaId = matriculaId;
        AlunoId = alunoId;
        CursoId = cursoId;
        DataConfirmacao = DateTime.UtcNow;
        AggregateId = matriculaId;
    }
}
