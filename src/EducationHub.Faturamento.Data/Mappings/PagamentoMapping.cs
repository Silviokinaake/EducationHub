using EducationHub.Faturamento.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationHub.Faturamento.Data.Mappings
{
    public class PagamentoMapping : IEntityTypeConfiguration<Pagamento>
    {
        public void Configure(EntityTypeBuilder<Pagamento> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.AlunoId)
                .IsRequired();

            builder.Property(p => p.PreMatriculaId)
                .IsRequired();

            builder.Property(p => p.Valor)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.DataPagamento)
                .IsRequired();

            builder.Property(p => p.Status)
                .IsRequired();

            builder.Property(p => p.TokenCartao)
                .HasColumnName("TokenCartao")
                .HasColumnType("varchar(200)")
                .HasMaxLength(200);

            builder.Property(p => p.NumeroCartaoMascarado)
                .HasColumnName("NumeroCartaoMascarado")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.ToTable("Pagamentos");
        }
    }
}
