using ChurrascariaSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurrascariaSystem.Infrastructure.Data.Configurations
{
    public class ItemPedidoConfiguration : IEntityTypeConfiguration<ItemPedido>
    {
        public void Configure(EntityTypeBuilder<ItemPedido> builder)
        {
            builder.ToTable("ItensPedido");

            builder.HasKey(i => i.idItemPedido);

            builder.Property(i => i.idPedido)
                .IsRequired();

            builder.Property(i => i.idProduto)
                .IsRequired();

            builder.Property(i => i.Quantidade)
                .IsRequired();

            builder.Property(i => i.PrecoUnitario)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            builder.Property(i => i.Subtotal)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            builder.Property(i => i.Observacao)
                .HasMaxLength(200);

            builder.HasOne(i => i.Pedido)
                .WithMany(p => p.Itens)
                .HasForeignKey(i => i.idPedido)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(i => i.Produto)
                .WithMany(p => p.ItensPedido)
                .HasForeignKey(i => i.idProduto)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
