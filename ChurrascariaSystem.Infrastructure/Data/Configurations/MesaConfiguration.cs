using ChurrascariaSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurrascariaSystem.Infrastructure.Data.Configurations
{
    public class MesaConfiguration : IEntityTypeConfiguration<Mesa>
    {
        public void Configure(EntityTypeBuilder<Mesa> builder)
        {
            builder.ToTable("Mesas");

            builder.HasKey(m => m.idMesa);

            builder.Property(m => m.Numero)
                .IsRequired()
                .HasMaxLength(10);

            builder.HasIndex(m => m.Numero)
                .IsUnique();

            builder.Property(m => m.Capacidade)
                .IsRequired();

            builder.Property(m => m.Status)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(m => m.Ativo)
                .IsRequired();

            builder.HasMany(m => m.Pedidos)
                .WithOne(p => p.Mesa)
                .HasForeignKey(p => p.idMesa)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
