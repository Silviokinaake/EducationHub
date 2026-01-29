using EducationHub.Alunos.Domain.Interfaces;
using EducationHub.Faturamento.Domain.Events;
using MediatR;

namespace EducationHub.Alunos.Application.Events;

public class PagamentoRejeitadoEventHandler : INotificationHandler<PagamentoRejeitadoEvent>
{
    private readonly IMatriculaRepository _matriculaRepository;

    public PagamentoRejeitadoEventHandler(IMatriculaRepository matriculaRepository)
    {
        _matriculaRepository = matriculaRepository;
    }

    public async Task Handle(PagamentoRejeitadoEvent notification, CancellationToken cancellationToken)
    {
        var matricula = await _matriculaRepository.ObterPorId(notification.PreMatriculaId);
        
        if (matricula == null)
            return;

        matricula.Cancelar();
        _matriculaRepository.Atualizar(matricula);
        
        await _matriculaRepository.UnitOfWork.Commit();
    }
}
