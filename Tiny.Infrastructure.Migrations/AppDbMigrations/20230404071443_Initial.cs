using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Tiny.Infrastructure.Migrations.AppDbMigrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateSequence(
                name: "EntityFrameworkHiLoSequence",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "AccountingType",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountingType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Department",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JournalEntryStatus",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JournalEntryStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Postable",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Postable", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JournalEntry",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    PostingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    JournalEntryStatusId = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, defaultValue: ""),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JournalEntry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JournalEntry_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalSchema: "dbo",
                        principalTable: "Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JournalEntry_JournalEntryStatus_JournalEntryStatusId",
                        column: x => x.JournalEntryStatusId,
                        principalSchema: "dbo",
                        principalTable: "JournalEntryStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GLAccount",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PostableId = table.Column<int>(type: "int", nullable: false),
                    AccountingTypeId = table.Column<int>(type: "int", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(19,6)", precision: 19, scale: 6, nullable: false, defaultValue: 0m),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GLAccount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GLAccount_AccountingType_AccountingTypeId",
                        column: x => x.AccountingTypeId,
                        principalSchema: "dbo",
                        principalTable: "AccountingType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GLAccount_Postable_PostableId",
                        column: x => x.PostableId,
                        principalSchema: "dbo",
                        principalTable: "Postable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JournalEntryLine",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GLAccountId = table.Column<long>(type: "bigint", nullable: false),
                    DebitAmount = table.Column<decimal>(type: "decimal(19,6)", precision: 19, scale: 6, nullable: false, defaultValue: 0m),
                    CreditAmount = table.Column<decimal>(type: "decimal(19,6)", precision: 19, scale: 6, nullable: false, defaultValue: 0m),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, defaultValue: ""),
                    JournalEntryId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JournalEntryLine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JournalEntryLine_GLAccount_GLAccountId",
                        column: x => x.GLAccountId,
                        principalSchema: "dbo",
                        principalTable: "GLAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JournalEntryLine_JournalEntry_JournalEntryId",
                        column: x => x.JournalEntryId,
                        principalSchema: "dbo",
                        principalTable: "JournalEntry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "AccountingType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Asset" },
                    { 2, "Liability" },
                    { 3, "Equity" },
                    { 4, "Revenue" },
                    { 5, "Expense" }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "JournalEntryStatus",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "신청" },
                    { 2, "승인" },
                    { 3, "반려" }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Postable",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Yes" },
                    { 2, "No" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Department_TenantId_Code",
                schema: "dbo",
                table: "Department",
                columns: new[] { "TenantId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GLAccount_AccountingTypeId",
                schema: "dbo",
                table: "GLAccount",
                column: "AccountingTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_GLAccount_PostableId",
                schema: "dbo",
                table: "GLAccount",
                column: "PostableId");

            migrationBuilder.CreateIndex(
                name: "IX_GLAccount_TenantId_Code_Name",
                schema: "dbo",
                table: "GLAccount",
                columns: new[] { "TenantId", "Code", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntry_DepartmentId",
                schema: "dbo",
                table: "JournalEntry",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntry_JournalEntryStatusId",
                schema: "dbo",
                table: "JournalEntry",
                column: "JournalEntryStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntryLine_GLAccountId",
                schema: "dbo",
                table: "JournalEntryLine",
                column: "GLAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntryLine_JournalEntryId",
                schema: "dbo",
                table: "JournalEntryLine",
                column: "JournalEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_User_TenantId_Code",
                schema: "dbo",
                table: "User",
                columns: new[] { "TenantId", "Code" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JournalEntryLine",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "User",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "GLAccount",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "JournalEntry",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AccountingType",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Postable",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Department",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "JournalEntryStatus",
                schema: "dbo");

            migrationBuilder.DropSequence(
                name: "EntityFrameworkHiLoSequence");
        }
    }
}
