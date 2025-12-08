using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ChurrascariaSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Mesas",
                columns: table => new
                {
                    idMesa = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Numero = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Capacidade = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mesas", x => x.idMesa);
                });

            migrationBuilder.CreateTable(
                name: "TiposProduto",
                columns: table => new
                {
                    idTipoProduto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposProduto", x => x.idTipoProduto);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    idUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Cpf = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false),
                    Senha = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TipoUsuario = table.Column<int>(type: "int", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataModificacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.idUsuario);
                });

            migrationBuilder.CreateTable(
                name: "Produtos",
                columns: table => new
                {
                    idProduto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Preco = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    idTipoProduto = table.Column<int>(type: "int", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    ImagemUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produtos", x => x.idProduto);
                    table.ForeignKey(
                        name: "FK_Produtos_TiposProduto_idTipoProduto",
                        column: x => x.idTipoProduto,
                        principalTable: "TiposProduto",
                        principalColumn: "idTipoProduto",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pedidos",
                columns: table => new
                {
                    idPedido = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idMesa = table.Column<int>(type: "int", nullable: false),
                    idUsuario = table.Column<int>(type: "int", nullable: false),
                    DataPedido = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ValorTotal = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Observacao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedidos", x => x.idPedido);
                    table.ForeignKey(
                        name: "FK_Pedidos_Mesas_idMesa",
                        column: x => x.idMesa,
                        principalTable: "Mesas",
                        principalColumn: "idMesa",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pedidos_Usuarios_idUsuario",
                        column: x => x.idUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "idUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ItensPedido",
                columns: table => new
                {
                    idItemPedido = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idPedido = table.Column<int>(type: "int", nullable: false),
                    idProduto = table.Column<int>(type: "int", nullable: false),
                    Quantidade = table.Column<int>(type: "int", nullable: false),
                    PrecoUnitario = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Observacao = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItensPedido", x => x.idItemPedido);
                    table.ForeignKey(
                        name: "FK_ItensPedido_Pedidos_idPedido",
                        column: x => x.idPedido,
                        principalTable: "Pedidos",
                        principalColumn: "idPedido",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItensPedido_Produtos_idProduto",
                        column: x => x.idProduto,
                        principalTable: "Produtos",
                        principalColumn: "idProduto",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Mesas",
                columns: new[] { "idMesa", "Ativo", "Capacidade", "Numero", "Status" },
                values: new object[,]
                {
                    { 1, true, 4, "1", 1 },
                    { 2, true, 4, "2", 1 },
                    { 3, true, 6, "3", 1 },
                    { 4, true, 2, "4", 1 },
                    { 5, true, 4, "5", 1 }
                });

            migrationBuilder.InsertData(
                table: "TiposProduto",
                columns: new[] { "idTipoProduto", "Ativo", "Descricao", "Nome" },
                values: new object[,]
                {
                    { 1, true, "Espetinhos variados", "Espetinhos" },
                    { 2, true, "Lanches completos", "Completos" },
                    { 3, true, "Bebidas variadas", "Bebidas" },
                    { 4, true, "Porções diversas", "Porções" }
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "idUsuario", "Ativo", "Cpf", "DataCriacao", "DataModificacao", "Nome", "Senha", "TipoUsuario" },
                values: new object[,]
                {
                    { 1, true, "14847384628", new DateTime(2025, 12, 8, 18, 25, 8, 400, DateTimeKind.Local).AddTicks(3602), new DateTime(2025, 12, 8, 18, 25, 8, 400, DateTimeKind.Local).AddTicks(3618), "Administrador", "$2a$11$AgLmWqXKvJnm6Eudyd.EheXvw20jnCvnZ90Nk8tVC8kYM13g5srYi", 1 },
                    { 2, true, "56757472651", new DateTime(2025, 12, 8, 18, 25, 8, 566, DateTimeKind.Local).AddTicks(6588), new DateTime(2025, 12, 8, 18, 25, 8, 566, DateTimeKind.Local).AddTicks(6607), "Garçom1", "$2a$11$FKX1fDrPmv1A4gt4yQNkVuVITtJRmLBipRJH0J8Ul2PKFSmPTEJoS", 2 }
                });

            migrationBuilder.InsertData(
                table: "Produtos",
                columns: new[] { "idProduto", "Ativo", "Descricao", "ImagemUrl", "Nome", "Preco", "idTipoProduto" },
                values: new object[,]
                {
                    { 1, true, "Espetinho de picanha suculenta", null, "Churrasquinho de Picanha", 12.00m, 1 },
                    { 2, true, "Espetinho de coração de frango", null, "Churrasquinho de Coração", 8.00m, 1 },
                    { 3, true, "Espetinho de frango temperado", null, "Churrasquinho de Frango", 7.00m, 1 },
                    { 4, true, "Pão, calabresa, queijo e molhos", null, "Completo de Calabresa", 15.00m, 2 },
                    { 5, true, "Refrigerante Coca-Cola lata", null, "Coca-Cola 350ml", 5.00m, 3 },
                    { 6, true, "Batata frita crocante", null, "Porção de Batata Frita", 18.00m, 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItensPedido_idPedido",
                table: "ItensPedido",
                column: "idPedido");

            migrationBuilder.CreateIndex(
                name: "IX_ItensPedido_idProduto",
                table: "ItensPedido",
                column: "idProduto");

            migrationBuilder.CreateIndex(
                name: "IX_Mesas_Numero",
                table: "Mesas",
                column: "Numero",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_idMesa",
                table: "Pedidos",
                column: "idMesa");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_idUsuario",
                table: "Pedidos",
                column: "idUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_idTipoProduto",
                table: "Produtos",
                column: "idTipoProduto");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Cpf",
                table: "Usuarios",
                column: "Cpf",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItensPedido");

            migrationBuilder.DropTable(
                name: "Pedidos");

            migrationBuilder.DropTable(
                name: "Produtos");

            migrationBuilder.DropTable(
                name: "Mesas");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "TiposProduto");
        }
    }
}
