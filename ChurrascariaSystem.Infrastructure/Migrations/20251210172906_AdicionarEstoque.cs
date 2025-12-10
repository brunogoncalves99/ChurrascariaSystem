using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChurrascariaSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarEstoque : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Estoques",
                columns: table => new
                {
                    idEstoque = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idProduto = table.Column<int>(type: "int", nullable: false),
                    QuantidadeAtual = table.Column<int>(type: "int", nullable: false),
                    QuantidadeMinima = table.Column<int>(type: "int", nullable: false),
                    UltimaAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estoques", x => x.idEstoque);
                    table.ForeignKey(
                        name: "FK_Estoques_Produtos_idProduto",
                        column: x => x.idProduto,
                        principalTable: "Produtos",
                        principalColumn: "idProduto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MovimentacoesEstoque",
                columns: table => new
                {
                    idMovimentacao = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idProduto = table.Column<int>(type: "int", nullable: false),
                    TipoMovimentacao = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Quantidade = table.Column<int>(type: "int", nullable: false),
                    QuantidadeAnterior = table.Column<int>(type: "int", nullable: false),
                    QuantidadeNova = table.Column<int>(type: "int", nullable: false),
                    Motivo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    idPedido = table.Column<int>(type: "int", nullable: true),
                    idUsuario = table.Column<int>(type: "int", nullable: true),
                    DataMovimentacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Observacao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovimentacoesEstoque", x => x.idMovimentacao);
                    table.ForeignKey(
                        name: "FK_MovimentacoesEstoque_Pedidos_idPedido",
                        column: x => x.idPedido,
                        principalTable: "Pedidos",
                        principalColumn: "idPedido",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MovimentacoesEstoque_Produtos_idProduto",
                        column: x => x.idProduto,
                        principalTable: "Produtos",
                        principalColumn: "idProduto",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MovimentacoesEstoque_Usuarios_idUsuario",
                        column: x => x.idUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "idUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Estoques_idProduto",
                table: "Estoques",
                column: "idProduto",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MovimentacoesEstoque_DataMovimentacao",
                table: "MovimentacoesEstoque",
                column: "DataMovimentacao");

            migrationBuilder.CreateIndex(
                name: "IX_MovimentacoesEstoque_idPedido",
                table: "MovimentacoesEstoque",
                column: "idPedido");

            migrationBuilder.CreateIndex(
                name: "IX_MovimentacoesEstoque_idProduto",
                table: "MovimentacoesEstoque",
                column: "idProduto");

            migrationBuilder.CreateIndex(
                name: "IX_MovimentacoesEstoque_idUsuario",
                table: "MovimentacoesEstoque",
                column: "idUsuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Estoques");

            migrationBuilder.DropTable(
                name: "MovimentacoesEstoque");
        }
    }
}
