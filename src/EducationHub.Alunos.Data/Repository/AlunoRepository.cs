using EducationHub.Alunos.Domain.Entidades;
using EducationHub.Alunos.Domain.Repositorio;
using EducationHub.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace EducationHub.Alunos.Data.Repository
{
    public class AlunoRepository : IAlunoRepositorio, IDisposable
    {
        private readonly AlunoDbContext _context;

        public AlunoRepository(AlunoDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<Aluno> ObterPorIdAsync(Guid id)
        {
            return await _context.Alunos
                .Include(a => a.Matriculas)
                    .ThenInclude(m => m.Historico)
                .Include(a => a.Certificados)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Aluno> ObterPorEmailAsync(string email)
        {
            return await _context.Alunos
                .AsNoTracking()
                .Include(a => a.Matriculas)
                .Include(a => a.Certificados)
                .FirstOrDefaultAsync(a => a.Email == email);
        }

        public async Task<IEnumerable<Aluno>> ObterTodosAsync()
        {
            return await _context.Alunos
                .AsNoTracking()
                .Include(a => a.Matriculas)
                .Include(a => a.Certificados)
                .ToListAsync();
        }

        public async Task AdicionarAsync(Aluno aluno)
        {
            await _context.Alunos.AddAsync(aluno);
        }

        public async Task AtualizarAsync(Aluno aluno)
        {
            _context.Alunos.Update(aluno);
            await Task.CompletedTask;
        }

        public async Task<Matricula> ObterMatriculaPorIdAsync(Guid matriculaId)
        {
            return await _context.Matriculas
                .Include(m => m.Historico)
                .FirstOrDefaultAsync(m => m.Id == matriculaId);
        }

        public async Task<IEnumerable<Matricula>> ObterMatriculasPorAlunoAsync(Guid alunoId)
        {
            return await _context.Matriculas
                .AsNoTracking()
                .Where(m => m.AlunoId == alunoId)
                .Include(m => m.Historico)
                .ToListAsync();
        }

        public async Task<IEnumerable<Certificado>> ObterCertificadosPorAlunoAsync(Guid alunoId)
        {
            return await _context.Certificados
                .AsNoTracking()
                .Where(c => c.AlunoId == alunoId)
                .ToListAsync();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
