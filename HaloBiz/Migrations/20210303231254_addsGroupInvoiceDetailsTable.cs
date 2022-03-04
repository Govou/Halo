using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class addsGroupInvoiceDetailsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GroupInvoiceNumber",
                table: "QuoteServices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GroupInvoiceNumber",
                table: "ContractServices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GroupInvoiceDetails",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitPrice = table.Column<double>(type: "float", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    VAT = table.Column<double>(type: "float", nullable: false),
                    Value = table.Column<double>(type: "float", nullable: false),
                    BillableAmount = table.Column<double>(type: "float", nullable: false),
                    ContractServiceId = table.Column<long>(type: "bigint", nullable: false),
                    InvoiceId = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupInvoiceDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupInvoiceDetails_ContractServices_ContractServiceId",
                        column: x => x.ContractServiceId,
                        principalTable: "ContractServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_GroupInvoiceDetails_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_GroupInvoiceDetails_UserProfiles_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "GroupInvoiceTracker",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupInvoiceTracker", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupInvoiceDetails_ContractServiceId",
                table: "GroupInvoiceDetails",
                column: "ContractServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupInvoiceDetails_CreatedById",
                table: "GroupInvoiceDetails",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_GroupInvoiceDetails_InvoiceId",
                table: "GroupInvoiceDetails",
                column: "InvoiceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupInvoiceDetails");

            migrationBuilder.DropTable(
                name: "GroupInvoiceTracker");

            migrationBuilder.DropColumn(
                name: "GroupInvoiceNumber",
                table: "QuoteServices");

            migrationBuilder.DropColumn(
                name: "GroupInvoiceNumber",
                table: "ContractServices");
        }
    }
}
