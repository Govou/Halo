using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class addsCreatedByToInvoiceModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CreatedById",
                table: "Invoices",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_CreatedById",
                table: "Invoices",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_UserProfiles_CreatedById",
                table: "Invoices",
                column: "CreatedById",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_UserProfiles_CreatedById",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_CreatedById",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Invoices");
        }
    }
}
