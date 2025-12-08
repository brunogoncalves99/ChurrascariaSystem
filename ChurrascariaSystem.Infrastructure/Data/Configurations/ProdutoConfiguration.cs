using ChurrascariaSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurrascariaSystem.Infrastructure.Data.Configurations
{
    public class ProdutoConfiguration : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.ToTable("Produtos");

            builder.HasKey(p => p.idProduto);

            builder.Property(p => p.Nome)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.Descricao)
                .HasMaxLength(500);

            builder.Property(p => p.PrecoValor)
                .IsRequired()
                .HasColumnType("decimal(10,2)")
                .HasColumnName("Preco");

            builder.Property(p => p.idTipoProduto)
                .IsRequired();

            builder.Property(p => p.ImagemUrl)
                .HasMaxLength(500);

            builder.Property(p => p.Ativo)
                .IsRequired();

            builder.HasOne(p => p.TipoProduto)
                .WithMany(t => t.Produtos)
                .HasForeignKey(p => p.idTipoProduto)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Ignore(p => p.Preco);
        }
    }
}
