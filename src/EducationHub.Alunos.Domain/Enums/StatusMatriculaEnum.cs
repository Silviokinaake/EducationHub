using System.ComponentModel;

namespace EducationHub.Alunos.Domain.Enums
{
    public enum StatusMatriculaEnum
    {
        [Description("Pendente de pagamento")]
        Pendente = 0,

        [Description("Pagamento realizado")]
        Ativa = 1,

        [Description("Curso concluído")]
        Concluida = 2,

        [Description("Matricula inativada")]
        Cancelada = 4
    }
}
