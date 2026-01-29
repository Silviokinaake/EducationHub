using EducationHub.Conteudo.Domain.Interfaces;
using MediatR;

namespace EducationHub.Conteudo.Application.Commands
{
    public class AtualizarCursoCommandHandler : IRequestHandler<AtualizarCursoCommand, bool>
    {
        private readonly ICursoRepositorio _cursoRepositorio;

        public AtualizarCursoCommandHandler(ICursoRepositorio cursoRepositorio)
        {
            _cursoRepositorio = cursoRepositorio;
        }

        public async Task<bool> Handle(AtualizarCursoCommand request, CancellationToken cancellationToken)
        {
            if (!request.EhValido())
                return false;

            var curso = await _cursoRepositorio.ObterPorId(request.Id);
            if (curso == null)
                return false;

            var conteudoProgramatico = new Conteudo.Domain.Entidades.ConteudoProgramatico(
                request.ConteudoProgramatico.Objetivo,
                request.ConteudoProgramatico.Conteudo,
                request.ConteudoProgramatico.Metodologia ?? string.Empty,
                request.ConteudoProgramatico.Bibliografia ?? string.Empty
            );

            curso.AtualizarInformacoes(
                request.Titulo,
                request.Descricao,
                request.CargaHoraria,
                request.Instrutor,
                request.Nivel,
                request.Valor,
                conteudoProgramatico
            );

            _cursoRepositorio.Atualizar(curso);
            return await _cursoRepositorio.UnitOfWork.Commit();
        }
    }
}
