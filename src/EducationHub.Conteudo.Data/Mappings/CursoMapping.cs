using EducationHub.Conteudo.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationHub.Conteudo.Data.Mappings
{
    public class CursoMapping : IEntityTypeConfiguration<Curso>
    {
        public void Configure(EntityTypeBuilder<Curso> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Titulo)
                .IsRequired()
                .HasColumnType("varchar(250)")
                .HasMaxLength(250);

            builder.Property(c => c.Descricao)
                .IsRequired()
                .HasColumnType("varchar(500)")
                .HasMaxLength(500);

            builder.Property(c => c.CargaHoraria)
                .IsRequired();

            builder.Property(c => c.Instrutor)
                .IsRequired()
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            builder.Property(c => c.Situacao)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(c => c.Nivel)
                .IsRequired()
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.OwnsOne(c => c.ConteudoProgramatico, cp =>
            {
                cp.Property(p => p.Objetivo)
                    .IsRequired()
                    .HasColumnType("varchar(250)")
                    .HasMaxLength(250);

                cp.Property(p => p.Conteudo)
                    .IsRequired()
                    .HasColumnType("varchar(1000)")
                    .HasMaxLength(1000);

                cp.Property(p => p.Metodologia)
                    .IsRequired()
                    .HasColumnType("varchar(500)")
                    .HasMaxLength(500);

                cp.Property(p => p.Bibliografia)
                    .IsRequired()
                    .HasColumnType("varchar(500)")
                    .HasMaxLength(500);
            });

            builder.HasMany(c => c.Aulas)
                .WithOne(a => a.Curso)
                .HasForeignKey(a => a.CursoId);

            builder.ToTable("Cursos");
        }
    }
}
