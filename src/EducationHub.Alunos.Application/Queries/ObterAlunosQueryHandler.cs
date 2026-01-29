using AutoMapper;
using EducationHub.Alunos.Application.ViewModels;
using EducationHub.Alunos.Domain.Repositorio;
using MediatR;

namespace EducationHub.Alunos.Application.Queries;

public class ObterAlunosQueryHandler : IRequestHandler<ObterAlunosQuery, IEnumerable<AlunoViewModel>>
{
    private readonly IAlunoRepositorio _alunoRepositorio;
    private readonly IMapper _mapper;

    public ObterAlunosQueryHandler(IAlunoRepositorio alunoRepositorio, IMapper mapper)
    {
        _alunoRepositorio = alunoRepositorio;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AlunoViewModel>> Handle(ObterAlunosQuery request, CancellationToken cancellationToken)
    {
        var alunos = await _alunoRepositorio.ObterTodosAsync();
        
        // Paginação simples
        var alunosPaginados = alunos
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize);

        return _mapper.Map<IEnumerable<AlunoViewModel>>(alunosPaginados);
    }
}
