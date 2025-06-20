using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyMindIA.Migrations
{
    /// <inheritdoc />
    public partial class MasCategorias : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categorias",
                columns: new[] { "Nombre" },
                values: new object[,]
                {
            { "Vivienda" },
            { "Servicios Públicos" },
            { "Ropa" },
            { "Ahorros" },
            { "Inversiones" },
            { "Seguros" },
            { "Viajes" },
            { "Regalos" },
            { "Deudas" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "Nombre",
                keyValues: new object[]
                {
            "Vivienda",
            "Servicios Públicos",
            "Ropa",
            "Ahorros",
            "Inversiones",
            "Seguros",
            "Viajes",
            "Regalos",
            "Deudas"
                });
        }
    }
}
