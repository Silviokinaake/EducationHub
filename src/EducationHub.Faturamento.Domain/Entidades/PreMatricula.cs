using EducationHub.Conteudo.Domain.Entidades;

namespace EducationHub.Faturamento.Domain.Entidades
{
    public class PreMatricula
    {
        public Guid Id { get; set; }
        public decimal Valor { get; set; }
        public List<Curso> Cursos { get; set; }
    }
}
