using ChurrascariaSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurrascariaSystem.Infrastructure.Data.Configurations
{
    public class TipoProdutoConfiguration : IEntityTypeConfiguration<TipoProduto>
    {
        public void Configure(EntityTypeBuilder<TipoProduto> builder)
        {
            builder.ToTable("TiposProduto");

            builder.HasKey(t => t.idTipoProduto);

            builder.Property(t => t.Nome)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(t => t.Descricao)
                .HasMaxLength(200);

            builder.Property(t => t.Ativo)
                .IsRequired();

            builder.HasMany(t => t.Produtos)
                .WithOne(p => p.TipoProduto)
                .HasForeignKey(p => p.idTipoProduto)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
