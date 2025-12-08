using ChurrascariaSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurrascariaSystem.Infrastructure.Data.Configurations
{
    public class PedidoConfiguration : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            builder.ToTable("Pedidos");

            builder.HasKey(p => p.idPedido);

            builder.Property(p => p.idMesa)
                .IsRequired();

            builder.Property(p => p.idUsuario)
                .IsRequired();

            builder.Property(p => p.DataPedido)
                .IsRequired();

            builder.Property(p => p.StatusPedidoValor)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("Status");

            builder.Property(p => p.ValorTotal)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            builder.Property(p => p.Observacao)
                .HasMaxLength(500);

            builder.HasOne(p => p.Mesa)
                .WithMany(m => m.Pedidos)
                .HasForeignKey(p => p.idMesa)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Usuario)
                .WithMany()
                .HasForeignKey(p => p.idUsuario)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.Itens)
                .WithOne(i => i.Pedido)
                .HasForeignKey(i => i.idPedido)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Ignore(p => p.Status);
        }
    }
}
