using EducationHub.Alunos.Application.ViewModels;
using EducationHub.Alunos.Domain.Repositorio;
using EducationHub.Conteudo.Application.Queries;
using MediatR;

namespace EducationHub.Alunos.Application.Queries;

public class ObterAlunoComMatriculasQueryHandler : IRequestHandler<ObterAlunoComMatriculasQuery, AlunoComMatriculasViewModel>
{
    private readonly IAlunoRepositorio _alunoRepositorio;
    private readonly IMediator _mediator;

    public ObterAlunoComMatriculasQueryHandler(IAlunoRepositorio alunoRepositorio, IMediator mediator)
    {
        _alunoRepositorio = alunoRepositorio;
        _mediator = mediator;
    }

    public async Task<AlunoComMatriculasViewModel> Handle(ObterAlunoComMatriculasQuery request, CancellationToken cancellationToken)
    {
        var aluno = await _alunoRepositorio.ObterPorIdAsync(request.AlunoId);
        
        if (aluno == null)
            return null;

        var resultado = new AlunoComMatriculasViewModel
        {
            AlunoId = aluno.Id,
            NomeAluno = aluno.Nome,
            EmailAluno = aluno.Email,
            Matriculas = new List<MatriculaDetalhadaViewModel>()
        };

        if (aluno.Matriculas == null || !aluno.Matriculas.Any())
            return resultado;

        // Para cada matrícula, buscar os dados do curso
        foreach (var matricula in aluno.Matriculas)
        {
            var curso = await _mediator.Send(new ObterCursoPorIdQuery(matricula.CursoId), cancellationToken);
            
            if (curso != null)
            {
                resultado.Matriculas.Add(new MatriculaDetalhadaViewModel
                {
                    MatriculaId = matricula.Id,
                    CursoId = matricula.CursoId,
                    TituloCurso = curso.Titulo,
                    DescricaoCurso = curso.Descricao,
                    InstrutorCurso = curso.Instrutor,
                    NivelCurso = curso.Nivel,
                    CargaHorariaCurso = curso.CargaHoraria,
                    ValorMatricula = matricula.Valor,
                    DataMatricula = matricula.DataMatricula,
                    Status = matricula.Status.ToString()
                });
            }
        }

        return resultado;
    }
}
