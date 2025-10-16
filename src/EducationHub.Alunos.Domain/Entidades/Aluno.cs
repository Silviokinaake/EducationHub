using EducationHub.Core.DomainObjects;

namespace EducationHub.Alunos.Domain.Entidades
{
    public class Aluno : Entity, IAggregateRoot
    {
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public Cpf Cpf { get; private set; }
        public DateTime DataCadastro { get; private set; }

        public ICollection<Matricula> Matriculas { get; private set; }
        public ICollection<Certificado> Certificados { get; private set; }

        protected Aluno() { }

        public Aluno(string nome, string email, Cpf cpf)
        {
            Nome = nome;
            Email = email;
            Cpf = cpf ?? throw new DomainException("CPF inválido.");
            DataCadastro = DateTime.UtcNow;
            Matriculas = new List<Matricula>();
            Certificados = new List<Certificado>();

            Validar();
        }

        private void Validar()
        {
            Validacoes.ValidarSeVazio(Nome, "O campo Nome não pode estar vazio.");
            Validacoes.ValidarTamanho(Nome, 3, 200, "O campo Nome deve ter entre 3 e 200 caracteres.");

            Validacoes.ValidarSeVazio(Email, "O campo Email não pode estar vazio.");
            if (!Email.Contains("@") || !Email.Contains("."))
                throw new DomainException("O email informado é inválido.");
        }


        public Matricula Matricular(Guid cursoId, decimal valor, DateTime dataMatricula)
        {
            var matricula = new Matricula(cursoId, Id, valor, dataMatricula);
            Matriculas.Add(matricula);
            return matricula;
        }

        public void AtivarMatricula(Guid matriculaId)
        {
            var matricula = Matriculas.FirstOrDefault(m => m.Id == matriculaId)
                ?? throw new DomainException("Matrícula não encontrada.");

            matricula.Ativar();
        }

        public void RegistrarProgresso(Guid matriculaId, Guid aulaId, double progressoPercentual)
        {
            var matricula = Matriculas.FirstOrDefault(m => m.Id == matriculaId)
                ?? throw new DomainException("Matrícula não encontrada.");

            matricula.RegistrarProgresso(aulaId, progressoPercentual);
        }

        public Certificado FinalizarCurso(Guid matriculaId, string tituloCurso)
        {
            var matricula = Matriculas.FirstOrDefault(m => m.Id == matriculaId)
                ?? throw new DomainException("Matrícula não encontrada.");

            if (!matricula.PodeFinalizar())
                throw new DomainException("Não é possível finalizar: requisitos não atendidos.");

            matricula.Concluir();

            var certificado = new Certificado(this.Id, matricula.CursoId, tituloCurso, DateTime.UtcNow);
            Certificados.Add(certificado);

            return certificado;
        }

    }
}

