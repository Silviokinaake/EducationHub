using EducationHub.Conteudo.Domain.Entidades;
using EducationHub.Conteudo.Domain.Interfaces;
using MediatR;

namespace EducationHub.Conteudo.Application.Commands
{
    public class CriarCursoCommandHandler : IRequestHandler<CriarCursoCommand, bool>
    {
        private readonly ICursoRepositorio _cursoRepositorio;

        public CriarCursoCommandHandler(ICursoRepositorio cursoRepositorio)
        {
            _cursoRepositorio = cursoRepositorio;
        }

        public async Task<bool> Handle(CriarCursoCommand request, CancellationToken cancellationToken)
        {
            if (!request.EhValido())
                return false;

            var conteudoProgramatico = new Conteudo.Domain.Entidades.ConteudoProgramatico(
                request.ConteudoProgramatico.Objetivo,
                request.ConteudoProgramatico.Conteudo,
                request.ConteudoProgramatico.Metodologia ?? string.Empty,
                request.ConteudoProgramatico.Bibliografia ?? string.Empty
            );

            var curso = new Curso(
                request.Titulo,
                request.Descricao,
                request.CargaHoraria,
                request.Instrutor,
                request.Nivel,
                request.Valor,
                conteudoProgramatico
            );

            request.Id = curso.Id;

            _cursoRepositorio.Adicionar(curso);
            return await _cursoRepositorio.UnitOfWork.Commit();
        }
    }
}
