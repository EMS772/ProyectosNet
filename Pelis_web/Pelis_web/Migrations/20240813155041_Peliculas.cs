using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pelis_web.Migrations
{
    /// <inheritdoc />
    public partial class Peliculas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "CalificacionImdb",
                table: "Peliculas",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "Peliculas",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CalificacionImdb",
                table: "Peliculas");

            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "Peliculas");
        }
    }
}
