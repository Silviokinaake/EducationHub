using EducationHub.Core.Messages;
using MediatR;
using System.Text.Json.Serialization;

namespace EducationHub.Alunos.Application.Commands
{
    public class CriarAlunoCommand : Command, IRequest<bool>
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        
        [JsonIgnore]
        public Guid UsuarioId { get; set; }
        
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Cpf { get; set; }
        public DateTime DataNascimento { get; set; }

        public CriarAlunoCommand()
        {
            Id = Guid.NewGuid();
            Nome = string.Empty;
            Email = string.Empty;
            Cpf = string.Empty;
        }

        public override bool EhValido()
        {
            return !string.IsNullOrWhiteSpace(Nome) &&
                   !string.IsNullOrWhiteSpace(Email) &&
                   !string.IsNullOrWhiteSpace(Cpf) &&
                   DataNascimento < DateTime.Now;
        }
    }
}