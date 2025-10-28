using EducationHub.Core.Data;
using EducationHub.Faturamento.Domain.Entidades;
using EducationHub.Faturamento.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EducationHub.Faturamento.Data.Repository
{
    public class PagamentoRepository : IPagamentoRepositorio, IDisposable
    {
        private readonly FaturamentoDbContext _context;

        public PagamentoRepository(FaturamentoDbContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<IEnumerable<Pagamento>> ObterPagamentosPorAlunoAsync(Guid alunoId)
        {
            return await _context.Pagamentos
                .AsNoTracking()
                .Where(p => p.AlunoId == alunoId)
                .ToListAsync();
        }

        public async Task<Pagamento?> ObterPorIdAsync(Guid id)
        {
            return await _context.Pagamentos
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AdicionarAsync(Pagamento pagamento)
        {
            await _context.Pagamentos.AddAsync(pagamento);
        }

        public async Task AtualizarAsync(Pagamento pagamento)
        {
            _context.Pagamentos.Update(pagamento);
            await Task.CompletedTask;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
