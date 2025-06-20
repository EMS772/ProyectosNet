using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyMindIA.Migrations
{
    /// <inheritdoc />
    public partial class ChatMod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RecomendacionId",
                table: "ChatMensajes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ChatMensajes_RecomendacionId",
                table: "ChatMensajes",
                column: "RecomendacionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMensajes_Recomendaciones_RecomendacionId",
                table: "ChatMensajes",
                column: "RecomendacionId",
                principalTable: "Recomendaciones",
                principalColumn: "RecomendacionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatMensajes_Recomendaciones_RecomendacionId",
                table: "ChatMensajes");

            migrationBuilder.DropIndex(
                name: "IX_ChatMensajes_RecomendacionId",
                table: "ChatMensajes");

            migrationBuilder.DropColumn(
                name: "RecomendacionId",
                table: "ChatMensajes");
        }
    }
}
