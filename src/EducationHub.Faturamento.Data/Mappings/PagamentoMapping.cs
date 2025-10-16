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

            builder.Property(p => p.MatriculaId)
                .IsRequired();

            builder.Property(p => p.Valor)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.DataPagamento)
                .IsRequired();

            builder.Property(p => p.Status)
                .IsRequired();

            builder.OwnsOne(p => p.DadosCartao, dc =>
            {
                dc.Property(d => d.NomeTitular)
                    .HasColumnName("NomeTitular")
                    .HasColumnType("varchar(100)")
                    .HasMaxLength(100)
                    .IsRequired();

                dc.Property(d => d.Numero)
                    .HasColumnName("NumeroCartao")
                    .HasColumnType("varchar(20)")
                    .HasMaxLength(20)
                    .IsRequired();

                dc.Property(d => d.Validade)
                    .HasColumnName("ValidadeCartao")
                    .HasColumnType("varchar(5)")
                    .HasMaxLength(5)
                    .IsRequired();

                dc.Property(d => d.Cvv)
                    .HasColumnName("CvvCartao")
                    .HasColumnType("varchar(4)")
                    .HasMaxLength(4)
                    .IsRequired();

                dc.WithOwner();
            });

            builder.ToTable("Pagamentos");
        }
    }
}
