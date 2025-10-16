using EducationHub.Core.Data;
using EducationHub.Faturamento.Domain.Entidades;
using EducationHub.Faturamento.Domain.Repositorios;
using Microsoft.EntityFrameworkCore;

namespace EducationHub.Faturamento.Data.Repository
{
    public class PagamentoRepository : IPagamentoRepositorio, IDisposable
    {
        private readonly FaturamentoContext _context;

        public PagamentoRepository(FaturamentoContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<IEnumerable<Pagamento>> ObterPagamentosPorAlunoAsync(Guid alunoId)
        {
            return await _context.Pagamento
                .AsNoTracking()
                .Where(p => p.AlunoId == alunoId)
                .ToListAsync();
        }

        public async Task AdicionarAsync(Pagamento pagamento)
        {
            await _context.Pagamento.AddAsync(pagamento);
        }

        public async Task AtualizarAsync(Pagamento pagamento)
        {
            _context.Pagamento.Update(pagamento);
            await Task.CompletedTask;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
