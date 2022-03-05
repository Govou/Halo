using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class updatesAccountDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountDetails_Accounts_AccountId",
                table: "AccountDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountDetails_Branches_BranchId",
                table: "AccountDetails");

            migrationBuilder.AlterColumn<long>(
                name: "BranchId",
                table: "AccountDetails",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "AccountId",
                table: "AccountDetails",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountDetails_Accounts_AccountId",
                table: "AccountDetails",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountDetails_Branches_BranchId",
                table: "AccountDetails",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountDetails_Accounts_AccountId",
                table: "AccountDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountDetails_Branches_BranchId",
                table: "AccountDetails");

            migrationBuilder.AlterColumn<long>(
                name: "BranchId",
                table: "AccountDetails",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "AccountId",
                table: "AccountDetails",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountDetails_Accounts_AccountId",
                table: "AccountDetails",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountDetails_Branches_BranchId",
                table: "AccountDetails",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
