using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class addedRoleIdToUserProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "RoleId",
                table: "UserProfiles",
                type: "bigint",
                nullable: false,
                defaultValue: 2L);

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_RoleId",
                table: "UserProfiles",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Roles_RoleId",
                table: "UserProfiles",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Roles_RoleId",
                table: "UserProfiles");

            migrationBuilder.DropIndex(
                name: "IX_UserProfiles_RoleId",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "UserProfiles");
        }
    }
}
