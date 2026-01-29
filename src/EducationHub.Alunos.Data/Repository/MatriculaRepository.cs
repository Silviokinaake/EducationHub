using EducationHub.Alunos.Domain.Entidades;
using EducationHub.Alunos.Domain.Interfaces;
using EducationHub.Core.Data;

namespace EducationHub.Alunos.Data.Repository
{
    public class MatriculaRepository(AlunoDbContext context) : IMatriculaRepository
    {
        private readonly AlunoDbContext _context = context;

        public IUnitOfWork UnitOfWork => _context;

        public async Task<Matricula> ObterPorId(Guid id)
        {
            return await _context.Matriculas.FindAsync(id);
        }

        public async Task AdicionarAsync(Matricula matricula)
        {
            await _context.Matriculas.AddAsync(matricula);
        }

        public void Atualizar(Matricula matricula)
        {
            _context.Matriculas.Update(matricula);
        }
        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
