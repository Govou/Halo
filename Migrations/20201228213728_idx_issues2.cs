using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class idx_issues2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Branches_UserProfiles_HeadId1",
                table: "Branches");

            migrationBuilder.DropIndex(
                name: "IX_Branches_HeadId1",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "HeadId1",
                table: "Branches");

            migrationBuilder.AlterColumn<long>(
                name: "HeadId",
                table: "Branches",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_HeadId",
                table: "Branches",
                column: "HeadId");

            migrationBuilder.AddForeignKey(
                name: "FK_Branches_UserProfiles_HeadId",
                table: "Branches",
                column: "HeadId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Branches_UserProfiles_HeadId",
                table: "Branches");

            migrationBuilder.DropIndex(
                name: "IX_Branches_HeadId",
                table: "Branches");

            migrationBuilder.AlterColumn<int>(
                name: "HeadId",
                table: "Branches",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "HeadId1",
                table: "Branches",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Branches_HeadId1",
                table: "Branches",
                column: "HeadId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Branches_UserProfiles_HeadId1",
                table: "Branches",
                column: "HeadId1",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
