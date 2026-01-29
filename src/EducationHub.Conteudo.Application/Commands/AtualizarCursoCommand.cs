using EducationHub.Core.Messages;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EducationHub.Conteudo.Application.Commands
{
    public class AtualizarCursoCommand : Command, IRequest<bool>
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

        public AtualizarCursoCommand()
        {
            Titulo = string.Empty;
            Descricao = string.Empty;
            Instrutor = string.Empty;
            Nivel = string.Empty;
            ConteudoProgramatico = new ConteudoProgramaticoCommand();
        }

        public override bool EhValido()
        {
            return Id != Guid.Empty &&
                   !string.IsNullOrWhiteSpace(Titulo) &&
                   !string.IsNullOrWhiteSpace(Descricao);
        }
    }
}
