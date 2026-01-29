using AutoMapper;
using EducationHub.Alunos.Application.ViewModels;
using EducationHub.Alunos.Domain.Repositorio;
using MediatR;

namespace EducationHub.Alunos.Application.Queries;

public class ObterTodasMatriculasQueryHandler : IRequestHandler<ObterTodasMatriculasQuery, IEnumerable<MatriculaViewModel>>
{
    private readonly IAlunoRepositorio _alunoRepositorio;
    private readonly IMapper _mapper;

    public ObterTodasMatriculasQueryHandler(IAlunoRepositorio alunoRepositorio, IMapper mapper)
    {
        _alunoRepositorio = alunoRepositorio;
        _mapper = mapper;
    }

    public async Task<IEnumerable<MatriculaViewModel>> Handle(ObterTodasMatriculasQuery request, CancellationToken cancellationToken)
    {
        var alunos = await _alunoRepositorio.ObterTodosAsync();
        
        if (alunos == null)
            return Enumerable.Empty<MatriculaViewModel>();

        var todasMatriculas = alunos
            .Where(a => a.Matriculas != null)
            .SelectMany(a => a.Matriculas);

        return _mapper.Map<IEnumerable<MatriculaViewModel>>(todasMatriculas);
    }
}
