using EducationHub.Alunos.Domain.Entidades;
using EducationHub.Core.Data;
using EducationHub.Core.DomainObjects;
using EducationHub.Core.Mediator;
using EducationHub.Core.Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;


namespace EducationHub.Alunos.Data
{
    public class AlunoDbContext : DbContext, IUnitOfWork
    {
        private readonly IMediatorHandler _mediatorHandler;

        public AlunoDbContext(DbContextOptions<AlunoDbContext> options, IMediatorHandler mediatorHandler)
            : base(options) 
        {
            _mediatorHandler = mediatorHandler;
        }

        public DbSet<Aluno> Alunos { get; set; }
        public DbSet<Matricula> Matriculas { get; set; }
        public DbSet<Certificado> Certificados { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Ignore a classe base abstrata Event - ela não deve ser mapeada
            modelBuilder.Ignore<EducationHub.Core.Messages.Event>();
            
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

            var sucesso = await base.SaveChangesAsync() > 0;
            if (sucesso) await _mediatorHandler.PublicarEventos(this);

            return sucesso;
        }
    }
}