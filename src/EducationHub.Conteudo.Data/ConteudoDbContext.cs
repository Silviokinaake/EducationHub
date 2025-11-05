using EducationHub.Conteudo.Domain.Entidades;
using EducationHub.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace EducationHub.Conteudo.Data
{
    public class ConteudoDbContext : DbContext, IUnitOfWork
    {
        public ConteudoDbContext(DbContextOptions<ConteudoDbContext> options)
            : base(options)
        {
        }

        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Aula> Aulas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define padrão de colunas string como varchar(100)
            foreach (var property in modelBuilder.Model
                         .GetEntityTypes()
                         .SelectMany(e => e.GetProperties())
                         .Where(p => p.ClrType == typeof(string)))
            {
                property.SetColumnType("varchar(100)");
            }

            // Aplica configurações de mapeamento (EntityTypeConfiguration)
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ConteudoDbContext).Assembly);

            // Define comportamento de deleção em cascata (boa prática)
            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                         .SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;
            }

            base.OnModelCreating(modelBuilder);
        }

        public async Task<bool> Commit()
        {
            // Atualiza DataCadastro apenas em novas entidades
            foreach (var entry in ChangeTracker.Entries()
                         .Where(entry => entry.Entity.GetType().GetProperty("DataCadastro") != null))
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
