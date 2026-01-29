using EducationHub.Conteudo.Application.ViewModels;
using MediatR;

namespace EducationHub.Conteudo.Application.Queries;

public class ObterCursoPorIdQuery : IRequest<CursoViewModel>
{
    public Guid CursoId { get; set; }

    public ObterCursoPorIdQuery(Guid cursoId)
    {
        CursoId = cursoId;
    }
}
