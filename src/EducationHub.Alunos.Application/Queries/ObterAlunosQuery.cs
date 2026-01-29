using EducationHub.Alunos.Application.ViewModels;
using MediatR;

namespace EducationHub.Alunos.Application.Queries;

public class ObterAlunosQuery : IRequest<IEnumerable<AlunoViewModel>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public ObterAlunosQuery(int pageNumber = 1, int pageSize = 10)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}
