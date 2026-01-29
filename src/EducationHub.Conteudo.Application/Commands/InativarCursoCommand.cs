using EducationHub.Core.Messages;
using MediatR;

namespace EducationHub.Conteudo.Application.Commands
{
    public class InativarCursoCommand : Command, IRequest<bool>
    {
        public Guid CursoId { get; set; }

        public InativarCursoCommand(Guid cursoId)
        {
            CursoId = cursoId;
        }

        public override bool EhValido()
        {
            return CursoId != Guid.Empty;
        }
    }
}
