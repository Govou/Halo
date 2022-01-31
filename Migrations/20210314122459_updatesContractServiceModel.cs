using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class updatesContractServiceModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContractServices_QuoteServices_QuoteServiceId",
                table: "ContractServices");

            migrationBuilder.AlterColumn<long>(
                name: "QuoteServiceId",
                table: "ContractServices",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_ContractServices_QuoteServices_QuoteServiceId",
                table: "ContractServices",
                column: "QuoteServiceId",
                principalTable: "QuoteServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContractServices_QuoteServices_QuoteServiceId",
                table: "ContractServices");

            migrationBuilder.AlterColumn<long>(
                name: "QuoteServiceId",
                table: "ContractServices",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ContractServices_QuoteServices_QuoteServiceId",
                table: "ContractServices",
                column: "QuoteServiceId",
                principalTable: "QuoteServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
