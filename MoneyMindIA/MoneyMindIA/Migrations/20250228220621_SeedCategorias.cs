using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyMindIA.Migrations
{
    /// <inheritdoc />
    public partial class SeedCategorias : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Insertar datos iniciales en la tabla Categorias
            migrationBuilder.InsertData(
                table: "Categorias", // Nombre de la tabla
                columns: new[] { "Nombre" }, // Columnas a insertar
                values: new object[,]
                {
                { "Comida" },
                { "Transporte" },
                { "Entretenimiento" },
                { "Salud" },
                { "Educación" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Eliminar los datos insertados (opcional, para revertir la migración)
            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "Nombre",
                keyValues: new object[]
                {
                "Comida",
                "Transporte",
                "Entretenimiento",
                "Salud",
                "Educación"
                });
        }
    }
}
