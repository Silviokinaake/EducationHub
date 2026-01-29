using AutoMapper;
using EducationHub.Conteudo.Application.ViewModels;
using EducationHub.Conteudo.Domain.Interfaces;
using MediatR;

namespace EducationHub.Conteudo.Application.Queries;

public class ObterCursosQueryHandler : IRequestHandler<ObterCursosQuery, IEnumerable<CursoViewModel>>
{
    private readonly ICursoRepositorio _cursoRepositorio;
    private readonly IMapper _mapper;

    public ObterCursosQueryHandler(ICursoRepositorio cursoRepositorio, IMapper mapper)
    {
        _cursoRepositorio = cursoRepositorio;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CursoViewModel>> Handle(ObterCursosQuery request, CancellationToken cancellationToken)
    {
        var cursos = await _cursoRepositorio.ObterTodosAsync();
        
        // Filtrar por situação, se especificado
        if (request.Situacao.HasValue)
        {
            cursos = cursos.Where(c => c.Situacao == request.Situacao.Value);
        }
        
        // Paginação simples
        var cursosPaginados = cursos
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize);

        return _mapper.Map<IEnumerable<CursoViewModel>>(cursosPaginados);
    }
}
