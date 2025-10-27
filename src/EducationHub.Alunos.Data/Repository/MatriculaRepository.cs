using EducationHub.Alunos.Domain.Entidades;
using EducationHub.Alunos.Domain.Interfaces;

namespace EducationHub.Alunos.Data.Repository
{
    public class MatriculaRepository(AlunoDbContext context) : IMatriculaRepository
    {
        private readonly AlunoDbContext _context = context;
        public async Task AdicionarAsync(Matricula mtricula)
        {
            await _context.Matriculas.AddAsync(mtricula);
        }

        public void AtualizarAsync(Matricula mtricula)
        {
            _context.Matriculas.Update(mtricula);
        }
        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
}
