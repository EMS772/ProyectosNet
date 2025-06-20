using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyMindIA.Migrations
{
    /// <inheritdoc />
    public partial class UsuarioTransacciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Eliminar la clave foránea existente (si existe)
            migrationBuilder.DropForeignKey(
                name: "FK_Transacciones_Usuarios_UsuarioId",
                table: "Transacciones");

            // Cambiar la columna UsuarioId para que no permita valores nulos
            migrationBuilder.AlterColumn<int>(
                name: "UsuarioId",
                table: "Transacciones",
                type: "int",
                nullable: false,
                defaultValue: 0, // Valor por defecto para evitar problemas con datos existentes
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            // Agregar la nueva clave foránea con ON DELETE NO ACTION
            migrationBuilder.AddForeignKey(
                name: "FK_Transacciones_Usuarios_UsuarioId",
                table: "Transacciones",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "UsuarioId",
                onDelete: ReferentialAction.NoAction); // NO ACTION para evitar ciclos de cascada
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Eliminar la clave foránea
            migrationBuilder.DropForeignKey(
                name: "FK_Transacciones_Usuarios_UsuarioId",
                table: "Transacciones");

            // Cambiar la columna UsuarioId para que permita valores nulos
            migrationBuilder.AlterColumn<int>(
                name: "UsuarioId",
                table: "Transacciones",
                type: "int",
                nullable: true, // Permitir valores nulos
                oldClrType: typeof(int),
                oldType: "int");

            // Restaurar la clave foránea original (si existía)
            migrationBuilder.AddForeignKey(
                name: "FK_Transacciones_Usuarios_UsuarioId",
                table: "Transacciones",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "UsuarioId",
                onDelete: ReferentialAction.NoAction); // Restaurar NO ACTION
        }

    }
}