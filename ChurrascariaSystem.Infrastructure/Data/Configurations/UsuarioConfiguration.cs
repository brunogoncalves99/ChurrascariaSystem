using ChurrascariaSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurrascariaSystem.Infrastructure.Data.Configurations
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuarios");

            builder.HasKey(u => u.idUsuario);

            builder.Property(u => u.Nome)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.Cpf)
                .IsRequired()
                .HasMaxLength(14);

            builder.HasIndex(u => u.Cpf)
                .IsUnique();

            builder.Property(u => u.Senha)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(u => u.TipoUsuario)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(u => u.Ativo)
                .IsRequired();

            builder.Property(u => u.DataCriacao)
                .IsRequired();
        }
    }
}
