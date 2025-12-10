using ChurrascariaSystem.Domain.Entities;
using ChurrascariaSystem.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace ChurrascariaSystem.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<TipoProduto> TiposProduto { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Mesa> Mesas { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<ItemPedido> ItensPedido { get; set; }
        public DbSet<Estoque> Estoques { get; set; }
        public DbSet<MovimentacaoEstoque> MovimentacoesEstoque { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UsuarioConfiguration());
            modelBuilder.ApplyConfiguration(new TipoProdutoConfiguration());    
            modelBuilder.ApplyConfiguration(new ProdutoConfiguration());
            modelBuilder.ApplyConfiguration(new MesaConfiguration());
            modelBuilder.ApplyConfiguration(new PedidoConfiguration());
            modelBuilder.ApplyConfiguration(new ItemPedidoConfiguration());
            modelBuilder.ApplyConfiguration(new EstoqueConfiguration());
            modelBuilder.ApplyConfiguration(new MovimentacaoEstoqueConfiguration());

            // Seed de dados iniciais
            //SeedData(modelBuilder);
        }

        #region Inserindos primeiros registros
        private void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario
                {
                    idUsuario = 1,
                    Nome = "Administrador",
                    Cpf = "14847384628",
                    Senha = BCrypt.Net.BCrypt.HashPassword("Bruno2105"),
                    TipoUsuario = Domain.Enums.TipoUsuario.Admin,
                    Ativo = true,
                    DataCriacao = DateTime.Now,
                    DataModificacao = DateTime.Now
                },
                new Usuario
                {
                    idUsuario = 2,
                    Nome = "Garçom1",
                    Cpf = "56757472651",
                    Senha = BCrypt.Net.BCrypt.HashPassword("garcom123"),
                    TipoUsuario = Domain.Enums.TipoUsuario.Garcom,
                    Ativo = true,
                    DataCriacao = DateTime.Now,
                    DataModificacao = DateTime.Now
                }
            );

            modelBuilder.Entity<TipoProduto>().HasData(
                new TipoProduto { idTipoProduto = 1, Nome = "Espetinhos", Descricao = "Espetinhos variados", Ativo = true },
                new TipoProduto { idTipoProduto = 2, Nome = "Completos", Descricao = "Lanches completos", Ativo = true },
                new TipoProduto { idTipoProduto = 3, Nome = "Bebidas", Descricao = "Bebidas variadas", Ativo = true },
                new TipoProduto { idTipoProduto = 4, Nome = "Porções", Descricao = "Porções diversas", Ativo = true }
            );

            modelBuilder.Entity<Produto>().HasData(
                new Produto { idProduto = 1, Nome = "Churrasquinho de Picanha", Descricao = "Espetinho de picanha suculenta", PrecoValor = 12.00m, idTipoProduto = 1, Ativo = true },
                new Produto { idProduto = 2, Nome = "Churrasquinho de Coração", Descricao = "Espetinho de coração de frango", PrecoValor = 8.00m, idTipoProduto = 1, Ativo = true },
                new Produto { idProduto = 3, Nome = "Churrasquinho de Frango", Descricao = "Espetinho de frango temperado", PrecoValor = 7.00m, idTipoProduto = 1, Ativo = true },
                new Produto { idProduto = 4, Nome = "Completo de Calabresa", Descricao = "Pão, calabresa, queijo e molhos", PrecoValor = 15.00m, idTipoProduto = 2, Ativo = true },
                new Produto { idProduto = 5, Nome = "Coca-Cola 350ml", Descricao = "Refrigerante Coca-Cola lata", PrecoValor = 5.00m, idTipoProduto = 3, Ativo = true },
                new Produto { idProduto = 6, Nome = "Porção de Batata Frita", Descricao = "Batata frita crocante", PrecoValor = 18.00m, idTipoProduto = 4, Ativo = true }
            );

             modelBuilder.Entity<Mesa>().HasData(
                new Mesa { idMesa = 1, Numero = "1", Capacidade = 4, Status = Domain.Enums.StatusMesa.Livre, Ativo = true },
                new Mesa { idMesa = 2, Numero = "2", Capacidade = 4, Status = Domain.Enums.StatusMesa.Livre, Ativo = true },
                new Mesa { idMesa = 3, Numero = "3", Capacidade = 6, Status = Domain.Enums.StatusMesa.Livre, Ativo = true },
                new Mesa { idMesa = 4, Numero = "4", Capacidade = 2, Status = Domain.Enums.StatusMesa.Livre, Ativo = true },
                new Mesa { idMesa = 5, Numero = "5", Capacidade = 4, Status = Domain.Enums.StatusMesa.Livre, Ativo = true }
            );
        }
        #endregion
    }
}
