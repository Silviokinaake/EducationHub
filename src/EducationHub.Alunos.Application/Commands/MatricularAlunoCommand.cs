using EducationHub.Alunos.Application.ViewModels;
using EducationHub.Core.Messages;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System.Text.Json.Serialization;
using static EducationHub.Alunos.Application.Commands.MatricularAlunoCommand;

namespace EducationHub.Alunos.Application.Commands
{
    public class MatricularAlunoCommand : Message, IRequest<MatriculaRealizadaViewModel>
    {
        [JsonIgnore]
        public Guid AlunoId { get; set; }
        
        public Guid CursoId { get; set; }
        
        [JsonIgnore]
        public DateTime Timestamp { get; private set; }
        
        [JsonIgnore]
        public ValidationResult ValidationResult { get; set; }

        public MatricularAlunoCommand()
        {
            Timestamp = DateTime.Now;
            ValidationResult = new ValidationResult();
        }

        public MatricularAlunoCommand(Guid alunoId, Guid cursoId)
        {
            AlunoId = alunoId;
            CursoId = cursoId;
            Timestamp = DateTime.Now;
            ValidationResult = new ValidationResult();
        }

        public bool EhValido()
        {
            ValidationResult = new MatricularAlunoValidattion().Validate(this);
            return ValidationResult.IsValid;
        }

        public class MatricularAlunoValidattion : AbstractValidator<MatricularAlunoCommand>
        { 
          public MatricularAlunoValidattion()
            {
                RuleFor(c => c.AlunoId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("O Id do aluno é obrigatório.");
                RuleFor(c => c.CursoId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("O Id do curso é obrigatório.");
            }
        }
    }
}
