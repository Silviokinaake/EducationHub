using EducationHub.Alunos.Application.ViewModels;
using MediatR;

namespace EducationHub.Alunos.Application.Queries;

public class ObterTodasMatriculasQuery : IRequest<IEnumerable<MatriculaViewModel>>
{
}
