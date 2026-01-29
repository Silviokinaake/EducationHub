using AutoMapper;
using EducationHub.Alunos.Application.ViewModels;
using EducationHub.Alunos.Domain.Repositorio;
using MediatR;

namespace EducationHub.Alunos.Application.Queries;

public class ObterAlunoPorIdQueryHandler : IRequestHandler<ObterAlunoPorIdQuery, AlunoViewModel>
{
    private readonly IAlunoRepositorio _alunoRepositorio;
    private readonly IMapper _mapper;

    public ObterAlunoPorIdQueryHandler(IAlunoRepositorio alunoRepositorio, IMapper mapper)
    {
        _alunoRepositorio = alunoRepositorio;
        _mapper = mapper;
    }

    public async Task<AlunoViewModel> Handle(ObterAlunoPorIdQuery request, CancellationToken cancellationToken)
    {
        var aluno = await _alunoRepositorio.ObterPorIdAsync(request.AlunoId);
        return _mapper.Map<AlunoViewModel>(aluno);
    }
}
