using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class updatesRelationshipBtwInvoiceAndGroupInvoiceDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupInvoiceDetails_Invoices_InvoiceId",
                table: "GroupInvoiceDetails");

            migrationBuilder.DropIndex(
                name: "IX_GroupInvoiceDetails_InvoiceId",
                table: "GroupInvoiceDetails");

            migrationBuilder.DropColumn(
                name: "InvoiceId",
                table: "GroupInvoiceDetails");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "InvoiceId",
                table: "GroupInvoiceDetails",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_GroupInvoiceDetails_InvoiceId",
                table: "GroupInvoiceDetails",
                column: "InvoiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupInvoiceDetails_Invoices_InvoiceId",
                table: "GroupInvoiceDetails",
                column: "InvoiceId",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
