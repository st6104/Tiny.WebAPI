using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tiny.Infrastructure.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class Second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GLAccount_AccountingType_AccountTypeId",
                schema: "dbo",
                table: "GLAccount");

            migrationBuilder.RenameColumn(
                name: "AccountTypeId",
                schema: "dbo",
                table: "GLAccount",
                newName: "AccountingTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_GLAccount_AccountTypeId",
                schema: "dbo",
                table: "GLAccount",
                newName: "IX_GLAccount_AccountingTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_GLAccount_AccountingType_AccountingTypeId",
                schema: "dbo",
                table: "GLAccount",
                column: "AccountingTypeId",
                principalSchema: "dbo",
                principalTable: "AccountingType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GLAccount_AccountingType_AccountingTypeId",
                schema: "dbo",
                table: "GLAccount");

            migrationBuilder.RenameColumn(
                name: "AccountingTypeId",
                schema: "dbo",
                table: "GLAccount",
                newName: "AccountTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_GLAccount_AccountingTypeId",
                schema: "dbo",
                table: "GLAccount",
                newName: "IX_GLAccount_AccountTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_GLAccount_AccountingType_AccountTypeId",
                schema: "dbo",
                table: "GLAccount",
                column: "AccountTypeId",
                principalSchema: "dbo",
                principalTable: "AccountingType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
