using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyMindIA.Migrations
{
    /// <inheritdoc />
    public partial class TraTarjeta : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TarjetaId",
                table: "Transacciones",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Transacciones_TarjetaId",
                table: "Transacciones",
                column: "TarjetaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transacciones_Tarjetas_TarjetaId",
                table: "Transacciones",
                column: "TarjetaId",
                principalTable: "Tarjetas",
                principalColumn: "TarjetaId",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transacciones_Tarjetas_TarjetaId",
                table: "Transacciones");

            migrationBuilder.DropIndex(
                name: "IX_Transacciones_TarjetaId",
                table: "Transacciones");

            migrationBuilder.DropColumn(
                name: "TarjetaId",
                table: "Transacciones");
        }
    }
}
