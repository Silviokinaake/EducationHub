using EducationHub.Alunos.Domain.Entidades;
using EducationHub.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;


namespace EducationHub.Alunos.Data
{
    public class AlunoDbContext : DbContext, IUnitOfWork
    {
        public AlunoDbContext(DbContextOptions<AlunoDbContext> options)
            : base(options) { }

        public DbSet<Aluno> Alunos { get; set; }
        public DbSet<Matricula> Matriculas { get; set; }
        public DbSet<Certificado> Certificados { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (Database.IsSqlServer())
            {
                foreach (var property in modelBuilder.Model
                             .GetEntityTypes()
                             .SelectMany(e => e.GetProperties())
                             .Where(p => p.ClrType == typeof(string)))
                {
                    property.SetColumnType("varchar(100)");
                }
            }

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AlunoDbContext).Assembly);
        }

        public async Task<bool> Commit()
        {
            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("DataCadastro") != null))
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("DataCadastro").CurrentValue = DateTime.Now;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property("DataCadastro").IsModified = false;
                }
            }

            return await base.SaveChangesAsync() > 0;
        }
    }
}