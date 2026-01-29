using EducationHub.Alunos.Domain.Entidades;
using EducationHub.Core.Data;

namespace EducationHub.Alunos.Domain.Interfaces
{
    public interface ICertificadoRepository : IRepository<Certificado>
    {
        Task AdicionarAsync(Certificado certificado);
        Task<Certificado> ObterPorId(Guid id);
        Task<Certificado> ObterPorMatriculaId(Guid matriculaId);
        Task<IEnumerable<Certificado>> ObterPorAlunoId(Guid alunoId);
    }
}
