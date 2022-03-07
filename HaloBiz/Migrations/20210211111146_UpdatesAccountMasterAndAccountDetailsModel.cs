using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class UpdatesAccountMasterAndAccountDetailsModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountMasters_Accounts_AccountId",
                table: "AccountMasters");

            migrationBuilder.DropIndex(
                name: "IX_AccountMasters_AccountId",
                table: "AccountMasters");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "AccountMasters");

            migrationBuilder.DropColumn(
                name: "AccountMasterAlias",
                table: "AccountMasters");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AccountMasters");

            migrationBuilder.DropColumn(
                name: "AccountDetailsAlias",
                table: "AccountDetails");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AccountDetails");

            migrationBuilder.AddColumn<double>(
                name: "Value",
                table: "AccountMasters",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<long>(
                name: "AccountId",
                table: "AccountDetails",
                type: "bigint",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AccountDetails_AccountId",
                table: "AccountDetails",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountDetails_Accounts_AccountId",
                table: "AccountDetails",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountDetails_Accounts_AccountId",
                table: "AccountDetails");

            migrationBuilder.DropIndex(
                name: "IX_AccountDetails_AccountId",
                table: "AccountDetails");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "AccountMasters");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "AccountDetails");

            migrationBuilder.AddColumn<long>(
                name: "AccountId",
                table: "AccountMasters",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "AccountMasterAlias",
                table: "AccountMasters",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AccountMasters",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "AccountDetailsAlias",
                table: "AccountDetails",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AccountDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_AccountMasters_AccountId",
                table: "AccountMasters",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountMasters_Accounts_AccountId",
                table: "AccountMasters",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
