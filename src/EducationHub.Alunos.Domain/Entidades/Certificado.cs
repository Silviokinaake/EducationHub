using EducationHub.Core.DomainObjects;

namespace EducationHub.Alunos.Domain.Entidades
{
    public class Certificado: Entity, IAggregateRoot
    {
        public Guid AlunoId { get; private set; }
        public Guid CursoId { get; private set; }
        public string TituloCurso { get; private set; }
        public DateTime DataEmissao { get; private set; }
        public string Codigo { get; private set; }

        protected Certificado() { }

        public Certificado(Guid alunoId, Guid cursoId, string tituloCurso, DateTime dataEmissao)
        {
            AlunoId = alunoId;
            CursoId = cursoId;
            TituloCurso = tituloCurso;
            DataEmissao = dataEmissao;
            Codigo = GerarCodigo();

            Validar();
        }

        private void Validar()
        {
            Validacoes.ValidarSeIgual(AlunoId, Guid.Empty, "AlunoId inválido.");
            Validacoes.ValidarSeIgual(CursoId, Guid.Empty, "CursoId inválido.");
            Validacoes.ValidarSeVazio(TituloCurso, "O título do curso é obrigatório.");
            Validacoes.ValidarTamanho(TituloCurso, 3, 200, "O título do curso deve ter entre 3 e 200 caracteres.");
        }

        private string GerarCodigo()
        {
            return $"{DataEmissao:yyyyMMddHHmmss}-{Id.ToString().Substring(0, 8).ToUpper()}";
        }

        public override string ToString()
        {
            return $"{TituloCurso} - Emitido em {DataEmissao:dd/MM/yyyy} (Código: {Codigo})";
        }
    }
}
