using System;
using EducationHub.Core.DomainObjects;

namespace EducationHub.Alunos.Domain.Entidades
{
    public class Aluno : Entity, IAggregateRoot
    {
        public Guid UsuarioId { get; private set; }
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public DateTime DataNascimento { get; private set; }

        private readonly List<Matricula> _matriculas = new();
        public IReadOnlyCollection<Matricula> Matriculas => _matriculas.AsReadOnly();

        private readonly List<Certificado> _certificados = new();
        public IReadOnlyCollection<Certificado> Certificados => _certificados.AsReadOnly();

        protected Aluno() { }

        public Aluno(Guid id, Guid usuarioId, string nome, string email, DateTime dataNascimento)
        {
            Id = id;
            UsuarioId = usuarioId;
            SetNome(nome);
            SetEmail(email);
            DataNascimento = dataNascimento;

            Validar();
        }

        public void SetNome(string nome)
        {
            Validacoes.ValidarSeVazio(nome, "O nome do aluno não pode estar vazio.");
            Validacoes.ValidarTamanho(nome, 3, 150, "O nome do aluno deve ter entre 3 e 150 caracteres.");
            Nome = nome.Trim();
        }

        public void SetEmail(string email)
        {
            Validacoes.ValidarSeVazio(email, "O email é obrigatório.");
            Validacoes.ValidarTamanho(email, 5, 150, "O email deve ter entre 5 e 150 caracteres.");

            var emailPattern = @"^[\w\.\-]+@([\w\-]+\.)+[\w\-]{2,}$";
            Validacoes.ValidarSeDiferente(emailPattern, email, "O email informado é inválido.");

            Email = email.Trim();
        }

        private void Validar()
        {
            Validacoes.ValidarSeIgual(UsuarioId, Guid.Empty, "UsuarioId inválido.");
            Validacoes.ValidarSeVazio(Nome, "O nome do aluno não pode estar vazio.");
            Validacoes.ValidarTamanho(Nome, 3, 150, "O nome do aluno deve ter entre 3 e 150 caracteres.");
            Validacoes.ValidarSeVazio(Email, "O email é obrigatório.");
            Validacoes.ValidarTamanho(Email, 5, 150, "O email deve ter entre 5 e 150 caracteres.");

            if (DataNascimento == default)
                throw new DomainException("Data de nascimento inválida.");

            var idade = (DateTime.UtcNow - DataNascimento).TotalDays / 365.25;
            Validacoes.ValidarSeMenorQue(idade, 10, "O aluno deve ter pelo menos 10 anos.");
        }

        public void GerarCertificado(Guid cursoId, DateTime dataConclusao, string tituloCurso = "Curso Concluído")
        {
            Validacoes.ValidarSeIgual(cursoId, Guid.Empty, "CursoId inválido.");
            
            // Verificar se já existe certificado para este curso
            if (_certificados.Any(c => c.CursoId == cursoId))
                throw new DomainException("Já existe um certificado para este curso.");

            var certificado = new Certificado(Id, cursoId, tituloCurso, dataConclusao);
            _certificados.Add(certificado);
        }
    }
}

