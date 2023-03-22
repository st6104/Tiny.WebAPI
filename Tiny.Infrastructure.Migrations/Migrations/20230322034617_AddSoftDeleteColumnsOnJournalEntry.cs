using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tiny.Infrastructure.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDeleteColumnsOnJournalEntry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "dbo",
                table: "JournalEntryLine",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<decimal>(
                name: "DebitAmount",
                schema: "dbo",
                table: "JournalEntryLine",
                type: "decimal(19,6)",
                precision: 19,
                scale: 6,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,6)",
                oldPrecision: 19,
                oldScale: 6);

            migrationBuilder.AlterColumn<decimal>(
                name: "CreditAmount",
                schema: "dbo",
                table: "JournalEntryLine",
                type: "decimal(19,6)",
                precision: 19,
                scale: 6,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,6)",
                oldPrecision: 19,
                oldScale: 6);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "JournalEntry",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "JournalEntry",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "JournalEntry",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "JournalEntry");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "JournalEntry");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "dbo",
                table: "JournalEntryLine",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<decimal>(
                name: "DebitAmount",
                schema: "dbo",
                table: "JournalEntryLine",
                type: "decimal(19,6)",
                precision: 19,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,6)",
                oldPrecision: 19,
                oldScale: 6,
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "CreditAmount",
                schema: "dbo",
                table: "JournalEntryLine",
                type: "decimal(19,6)",
                precision: 19,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,6)",
                oldPrecision: 19,
                oldScale: 6,
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "JournalEntry",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldDefaultValue: "");
        }
    }
}
