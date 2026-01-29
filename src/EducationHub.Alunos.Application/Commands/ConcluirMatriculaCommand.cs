using EducationHub.Core.Messages;
using FluentValidation;

namespace EducationHub.Alunos.Application.Commands
{
    public class ConcluirMatriculaCommand : Command
    {
        public Guid MatriculaId { get; set; }

        public ConcluirMatriculaCommand(Guid matriculaId)
        {
            MatriculaId = matriculaId;
        }

        public override bool EhValido()
        {
            ValidationResult = new ConcluirMatriculaValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class ConcluirMatriculaValidation : AbstractValidator<ConcluirMatriculaCommand>
    {
        public ConcluirMatriculaValidation()
        {
            RuleFor(c => c.MatriculaId)
                .NotEqual(Guid.Empty)
                .WithMessage("O ID da matrícula é obrigatório.");
        }
    }
}
