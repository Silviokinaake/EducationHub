using FluentValidation.Results;
using MediatR;
using System.Text.Json.Serialization;

namespace EducationHub.Core.Messages
{
    public abstract class Command : Message, IRequest<bool>
    {
        [JsonIgnore]
        public DateTime Timestamp { get; private set; }
        
        [JsonIgnore]
        public ValidationResult ValidationResult { get; set; }

        protected Command()
        {
            Timestamp = DateTime.Now;
            ValidationResult = new ValidationResult();
        }

        public virtual bool EhValido()
        {
            throw new NotImplementedException();
        }
    }
}
