using EducationHub.Alunos.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationHub.Alunos.Data.Mappings
{
    public class AlunoMapping : IEntityTypeConfiguration<Aluno>
    {
        public void Configure(EntityTypeBuilder<Aluno> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.UsuarioId)
                .IsRequired();

            builder.Property(a => a.Nome)
                .IsRequired()
                .HasColumnType("varchar(150)")
                .HasMaxLength(150);

            builder.Property(a => a.Email)
                .IsRequired()
                .HasColumnType("varchar(150)")
                .HasMaxLength(150);

            builder.Property(a => a.DataNascimento)
                .IsRequired()
                .HasColumnType("datetime2");

            builder.HasIndex(a => a.Email).IsUnique();

            builder.HasMany(a => a.Matriculas)
                .WithOne()
                .HasForeignKey("AlunoId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(a => a.Certificados)
                .WithOne()
                .HasForeignKey("AlunoId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Alunos");
        }
    }
}
