using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class AddAmortizationAndInvoiceTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Quotes_LeadDivisionId",
                table: "Quotes");

            migrationBuilder.CreateTable(
                name: "Amortizations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<long>(type: "bigint", nullable: false),
                    DivisionId = table.Column<long>(type: "bigint", nullable: false),
                    ContractId = table.Column<long>(type: "bigint", nullable: false),
                    ContractServiceId = table.Column<long>(type: "bigint", nullable: false),
                    ContractValue = table.Column<double>(type: "float", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    January = table.Column<double>(type: "float", nullable: false),
                    February = table.Column<double>(type: "float", nullable: false),
                    March = table.Column<double>(type: "float", nullable: false),
                    April = table.Column<double>(type: "float", nullable: false),
                    May = table.Column<double>(type: "float", nullable: false),
                    June = table.Column<double>(type: "float", nullable: false),
                    July = table.Column<double>(type: "float", nullable: false),
                    August = table.Column<double>(type: "float", nullable: false),
                    September = table.Column<double>(type: "float", nullable: false),
                    October = table.Column<double>(type: "float", nullable: false),
                    November = table.Column<double>(type: "float", nullable: false),
                    December = table.Column<double>(type: "float", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Amortizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Amortizations_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Amortizations_ContractServices_ContractServiceId",
                        column: x => x.ContractServiceId,
                        principalTable: "ContractServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Amortizations_CustomerDivisions_DivisionId",
                        column: x => x.DivisionId,
                        principalTable: "CustomerDivisions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Amortizations_Customers_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitPrice = table.Column<long>(type: "bigint", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Discount = table.Column<double>(type: "float", nullable: false),
                    Value = table.Column<double>(type: "float", nullable: false),
                    DateToBeSent = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsInvoiceSent = table.Column<bool>(type: "bit", nullable: false),
                    CustomerDivisionId = table.Column<long>(type: "bigint", nullable: false),
                    ContractId = table.Column<long>(type: "bigint", nullable: false),
                    ContractServiceId = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoices_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Invoices_ContractServices_ContractServiceId",
                        column: x => x.ContractServiceId,
                        principalTable: "ContractServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Invoices_CustomerDivisions_CustomerDivisionId",
                        column: x => x.CustomerDivisionId,
                        principalTable: "CustomerDivisions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Quotes_LeadDivisionId",
                table: "Quotes",
                column: "LeadDivisionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Amortizations_ClientId",
                table: "Amortizations",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Amortizations_ContractId",
                table: "Amortizations",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_Amortizations_ContractServiceId",
                table: "Amortizations",
                column: "ContractServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Amortizations_DivisionId",
                table: "Amortizations",
                column: "DivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_ContractId",
                table: "Invoices",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_ContractServiceId",
                table: "Invoices",
                column: "ContractServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_CustomerDivisionId",
                table: "Invoices",
                column: "CustomerDivisionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Amortizations");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Quotes_LeadDivisionId",
                table: "Quotes");

            migrationBuilder.CreateIndex(
                name: "IX_Quotes_LeadDivisionId",
                table: "Quotes",
                column: "LeadDivisionId");
        }
    }
}
