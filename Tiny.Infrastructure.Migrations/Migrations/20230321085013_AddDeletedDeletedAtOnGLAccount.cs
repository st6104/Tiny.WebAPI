using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tiny.Infrastructure.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddDeletedDeletedAtOnGLAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                schema: "dbo",
                table: "GLAccount",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "dbo",
                table: "GLAccount",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deleted",
                schema: "dbo",
                table: "GLAccount");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "dbo",
                table: "GLAccount");
        }
    }
}
