using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class addsNewFieldsToInvoiceAndContractServiceForEndorsement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAccountPosted",
                table: "Invoices",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsConvertedToContractService",
                table: "ContractServiceForEndorsements",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAccountPosted",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "IsConvertedToContractService",
                table: "ContractServiceForEndorsements");
        }
    }
}
