using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NexoraERP.Infrastructure.Persistence.Tenant.Migrations
{
    /// <inheritdoc />
    public partial class ExpandEnterpriseAccounting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // COA/journal shape change: clear prior dev data (replace with proper ETL in real migrations).
            migrationBuilder.Sql("DELETE FROM JournalLines;");
            migrationBuilder.Sql("DELETE FROM JournalEntries;");
            migrationBuilder.Sql("DELETE FROM ChartAccounts;");

            migrationBuilder.DropIndex(
                name: "IX_JournalLines_JournalEntryId",
                table: "JournalLines");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "JournalLines",
                newName: "TransactionDebit");

            migrationBuilder.DropColumn(
                name: "Side",
                table: "JournalLines");

            migrationBuilder.AddColumn<int>(
                name: "LineNumber",
                table: "JournalLines",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.RenameColumn(
                name: "CurrencyCode",
                table: "JournalEntries",
                newName: "BaseCurrencyCode");

            migrationBuilder.AddColumn<decimal>(
                name: "BaseCreditAmount",
                table: "JournalLines",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "BaseDebitAmount",
                table: "JournalLines",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ExchangeRateToBase",
                table: "JournalLines",
                type: "decimal(18,8)",
                precision: 18,
                scale: 8,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TransactionCredit",
                table: "JournalLines",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<string>(
                name: "Memo",
                table: "JournalEntries",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(512)",
                oldMaxLength: 512,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAtUtc",
                table: "JournalEntries",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<int>(
                name: "EntrySequence",
                table: "JournalEntries",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "PostedAtUtc",
                table: "JournalEntries",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Reference",
                table: "JournalEntries",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "JournalEntries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ChartAccounts",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "ChartAccounts",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(32)",
                oldMaxLength: 32);

            migrationBuilder.AddColumn<int>(
                name: "AccountType",
                table: "ChartAccounts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAtUtc",
                table: "ChartAccounts",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<int>(
                name: "Depth",
                table: "ChartAccounts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "HierarchyPath",
                table: "ChartAccounts",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsPostingAccount",
                table: "ChartAccounts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedAtUtc",
                table: "ChartAccounts",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ParentAccountId",
                table: "ChartAccounts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "ChartAccounts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_JournalLines_JournalEntryId_LineNumber",
                table: "JournalLines",
                columns: new[] { "JournalEntryId", "LineNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntries_EntryDate",
                table: "JournalEntries",
                column: "EntryDate");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntries_Status",
                table: "JournalEntries",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_ChartAccounts_HierarchyPath",
                table: "ChartAccounts",
                column: "HierarchyPath");

            migrationBuilder.CreateIndex(
                name: "IX_ChartAccounts_ParentAccountId",
                table: "ChartAccounts",
                column: "ParentAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChartAccounts_ChartAccounts_ParentAccountId",
                table: "ChartAccounts",
                column: "ParentAccountId",
                principalTable: "ChartAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChartAccounts_ChartAccounts_ParentAccountId",
                table: "ChartAccounts");

            migrationBuilder.DropIndex(
                name: "IX_JournalLines_JournalEntryId_LineNumber",
                table: "JournalLines");

            migrationBuilder.DropIndex(
                name: "IX_JournalEntries_EntryDate",
                table: "JournalEntries");

            migrationBuilder.DropIndex(
                name: "IX_JournalEntries_Status",
                table: "JournalEntries");

            migrationBuilder.DropIndex(
                name: "IX_ChartAccounts_HierarchyPath",
                table: "ChartAccounts");

            migrationBuilder.DropIndex(
                name: "IX_ChartAccounts_ParentAccountId",
                table: "ChartAccounts");

            migrationBuilder.DropColumn(
                name: "BaseCreditAmount",
                table: "JournalLines");

            migrationBuilder.DropColumn(
                name: "BaseDebitAmount",
                table: "JournalLines");

            migrationBuilder.DropColumn(
                name: "ExchangeRateToBase",
                table: "JournalLines");

            migrationBuilder.DropColumn(
                name: "TransactionCredit",
                table: "JournalLines");

            migrationBuilder.DropColumn(
                name: "CreatedAtUtc",
                table: "JournalEntries");

            migrationBuilder.DropColumn(
                name: "EntrySequence",
                table: "JournalEntries");

            migrationBuilder.DropColumn(
                name: "PostedAtUtc",
                table: "JournalEntries");

            migrationBuilder.DropColumn(
                name: "Reference",
                table: "JournalEntries");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "JournalEntries");

            migrationBuilder.DropColumn(
                name: "AccountType",
                table: "ChartAccounts");

            migrationBuilder.DropColumn(
                name: "CreatedAtUtc",
                table: "ChartAccounts");

            migrationBuilder.DropColumn(
                name: "Depth",
                table: "ChartAccounts");

            migrationBuilder.DropColumn(
                name: "HierarchyPath",
                table: "ChartAccounts");

            migrationBuilder.DropColumn(
                name: "IsPostingAccount",
                table: "ChartAccounts");

            migrationBuilder.DropColumn(
                name: "ModifiedAtUtc",
                table: "ChartAccounts");

            migrationBuilder.DropColumn(
                name: "ParentAccountId",
                table: "ChartAccounts");

            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "ChartAccounts");

            migrationBuilder.RenameColumn(
                name: "TransactionDebit",
                table: "JournalLines",
                newName: "Amount");

            migrationBuilder.DropColumn(
                name: "LineNumber",
                table: "JournalLines");

            migrationBuilder.AddColumn<int>(
                name: "Side",
                table: "JournalLines",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.RenameColumn(
                name: "BaseCurrencyCode",
                table: "JournalEntries",
                newName: "CurrencyCode");

            migrationBuilder.AlterColumn<string>(
                name: "Memo",
                table: "JournalEntries",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1024)",
                oldMaxLength: 1024,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ChartAccounts",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(512)",
                oldMaxLength: 512);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "ChartAccounts",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(64)",
                oldMaxLength: 64);

            migrationBuilder.CreateIndex(
                name: "IX_JournalLines_JournalEntryId",
                table: "JournalLines",
                column: "JournalEntryId");
        }
    }
}
