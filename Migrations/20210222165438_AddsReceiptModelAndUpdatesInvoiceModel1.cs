using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class AddsReceiptModelAndUpdatesInvoiceModel1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Receipt_Invoices_InvoiceId",
                table: "Receipt");

            migrationBuilder.DropForeignKey(
                name: "FK_Receipt_UserProfiles_CreatedById",
                table: "Receipt");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Receipt",
                table: "Receipt");

            migrationBuilder.RenameTable(
                name: "Receipt",
                newName: "Receipts");

            migrationBuilder.RenameIndex(
                name: "IX_Receipt_InvoiceId",
                table: "Receipts",
                newName: "IX_Receipts_InvoiceId");

            migrationBuilder.RenameIndex(
                name: "IX_Receipt_CreatedById",
                table: "Receipts",
                newName: "IX_Receipts_CreatedById");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Receipts",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Receipts",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Receipts",
                table: "Receipts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Receipts_Invoices_InvoiceId",
                table: "Receipts",
                column: "InvoiceId",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Receipts_UserProfiles_CreatedById",
                table: "Receipts",
                column: "CreatedById",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Receipts_Invoices_InvoiceId",
                table: "Receipts");

            migrationBuilder.DropForeignKey(
                name: "FK_Receipts_UserProfiles_CreatedById",
                table: "Receipts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Receipts",
                table: "Receipts");

            migrationBuilder.RenameTable(
                name: "Receipts",
                newName: "Receipt");

            migrationBuilder.RenameIndex(
                name: "IX_Receipts_InvoiceId",
                table: "Receipt",
                newName: "IX_Receipt_InvoiceId");

            migrationBuilder.RenameIndex(
                name: "IX_Receipts_CreatedById",
                table: "Receipt",
                newName: "IX_Receipt_CreatedById");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Receipt",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Receipt",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Receipt",
                table: "Receipt",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Receipt_Invoices_InvoiceId",
                table: "Receipt",
                column: "InvoiceId",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Receipt_UserProfiles_CreatedById",
                table: "Receipt",
                column: "CreatedById",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
