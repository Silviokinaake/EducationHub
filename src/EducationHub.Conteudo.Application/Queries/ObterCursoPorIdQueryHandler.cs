using AutoMapper;
using EducationHub.Conteudo.Application.ViewModels;
using EducationHub.Conteudo.Domain.Interfaces;
using MediatR;

namespace EducationHub.Conteudo.Application.Queries;

public class ObterCursoPorIdQueryHandler : IRequestHandler<ObterCursoPorIdQuery, CursoViewModel>
{
    private readonly ICursoRepositorio _cursoRepositorio;
    private readonly IMapper _mapper;

    public ObterCursoPorIdQueryHandler(ICursoRepositorio cursoRepositorio, IMapper mapper)
    {
        _cursoRepositorio = cursoRepositorio;
        _mapper = mapper;
    }

    public async Task<CursoViewModel> Handle(ObterCursoPorIdQuery request, CancellationToken cancellationToken)
    {
        var curso = await _cursoRepositorio.ObterPorIdAsync(request.CursoId);
        return _mapper.Map<CursoViewModel>(curso);
    }
}
