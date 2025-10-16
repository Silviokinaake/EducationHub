using EducationHub.Conteudo.Domain.Entidades;
using EducationHub.Conteudo.Domain.Repositorios;
using EducationHub.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace EducationHub.Conteudo.Data.Repository
{
    public class CursoRepository : ICursoRepositorio
    {
        private readonly ConteudoContext _context;

        public CursoRepository(ConteudoContext context)
        {
            _context = context;
        }
        public IUnitOfWork UnitOfWork => _context;

        public async Task<IEnumerable<Curso>> ObterTodosAsync()
        {
            return await _context.Cursos
                .Include(c => c.Aulas)
                .ToListAsync();
        }

        public async Task<Curso> ObterPorIdAsync(Guid id)
        {
            return await _context.Cursos
                .Include(c => c.Aulas)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AdicionarAsync(Curso curso)
        {
            await _context.Cursos.AddAsync(curso);
        }

        public async Task AtualizarAsync(Curso curso)
        {
            _context.Cursos.Update(curso);
            await Task.CompletedTask;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
