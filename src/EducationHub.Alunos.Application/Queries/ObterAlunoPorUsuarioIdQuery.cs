using EducationHub.Alunos.Application.ViewModels;
using MediatR;

namespace EducationHub.Alunos.Application.Queries
{
    public class ObterAlunoPorUsuarioIdQuery : IRequest<AlunoViewModel>
    {
        public Guid UsuarioId { get; set; }

        public ObterAlunoPorUsuarioIdQuery(Guid usuarioId)
        {
            UsuarioId = usuarioId;
        }
    }
}
