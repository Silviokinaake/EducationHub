using EducationHub.Alunos.Domain.Entidades;
using EducationHub.Alunos.Domain.Interfaces;
using EducationHub.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace EducationHub.Alunos.Data.Repository
{
    public class CertificadoRepository(AlunoDbContext context) : ICertificadoRepository
    {
        private readonly AlunoDbContext _context = context;

        public IUnitOfWork UnitOfWork => _context;

        public async Task<Certificado> ObterPorId(Guid id)
        {
            return await _context.Certificados.FindAsync(id);
        }

        public async Task<Certificado> ObterPorMatriculaId(Guid matriculaId)
        {
            var matricula = await _context.Matriculas.FindAsync(matriculaId);
            if (matricula == null) return null;

            return await _context.Certificados
                .FirstOrDefaultAsync(c => c.AlunoId == matricula.AlunoId && c.CursoId == matricula.CursoId);
        }

        public async Task<IEnumerable<Certificado>> ObterPorAlunoId(Guid alunoId)
        {
            return await _context.Certificados
                .Where(c => c.AlunoId == alunoId)
                .ToListAsync();
        }

        public async Task AdicionarAsync(Certificado certificado)
        {
            await _context.Certificados.AddAsync(certificado);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
