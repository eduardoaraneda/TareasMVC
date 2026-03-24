using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TareasMVC.Migrations
{
    /// <inheritdoc />
    public partial class TareasUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UsuarioCreacion",
                table: "Tareas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UsuarioCreacionNavigationId",
                table: "Tareas",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tareas_UsuarioCreacionNavigationId",
                table: "Tareas",
                column: "UsuarioCreacionNavigationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tareas_AspNetUsers_UsuarioCreacionNavigationId",
                table: "Tareas",
                column: "UsuarioCreacionNavigationId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tareas_AspNetUsers_UsuarioCreacionNavigationId",
                table: "Tareas");

            migrationBuilder.DropIndex(
                name: "IX_Tareas_UsuarioCreacionNavigationId",
                table: "Tareas");

            migrationBuilder.DropColumn(
                name: "UsuarioCreacion",
                table: "Tareas");

            migrationBuilder.DropColumn(
                name: "UsuarioCreacionNavigationId",
                table: "Tareas");
        }
    }
}
