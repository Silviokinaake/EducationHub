using EducationHub.Conteudo.Domain.Entidades;
using EducationHub.Conteudo.Domain.Interfaces;
using EducationHub.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace EducationHub.Conteudo.Data.Repository
{
    public class AulaRepository : IAulaRepositorio
    {
        private readonly ConteudoDbContext _context;

        public AulaRepository(ConteudoDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<IEnumerable<Aula>> ObterTodosAsync()
        {
            return await _context.Aulas
                .AsNoTracking()
                .OrderBy(a => a.Titulo)
                .ToListAsync();
        }

        public async Task<IEnumerable<Aula>> ObterTodosAsync(Guid cursoId)
        {
            return await _context.Aulas
                .AsNoTracking()
                .Where(a => a.CursoId == cursoId)
                .OrderBy(a => a.Titulo)
                .ToListAsync();
        }

        public async Task<Aula?> ObterPorIdAsync(Guid id)
        {
            return await _context.Aulas
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task AdicionarAsync(Aula aula)
        {
            if (aula == null)
                throw new ArgumentNullException(nameof(aula));

            await _context.Aulas.AddAsync(aula);
        }

        public async Task AtualizarAsync(Aula aula)
        {
            if (aula == null)
                throw new ArgumentNullException(nameof(aula));

            _context.Aulas.Update(aula);
            await Task.CompletedTask;
        }

        public async Task RemoverAsync(Guid id)
        {
            var aula = await _context.Aulas.FindAsync(id);
            if (aula != null)
            {
                _context.Aulas.Remove(aula);
            }
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
