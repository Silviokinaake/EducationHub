using EducationHub.Alunos.Domain.Interfaces;
using EducationHub.Faturamento.Domain.Events;
using MediatR;

namespace EducationHub.Alunos.Application.Events;

public class PagamentoRealizadoEventHandler : INotificationHandler<PagamentoRealizadoEvent>
{
    private readonly IMatriculaRepository _matriculaRepository;

    public PagamentoRealizadoEventHandler(IMatriculaRepository matriculaRepository)
    {
        _matriculaRepository = matriculaRepository;
    }

    public async Task Handle(PagamentoRealizadoEvent notification, CancellationToken cancellationToken)
    {
        var matricula = await _matriculaRepository.ObterPorId(notification.PreMatriculaId);
        
        if (matricula == null)
            return;

        matricula.Ativar();
        _matriculaRepository.Atualizar(matricula);
        
        await _matriculaRepository.UnitOfWork.Commit();
    }
}
