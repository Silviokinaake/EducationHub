using EducationHub.Alunos.Application.ViewModels;
using MediatR;

namespace EducationHub.Alunos.Application.Queries;

public class ObterMatriculasPorAlunoIdQuery : IRequest<IEnumerable<MatriculaViewModel>>
{
    public Guid AlunoId { get; set; }

    public ObterMatriculasPorAlunoIdQuery(Guid alunoId)
    {
        AlunoId = alunoId;
    }
}
