using EducationHub.Alunos.Domain.Interfaces;
using EducationHub.Core.DomainObjects;
using EducationHub.Core.Messages;
using MediatR;

namespace EducationHub.Alunos.Application.Commands
{
    public class ConcluirMatriculaCommandHandler : IRequestHandler<ConcluirMatriculaCommand, bool>
    {
        private readonly IMatriculaRepository _matriculaRepository;

        public ConcluirMatriculaCommandHandler(IMatriculaRepository matriculaRepository)
        {
            _matriculaRepository = matriculaRepository;
        }

        public async Task<bool> Handle(ConcluirMatriculaCommand request, CancellationToken cancellationToken)
        {
            if (!request.EhValido()) return false;

            var matricula = await _matriculaRepository.ObterPorId(request.MatriculaId);
            
            if (matricula == null)
                throw new DomainException("Matrícula não encontrada.");

            // Concluir matrícula (valida se está Ativa)
            matricula.Concluir();

            _matriculaRepository.Atualizar(matricula);
            await _matriculaRepository.UnitOfWork.Commit();

            return true;
        }
    }
}
