using EducationHub.Conteudo.Domain.Entidades;
using EducationHub.Alunos.Domain.Entidades;
using EducationHub.Faturamento.Domain.Entidades;
using EducationHub.Conteudo.Data.Mappings;
using EducationHub.Alunos.Data.Mappings;
using EducationHub.Faturamento.Data.Mappings;
using EducationHub.Core.Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace EducationHub.API.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Aula> Aulas { get; set; }

        public DbSet<Aluno> Alunos { get; set; }
        public DbSet<Matricula> Matriculas { get; set; }
        public DbSet<Certificado> Certificados { get; set; }

        public DbSet<Pagamento> Pagamentos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Ignorar tipos que não são entidades (classes de mensagens do MediatR)
            modelBuilder.Ignore<Event>();
            modelBuilder.Ignore<Message>();
            modelBuilder.Ignore<Command>();
            
            // Apply configurations from current assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            
            // Apply configurations from Data layer assemblies
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CursoMapping).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AlunoMapping).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PagamentoMapping).Assembly);

            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
            {
                if (string.IsNullOrEmpty(property.GetColumnType()) && !property.GetMaxLength().HasValue)
                    property.SetColumnType("varchar(200)");
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}

