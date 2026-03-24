using Microsoft.EntityFrameworkCore.Migrations;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;

#nullable disable

namespace TareasMVC.Migrations
{
    /// <inheritdoc />
    public partial class AdminRol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            IF NOT EXISTS (SELECT Id FROM [AspNetRoles] WHERE Id = 'c4c2d7e4-3d3c-4c08-a2fa-5bc0975e7f51')
            BEGIN
            INSERT INTO [AspNetRoles] ([Id], [Name], [NormalizedName])
            VALUES ('c4c2d7e4-3d3c-4c08-a2fa-5bc0975e7f51', 'admin', 'ADMIN')
            END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            DELETE FROM [AspNetRoles]
            WHERE Id = 'c4c2d7e4-3d3c-4c08-a2fa-5bc0975e7f51'");
        }
    }
}

