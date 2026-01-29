using EducationHub.Alunos.Domain.Enums;
using EducationHub.Alunos.Domain.Events;
using EducationHub.Core.DomainObjects;

namespace EducationHub.Alunos.Domain.Entidades
{
    public class Matricula : Entity, IAggregateRoot
    {
        public Guid CursoId { get; private set; }
        public Guid AlunoId { get; private set; }
        public decimal Valor { get; private set; }
        public DateTime DataMatricula { get; private set; }
        public DateTime? DataAtivacao { get; private set; }
        public DateTime? DataConclusao { get; private set; }
        public StatusMatriculaEnum Status { get; private set; }

        private readonly List<HistoricoAprendizado> _historico = new();
        public IReadOnlyCollection<HistoricoAprendizado> Historico => _historico.AsReadOnly();

        protected Matricula() { }

        public Matricula(Guid cursoId, Guid alunoId, decimal valor, DateTime dataMatricula)
        {
            CursoId = cursoId;
            AlunoId = alunoId;
            Valor = valor;
            DataMatricula = dataMatricula;
            Status = StatusMatriculaEnum.Pendente;

            Validar();
        }

        private void Validar()
        {
            Validacoes.ValidarSeIgual(CursoId, Guid.Empty, "CursoId inválido.");
            Validacoes.ValidarSeIgual(AlunoId, Guid.Empty, "AlunoId inválido.");
            Validacoes.ValidarMinimoMaximo(Valor, 0, decimal.MaxValue, "Valor da matrícula inválido.");
            Validacoes.ValidarSeMenorQue((long)DataMatricula.Ticks, 0, "Data de matrícula inválida."); // simples; opcional
        }

        public void Ativar()
        {
            if (Status == StatusMatriculaEnum.Ativa)
                return;

            if (Status == StatusMatriculaEnum.Concluida || Status == StatusMatriculaEnum.Cancelada)
                throw new DomainException("Não é possível ativar uma matrícula finalizada ou cancelada.");

            Status = StatusMatriculaEnum.Ativa;
            DataAtivacao = DateTime.UtcNow;
            
            // Publicar evento de domínio
            AdicionarEvento(new MatriculaConfirmadaEvent(Id, AlunoId, CursoId));
        }

        public void Concluir()
        {
            if (Status != StatusMatriculaEnum.Ativa)
                throw new DomainException("Somente matrículas ativas podem ser concluídas.");

            Status = StatusMatriculaEnum.Concluida;
            DataConclusao = DateTime.UtcNow;
            
            // Publicar evento de domínio
            AdicionarEvento(new CursoConcluidoEvent(Id, AlunoId, CursoId));
        }

        public void Cancelar()
        {
            if (Status == StatusMatriculaEnum.Concluida)
                throw new DomainException("Não é possível cancelar uma matrícula concluída.");

            Status = StatusMatriculaEnum.Cancelada;
        }

        public void RegistrarProgresso(Guid cursoId, double percentual)
        {
            Validacoes.ValidarSeIgual(cursoId, Guid.Empty, "Id da aula inválido.");
            Validacoes.ValidarMinimoMaximo(percentual, 0, 100, "Percentual inválido (0-100).");

            var existente = _historico.FirstOrDefault(h => h.CursoId == cursoId);
            if (existente is null)
            {
                _historico.Add(new HistoricoAprendizado(cursoId, percentual));
            }
            else
            {
                existente.AtualizarProgresso(percentual);
            }
        }

        public bool PodeFinalizar()
        {
            if (Status != StatusMatriculaEnum.Ativa) return false;
            if (!_historico.Any()) return false;
            return _historico.All(h => h.ProgressoPercentual >= 100);
        }
    }
}
