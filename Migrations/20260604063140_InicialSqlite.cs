using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InventarioApp.Migrations
{
    /// <inheritdoc />
    public partial class InicialSqlite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "TEXT", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Productos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    CategoriaId = table.Column<int>(type: "INTEGER", nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    Stock = table.Column<int>(type: "INTEGER", nullable: false),
                    StockMinimo = table.Column<int>(type: "INTEGER", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Productos_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Movimientos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProductoId = table.Column<int>(type: "INTEGER", nullable: false),
                    Tipo = table.Column<string>(type: "TEXT", nullable: false),
                    Cantidad = table.Column<int>(type: "INTEGER", nullable: false),
                    Fecha = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Observacion = table.Column<string>(type: "TEXT", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movimientos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Movimientos_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categorias",
                columns: new[] { "Id", "Descripcion", "Nombre" },
                values: new object[,]
                {
                    { 1, "Equipos electronicos", "Electronica" },
                    { 2, "Articulos de oficina", "Papeleria" },
                    { 3, "Equipos de trabajo", "Herramientas" }
                });

            migrationBuilder.InsertData(
                table: "Productos",
                columns: new[] { "Id", "CategoriaId", "FechaRegistro", "Nombre", "PrecioUnitario", "Stock", "StockMinimo" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Laptop Dell", 15000m, 8, 5 },
                    { 2, 1, new DateTime(2026, 1, 11, 0, 0, 0, 0, DateTimeKind.Utc), "Mouse Logitech", 350m, 3, 10 },
                    { 3, 2, new DateTime(2026, 1, 12, 0, 0, 0, 0, DateTimeKind.Utc), "Resma Papel", 120m, 50, 20 },
                    { 4, 3, new DateTime(2026, 1, 13, 0, 0, 0, 0, DateTimeKind.Utc), "Taladro Bosch", 2800m, 2, 3 }
                });

            migrationBuilder.InsertData(
                table: "Movimientos",
                columns: new[] { "Id", "Cantidad", "Fecha", "Observacion", "ProductoId", "Tipo" },
                values: new object[,]
                {
                    { 1, 10, new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Compra inicial", 1, "Entrada" },
                    { 2, 2, new DateTime(2026, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), "Venta cliente A", 1, "Salida" },
                    { 3, 5, new DateTime(2026, 2, 3, 0, 0, 0, 0, DateTimeKind.Utc), "Reposicion", 2, "Entrada" },
                    { 4, 1, new DateTime(2026, 2, 7, 0, 0, 0, 0, DateTimeKind.Utc), "Prestamo interno", 4, "Salida" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Movimientos_ProductoId",
                table: "Movimientos",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_CategoriaId",
                table: "Productos",
                column: "CategoriaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Movimientos");

            migrationBuilder.DropTable(
                name: "Productos");

            migrationBuilder.DropTable(
                name: "Categorias");
        }
    }
}
