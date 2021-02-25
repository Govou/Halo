using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class UpdatesAccountMasterModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountMasters_Branches_BranchId",
                table: "AccountMasters");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountMasters_Offices_OfficeId",
                table: "AccountMasters");

            migrationBuilder.AlterColumn<long>(
                name: "OfficeId",
                table: "AccountMasters",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "BranchId",
                table: "AccountMasters",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountMasters_Branches_BranchId",
                table: "AccountMasters",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountMasters_Offices_OfficeId",
                table: "AccountMasters",
                column: "OfficeId",
                principalTable: "Offices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountMasters_Branches_BranchId",
                table: "AccountMasters");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountMasters_Offices_OfficeId",
                table: "AccountMasters");

            migrationBuilder.AlterColumn<long>(
                name: "OfficeId",
                table: "AccountMasters",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "BranchId",
                table: "AccountMasters",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountMasters_Branches_BranchId",
                table: "AccountMasters",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountMasters_Offices_OfficeId",
                table: "AccountMasters",
                column: "OfficeId",
                principalTable: "Offices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
