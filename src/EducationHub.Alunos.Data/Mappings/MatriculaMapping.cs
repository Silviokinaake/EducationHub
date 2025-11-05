using EducationHub.Alunos.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationHub.Alunos.Data.Mappings
{
    public class MatriculaMapping : IEntityTypeConfiguration<Matricula>
    {
        public void Configure(EntityTypeBuilder<Matricula> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.CursoId)
                .IsRequired();

            builder.Property(m => m.AlunoId)
                .IsRequired();

            builder.Property(m => m.Valor)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(m => m.DataMatricula)
                .IsRequired()
                .HasColumnType("datetime2");

            builder.Property(m => m.Status)
                .IsRequired();

            builder.HasIndex(m => m.AlunoId);
            builder.HasIndex(m => m.CursoId);

            builder.OwnsMany(
                m => m.Historico,
                ha =>
                {
                    ha.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    ha.HasKey("Id");

                    ha.WithOwner().HasForeignKey("MatriculaId");

                    ha.Property(h => h.CursoId)
                        .IsRequired();

                    ha.Property(h => h.ProgressoPercentual)
                        .IsRequired();

                    ha.Property(h => h.DataUltimaAtualizacao)
                        .IsRequired()
                        .HasColumnType("datetime2");

                    ha.ToTable("MatriculaHistoricos");
                });

            builder.HasOne<Domain.Entidades.Aluno>()
                .WithMany(a => a.Matriculas)
                .HasForeignKey("AlunoId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Matriculas");
        }
    }
}
