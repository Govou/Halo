using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class addedDescriptionForRoleAndRoleClaim : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Roles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "RoleClaims",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "RoleClaims");
        }
    }
}
