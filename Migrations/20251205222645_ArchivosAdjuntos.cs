using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TareasMVC.Migrations
{
    /// <inheritdoc />
    public partial class ArchivosAdjuntos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ArchivoAdjuntoId",
                table: "Pasos",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ArchivosAdjuntos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TareaId = table.Column<int>(type: "int", nullable: false),
                    Url = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    titulo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    orden = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ArchivoAdjuntoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArchivosAdjuntos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArchivosAdjuntos_ArchivosAdjuntos_ArchivoAdjuntoId",
                        column: x => x.ArchivoAdjuntoId,
                        principalTable: "ArchivosAdjuntos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ArchivosAdjuntos_Tareas_TareaId",
                        column: x => x.TareaId,
                        principalTable: "Tareas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pasos_ArchivoAdjuntoId",
                table: "Pasos",
                column: "ArchivoAdjuntoId");

            migrationBuilder.CreateIndex(
                name: "IX_ArchivosAdjuntos_ArchivoAdjuntoId",
                table: "ArchivosAdjuntos",
                column: "ArchivoAdjuntoId");

            migrationBuilder.CreateIndex(
                name: "IX_ArchivosAdjuntos_TareaId",
                table: "ArchivosAdjuntos",
                column: "TareaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pasos_ArchivosAdjuntos_ArchivoAdjuntoId",
                table: "Pasos",
                column: "ArchivoAdjuntoId",
                principalTable: "ArchivosAdjuntos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pasos_ArchivosAdjuntos_ArchivoAdjuntoId",
                table: "Pasos");

            migrationBuilder.DropTable(
                name: "ArchivosAdjuntos");

            migrationBuilder.DropIndex(
                name: "IX_Pasos_ArchivoAdjuntoId",
                table: "Pasos");

            migrationBuilder.DropColumn(
                name: "ArchivoAdjuntoId",
                table: "Pasos");
        }
    }
}
