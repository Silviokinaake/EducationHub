using EducationHub.Core.DomainObjects;
using EducationHub.Conteudo.Domain.Enums;

namespace EducationHub.Conteudo.Domain.Entidades
{
    public class Curso : Entity, IAggregateRoot
    {
        public string Titulo { get; private set; }
        public string Descricao { get; private set; }
        public TimeSpan CargaHoraria { get; private set; }
        public string Instrutor { get; private set; }
        public SituacaoCurso Situacao { get; private set; }
        public string Nivel { get; private set; }
        public decimal Valor { get; private set; }

        public ConteudoProgramatico ConteudoProgramatico { get; private set; }
        public ICollection<Aula> Aulas { get; private set; }

        protected Curso() { }

        public Curso(string titulo, string descricao, TimeSpan cargaHoraria, string Intrutor, string nivel, decimal valor, ConteudoProgramatico conteudoProgramatico)
        {
            Titulo = titulo;
            Descricao = descricao;
            CargaHoraria = cargaHoraria;
            Instrutor = Intrutor;
            Nivel = nivel;
            Valor = valor;
            ConteudoProgramatico = conteudoProgramatico;
            Situacao = SituacaoCurso.Ativo;
            Aulas = new List<Aula>();

            Validar();
        }

        public void AdicionarAula(Aula aula)
        {
            Validacoes.ValidarSeNulo(aula, "A aula não pode ser nula.");
            ((List<Aula>)Aulas).Add(aula);
        }

        public void AtualizarDescricao(string novaDescricao)
        {
            Validacoes.ValidarSeVazio(novaDescricao, "A descrição não pode estar vazia.");
            Validacoes.ValidarTamanho(novaDescricao, 10, 500, "A descrição deve ter entre 10 e 500 caracteres.");
            Descricao = novaDescricao;
        }

        public void Desativar() => Situacao = SituacaoCurso.Inativo;
        public void Ativar() => Situacao = SituacaoCurso.Ativo;
        public void Inativar() => Situacao = SituacaoCurso.Inativo;

        public void AtualizarInformacoes(string titulo, string descricao, TimeSpan cargaHoraria, string instrutor, string nivel, decimal valor, ConteudoProgramatico conteudoProgramatico)
        {
            Titulo = titulo;
            Descricao = descricao;
            CargaHoraria = cargaHoraria;
            Instrutor = instrutor;
            Nivel = nivel;
            Valor = valor;
            ConteudoProgramatico = conteudoProgramatico;
            Validar();
        }

        private void Validar()
        {
            Validacoes.ValidarSeVazio(Titulo, "O campo Título não pode estar vazio.");
            Validacoes.ValidarTamanho(Titulo, 3, 250, "O campo Título deve ter entre 3 e 250 caracteres.");

            Validacoes.ValidarSeVazio(Descricao, "O campo Descrição não pode estar vazio.");
            Validacoes.ValidarTamanho(Descricao, 10, 500, "O campo Descrição deve ter entre 10 e 500 caracteres.");

            Validacoes.ValidarSeVazio(Instrutor, "O campo Instrutor não pode estar vazio.");
            Validacoes.ValidarTamanho(Instrutor, 3, 100, "O campo Instrutor deve ter entre 3 e 100 caracteres.");

            Validacoes.ValidarSeVazio(Nivel, "O campo Nível não pode estar vazio.");
            Validacoes.ValidarTamanho(Nivel, 3, 50, "O campo Nível deve ter entre 3 e 50 caracteres.");

            Validacoes.ValidarSeNulo(ConteudoProgramatico, "O conteúdo programático é obrigatório.");

            Validacoes.ValidarSeMenorQue(CargaHoraria.TotalMinutes, 30, "A carga horária mínima de um curso deve ser de 30 minutos.");
            
            Validacoes.ValidarSeMenorQue(Valor, 0, "O valor do curso não pode ser negativo.");
        }
    }
}
