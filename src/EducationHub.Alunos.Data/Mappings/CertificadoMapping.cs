using EducationHub.Alunos.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationHub.Alunos.Data.Mappings
{
    public class CertificadoMapping : IEntityTypeConfiguration<Certificado>
    {
        public void Configure(EntityTypeBuilder<Certificado> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.AlunoId)
                .IsRequired();

            builder.Property(c => c.CursoId)
                .IsRequired();

            builder.Property(c => c.TituloCurso)
                .IsRequired()
                .HasColumnType("varchar(200)")
                .HasMaxLength(200);

            builder.Property(c => c.DataEmissao)
                .IsRequired()
                .HasColumnType("datetime2");

            builder.Property(c => c.Codigo)
                .IsRequired()
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            builder.HasIndex(c => c.Codigo).IsUnique();

            builder.HasOne<Aluno>()
                .WithMany(a => a.Certificados)
                .HasForeignKey(c => c.AlunoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Certificados");
        }
    }
}
