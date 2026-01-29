using EducationHub.Conteudo.Domain.Entidades;
using EducationHub.Conteudo.Domain.Interfaces;
using EducationHub.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace EducationHub.Conteudo.Data.Repository
{
    public class CursoRepository : ICursoRepositorio
    {
        private readonly ConteudoDbContext _context;

        public CursoRepository(ConteudoDbContext context)
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

        public Task<Curso> ObterPorId(Guid id)
        {
            return _context.Cursos
                .Include(c => c.Aulas)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public void Adicionar(Curso curso)
        {
            _context.Cursos.Add(curso);
        }

        public void Atualizar(Curso curso)
        {
            _context.Cursos.Update(curso);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
