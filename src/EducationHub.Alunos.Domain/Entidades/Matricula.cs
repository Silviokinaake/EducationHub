using EducationHub.Alunos.Domain.Enums;
using EducationHub.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EducationHub.Alunos.Domain.Entidades
{
    public class Matricula : Entity
    {
        public Guid CursoId { get; private set; }
        public Guid AlunoId { get; private set; }
        public decimal Valor { get; private set; }
        public DateTime DataMatricula { get; private set; }
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
        }

        public void Concluir()
        {
            if (Status != StatusMatriculaEnum.Ativa)
                throw new DomainException("Somente matrículas ativas podem ser concluídas.");

            Status = StatusMatriculaEnum.Concluida;
        }

        public void Cancelar()
        {
            if (Status == StatusMatriculaEnum.Concluida)
                throw new DomainException("Não é possível cancelar uma matrícula concluída.");

            Status = StatusMatriculaEnum.Cancelada;
        }

        public void RegistrarProgresso(Guid aulaId, double percentual)
        {
            Validacoes.ValidarSeIgual(aulaId, Guid.Empty, "Id da aula inválido.");
            Validacoes.ValidarMinimoMaximo(percentual, 0, 100, "Percentual inválido (0-100).");

            var existente = _historico.FirstOrDefault(h => h.AulaId == aulaId);
            if (existente is null)
            {
                _historico.Add(new HistoricoAprendizado(aulaId, percentual));
            }
            else
            {
                existente.AtualizarProgresso(percentual);
            }
        }

        // regra simples para permitir finalização: todas as aulas com >= 100% ou progresso médio >= 100
        // Como não temos número de aulas aqui, assumimos: se existir ao menos uma entrada e todas >=100
        public bool PodeFinalizar()
        {
            if (Status != StatusMatriculaEnum.Ativa) return false;
            if (!_historico.Any()) return false;
            return _historico.All(h => h.ProgressoPercentual >= 100);
        }
    }
}
