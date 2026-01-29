using EducationHub.Alunos.Application.ViewModels;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace EducationHub.Alunos.Application.Commands
{
    public class EmitirCertificadoCommand : IRequest<CertificadoEmitidoViewModel>
    {
        public Guid MatriculaId { get; set; }
        
        public ValidationResult ValidationResult { get; set; }

        public EmitirCertificadoCommand(Guid matriculaId)
        {
            MatriculaId = matriculaId;
        }

        public bool EhValido()
        {
            ValidationResult = new EmitirCertificadoValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class EmitirCertificadoValidation : AbstractValidator<EmitirCertificadoCommand>
    {
        public EmitirCertificadoValidation()
        {
            RuleFor(c => c.MatriculaId)
                .NotEqual(Guid.Empty)
                .WithMessage("O ID da matrícula é obrigatório.");
        }
    }
}
