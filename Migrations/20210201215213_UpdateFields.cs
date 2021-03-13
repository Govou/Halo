using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class UpdateFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "InvoiceCycleInDays",
                table: "QuoteServices",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "UnitPrice",
                table: "Invoices",
                type: "float",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "Quantity",
                table: "Invoices",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<long>(
                name: "InvoiceCycleInDays",
                table: "ContractServices",
                type: "bigint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvoiceCycleInDays",
                table: "QuoteServices");

            migrationBuilder.DropColumn(
                name: "InvoiceCycleInDays",
                table: "ContractServices");

            migrationBuilder.AlterColumn<long>(
                name: "UnitPrice",
                table: "Invoices",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "Invoices",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
