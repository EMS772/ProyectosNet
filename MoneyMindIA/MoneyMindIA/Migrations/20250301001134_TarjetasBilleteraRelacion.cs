using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyMindIA.Migrations
{
    /// <inheritdoc />
    public partial class TarjetasBilleteraRelacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Billeteras_Usuarios_UsuarioId",
                table: "Billeteras");

            migrationBuilder.DropForeignKey(
                name: "FK_Transacciones_Usuarios_UsuarioId",
                table: "Transacciones");

            migrationBuilder.AddForeignKey(
                name: "FK_Billeteras_Usuarios_UsuarioId",
                table: "Billeteras",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transacciones_Usuarios_UsuarioId",
                table: "Transacciones",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Billeteras_Usuarios_UsuarioId",
                table: "Billeteras");

            migrationBuilder.DropForeignKey(
                name: "FK_Transacciones_Usuarios_UsuarioId",
                table: "Transacciones");

            migrationBuilder.AddForeignKey(
                name: "FK_Billeteras_Usuarios_UsuarioId",
                table: "Billeteras",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "UsuarioId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transacciones_Usuarios_UsuarioId",
                table: "Transacciones",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "UsuarioId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
