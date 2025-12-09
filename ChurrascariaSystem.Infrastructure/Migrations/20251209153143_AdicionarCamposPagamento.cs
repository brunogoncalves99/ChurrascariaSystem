using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ChurrascariaSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarCamposPagamento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataPagamento",
                table: "Pedidos",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FormaPagamento",
                table: "Pedidos",
                type: "nvarchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Pago",
                table: "Pedidos",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataPagamento",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "FormaPagamento",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "Pago",
                table: "Pedidos");

        }
    }
}
