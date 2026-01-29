using AutoMapper;
using EducationHub.Alunos.Application.ViewModels;
using EducationHub.Alunos.Domain.Repositorio;
using MediatR;

namespace EducationHub.Alunos.Application.Queries;

public class ObterMatriculasPorAlunoIdQueryHandler : IRequestHandler<ObterMatriculasPorAlunoIdQuery, IEnumerable<MatriculaViewModel>>
{
    private readonly IAlunoRepositorio _alunoRepositorio;
    private readonly IMapper _mapper;

    public ObterMatriculasPorAlunoIdQueryHandler(IAlunoRepositorio alunoRepositorio, IMapper mapper)
    {
        _alunoRepositorio = alunoRepositorio;
        _mapper = mapper;
    }

    public async Task<IEnumerable<MatriculaViewModel>> Handle(ObterMatriculasPorAlunoIdQuery request, CancellationToken cancellationToken)
    {
        var aluno = await _alunoRepositorio.ObterPorIdAsync(request.AlunoId);
        
        if (aluno == null || aluno.Matriculas == null)
            return Enumerable.Empty<MatriculaViewModel>();

        return _mapper.Map<IEnumerable<MatriculaViewModel>>(aluno.Matriculas);
    }
}
