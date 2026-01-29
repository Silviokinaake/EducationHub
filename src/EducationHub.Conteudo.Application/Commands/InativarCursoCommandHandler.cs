using EducationHub.Conteudo.Domain.Interfaces;
using MediatR;

namespace EducationHub.Conteudo.Application.Commands
{
    public class InativarCursoCommandHandler : IRequestHandler<InativarCursoCommand, bool>
    {
        private readonly ICursoRepositorio _cursoRepositorio;

        public InativarCursoCommandHandler(ICursoRepositorio cursoRepositorio)
        {
            _cursoRepositorio = cursoRepositorio;
        }

        public async Task<bool> Handle(InativarCursoCommand request, CancellationToken cancellationToken)
        {
            if (!request.EhValido())
                return false;

            var curso = await _cursoRepositorio.ObterPorId(request.CursoId);
            if (curso == null)
                return false;

            curso.Inativar();

            _cursoRepositorio.Atualizar(curso);
            return await _cursoRepositorio.UnitOfWork.Commit();
        }
    }
}
