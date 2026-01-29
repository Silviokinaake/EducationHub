using EducationHub.Alunos.Domain.Events;
using MediatR;

namespace EducationHub.Alunos.Application.Events;

public class CursoConcluidoEventHandler : INotificationHandler<CursoConcluidoEvent>
{
    public Task Handle(CursoConcluidoEvent notification, CancellationToken cancellationToken)
    {
        // Evento registrado para auditoria/log
        // A emissão de certificado é feita via endpoint dedicado POST /api/Alunos/matriculas/{id}/certificado
        
        // TODO: Adicionar log de conclusão de curso
        // TODO: Enviar notificação ao aluno sobre conclusão
        
        return Task.CompletedTask;
    }
}
