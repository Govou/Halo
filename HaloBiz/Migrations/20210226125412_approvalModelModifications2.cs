using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class approvalModelModifications2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Approvals_Quotes_QuoteId",
                table: "Approvals");

            migrationBuilder.DropForeignKey(
                name: "FK_Approvals_QuoteServices_QuoteServiceId",
                table: "Approvals");

            migrationBuilder.AlterColumn<long>(
                name: "QuoteServiceId",
                table: "Approvals",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "QuoteId",
                table: "Approvals",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_Approvals_Quotes_QuoteId",
                table: "Approvals",
                column: "QuoteId",
                principalTable: "Quotes",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Approvals_QuoteServices_QuoteServiceId",
                table: "Approvals",
                column: "QuoteServiceId",
                principalTable: "QuoteServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Approvals_Quotes_QuoteId",
                table: "Approvals");

            migrationBuilder.DropForeignKey(
                name: "FK_Approvals_QuoteServices_QuoteServiceId",
                table: "Approvals");

            migrationBuilder.AlterColumn<long>(
                name: "QuoteServiceId",
                table: "Approvals",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "QuoteId",
                table: "Approvals",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Approvals_Quotes_QuoteId",
                table: "Approvals",
                column: "QuoteId",
                principalTable: "Quotes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Approvals_QuoteServices_QuoteServiceId",
                table: "Approvals",
                column: "QuoteServiceId",
                principalTable: "QuoteServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
