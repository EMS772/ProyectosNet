using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyMindIA.Migrations
{
    public partial class FixTarjetaTransaccionesRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Eliminar la restricción de clave foránea con ON DELETE CASCADE
            migrationBuilder.DropForeignKey(
                name: "FK_Transacciones_Tarjetas_TarjetaId",
                table: "Transacciones");

            // Agregar la restricción de clave foránea sin ON DELETE CASCADE
            migrationBuilder.AddForeignKey(
                name: "FK_Transacciones_Tarjetas_TarjetaId",
                table: "Transacciones",
                column: "TarjetaId",
                principalTable: "Tarjetas",
                principalColumn: "TarjetaId",
                onDelete: ReferentialAction.NoAction); // Cambia a NoAction
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revertir los cambios (opcional, para deshacer la migración)
            migrationBuilder.DropForeignKey(
                name: "FK_Transacciones_Tarjetas_TarjetaId",
                table: "Transacciones");

            // Restaurar la restricción de clave foránea con ON DELETE CASCADE
            migrationBuilder.AddForeignKey(
                name: "FK_Transacciones_Tarjetas_TarjetaId",
                table: "Transacciones",
                column: "TarjetaId",
                principalTable: "Tarjetas",
                principalColumn: "TarjetaId",
                onDelete: ReferentialAction.Cascade); // Restaurar ON DELETE CASCADE
        }
    }
}