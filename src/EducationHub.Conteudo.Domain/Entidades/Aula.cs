using EducationHub.Core.DomainObjects;

namespace EducationHub.Conteudo.Domain.Entidades
{
    public class Aula : Entity
    {
        public Guid CursoId { get; private set; }
        public string Titulo { get; private set; }
        public string ConteudoAula { get; private set; }
        public string MaterialDeApoio { get; private set; }
        public TimeSpan Duracao { get; private set; }
        public Curso Curso { get; private set; }

        protected Aula() { }

        public Aula(string titulo, string conteudoAula, string materialDeApoio, TimeSpan duracao, Guid cursoId)
        {
            Titulo = titulo;
            ConteudoAula = conteudoAula;
            MaterialDeApoio = materialDeApoio;
            Duracao = duracao;
            CursoId = cursoId;

            Validar();
        }

        public void AtualizarConteudo(string novoConteudo)
        {
            Validacoes.ValidarSeVazio(novoConteudo, "O conteúdo da aula não pode estar vazio.");
            ConteudoAula = novoConteudo;
        }

        private void Validar()
        {
            Validacoes.ValidarSeVazio(Titulo, "O campo Título não pode estar vazio.");
            Validacoes.ValidarTamanho(Titulo, 3, 150, "O campo Título deve ter entre 3 e 150 caracteres.");
            Validacoes.ValidarSeVazio(ConteudoAula, "O campo Conteúdo da aula não pode estar vazio.");
            Validacoes.ValidarTamanho(ConteudoAula, 10, 5000, "O campo Conteúdo deve ter entre 10 e 1000 caracteres.");
            if (!string.IsNullOrWhiteSpace(MaterialDeApoio))
            {
                Validacoes.ValidarTamanho(MaterialDeApoio, 5, 1000, "O campo Material de Apoio deve ter entre 5 e 1000 caracteres.");
            }
            Validacoes.ValidarSeMenorQue(Duracao.TotalMinutes, 1, "A duração da aula deve ser de pelo menos 1 minuto.");
            Validacoes.ValidarSeIgual(CursoId, Guid.Empty, "O campo CursoId é obrigatório.");
        }
    }
}
