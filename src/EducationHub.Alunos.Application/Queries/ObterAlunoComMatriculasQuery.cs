using EducationHub.Alunos.Application.ViewModels;
using MediatR;

namespace EducationHub.Alunos.Application.Queries;

public class ObterAlunoComMatriculasQuery : IRequest<AlunoComMatriculasViewModel>
{
    public Guid AlunoId { get; set; }

    public ObterAlunoComMatriculasQuery(Guid alunoId)
    {
        AlunoId = alunoId;
    }
}
