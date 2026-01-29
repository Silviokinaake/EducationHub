using EducationHub.Core.DomainObjects;

namespace EducationHub.Alunos.Domain.Entidades
{
    public class HistoricoAprendizado
    {
        public Guid Id { get; private set; }
        public Guid CursoId { get; private set; }
        public double ProgressoPercentual { get; private set; }
        public DateTime DataUltimaAtualizacao { get; private set; }

        protected HistoricoAprendizado() { }

        public HistoricoAprendizado(Guid cursoId, double progressoPercentual)
        {
            Id = Guid.NewGuid();
            CursoId = cursoId;
            AtualizarProgresso(progressoPercentual);
        }

        public void AtualizarProgresso(double percentual)
        {
            Validacoes.ValidarMinimoMaximo(percentual, 0, 100, "Percentual inválido (0-100).");
            ProgressoPercentual = percentual;
            DataUltimaAtualizacao = DateTime.UtcNow;
        }

        public override bool Equals(object obj)
        {
            if (obj is not HistoricoAprendizado other) return false;
            return CursoId == other.CursoId;
        }

        public override int GetHashCode() => CursoId.GetHashCode();
    }
}
