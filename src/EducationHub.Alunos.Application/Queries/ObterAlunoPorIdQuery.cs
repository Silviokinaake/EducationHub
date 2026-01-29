using EducationHub.Alunos.Application.ViewModels;
using MediatR;

namespace EducationHub.Alunos.Application.Queries;

public class ObterAlunoPorIdQuery : IRequest<AlunoViewModel>
{
    public Guid AlunoId { get; set; }

    public ObterAlunoPorIdQuery(Guid alunoId)
    {
        AlunoId = alunoId;
    }
}
