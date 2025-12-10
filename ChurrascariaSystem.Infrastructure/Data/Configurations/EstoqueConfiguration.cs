using ChurrascariaSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurrascariaSystem.Infrastructure.Data.Configurations
{
    public class EstoqueConfiguration : IEntityTypeConfiguration<Estoque>
    {
        public void Configure(EntityTypeBuilder<Estoque> builder)
        {
            builder.ToTable("Estoques");

            builder.HasKey(e => e.idEstoque);

            builder.Property(e => e.QuantidadeAtual)
                .IsRequired();

            builder.Property(e => e.QuantidadeMinima)
                .IsRequired();

            builder.Property(e => e.UltimaAtualizacao)
                .IsRequired();

            builder.HasOne(e => e.Produto)
                .WithMany()
                .HasForeignKey(e => e.idProduto)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(e => e.idProduto)
                .IsUnique();
        }
    }

    public class MovimentacaoEstoqueConfiguration : IEntityTypeConfiguration<MovimentacaoEstoque>
    {
        public void Configure(EntityTypeBuilder<MovimentacaoEstoque> builder)
        {
            builder.ToTable("MovimentacoesEstoque");

            builder.HasKey(m => m.idMovimentacao);

            builder.Property(m => m.TipoMovimentacao)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(m => m.Quantidade)
                .IsRequired();

            builder.Property(m => m.QuantidadeAnterior)
                .IsRequired();

            builder.Property(m => m.QuantidadeNova)
                .IsRequired();

            builder.Property(m => m.Motivo)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(m => m.DataMovimentacao)
                .IsRequired();

            builder.Property(m => m.Observacao)
                .HasMaxLength(500);

            builder.HasOne(m => m.Produto)
                .WithMany()
                .HasForeignKey(m => m.idProduto)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.Pedido)
                .WithMany()
                .HasForeignKey(m => m.idPedido)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            builder.HasOne(m => m.Usuario)
                .WithMany()
                .HasForeignKey(m => m.idUsuario)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            builder.HasIndex(m => m.idProduto);
            builder.HasIndex(m => m.DataMovimentacao);
        }
    }
}
