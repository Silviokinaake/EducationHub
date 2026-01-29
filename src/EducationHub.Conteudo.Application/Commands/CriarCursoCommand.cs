using EducationHub.Core.Messages;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EducationHub.Conteudo.Application.Commands
{
    public class CriarCursoCommand : Command, IRequest<bool>
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public TimeSpan CargaHoraria { get; set; }
        public string Instrutor { get; set; }
        public string Nivel { get; set; }
        public decimal Valor { get; set; }
        
        [Required]
        public ConteudoProgramaticoCommand ConteudoProgramatico { get; set; }

        public CriarCursoCommand()
        {
            Id = Guid.NewGuid();
            Titulo = string.Empty;
            Descricao = string.Empty;
            Instrutor = string.Empty;
            Nivel = string.Empty;
            ConteudoProgramatico = new ConteudoProgramaticoCommand();
        }

        public override bool EhValido()
        {
            return !string.IsNullOrWhiteSpace(Titulo) &&
                   !string.IsNullOrWhiteSpace(Descricao) &&
                   CargaHoraria > TimeSpan.Zero &&
                   !string.IsNullOrWhiteSpace(Instrutor);
        }
    }
}
