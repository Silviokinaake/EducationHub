using EducationHub.Conteudo.Application.ViewModels;
using EducationHub.Conteudo.Domain.Enums;
using MediatR;

namespace EducationHub.Conteudo.Application.Queries;

public class ObterCursosQuery : IRequest<IEnumerable<CursoViewModel>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public SituacaoCurso? Situacao { get; set; }

    public ObterCursosQuery(int pageNumber = 1, int pageSize = 10, SituacaoCurso? situacao = null)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        Situacao = situacao;
    }
}
