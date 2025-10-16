using EducationHub.Conteudo.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationHub.Conteudo.Data.Mappings
{
    public class AulaMapping : IEntityTypeConfiguration<Aula>
    {
        public void Configure(EntityTypeBuilder<Aula> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Titulo)
                .IsRequired()
                .HasColumnType("varchar(150)")
                .HasMaxLength(150);

            builder.Property(a => a.ConteudoAula)
                .IsRequired()
                .HasColumnType("varchar(5000)")
                .HasMaxLength(5000);

            builder.Property(a => a.MaterialDeApoio)
                .HasColumnType("varchar(1000)")
                .HasMaxLength(1000);

            builder.Property(a => a.Duracao)
                .IsRequired();

            builder.Property(a => a.CursoId)
                .IsRequired();

            builder.HasOne(a => a.Curso)
                .WithMany(c => c.Aulas)
                .HasForeignKey(a => a.CursoId);

            builder.ToTable("Aulas");
        }
    }
}
