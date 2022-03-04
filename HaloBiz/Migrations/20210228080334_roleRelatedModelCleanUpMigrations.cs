using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class roleRelatedModelCleanUpMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CanAdd",
                table: "RoleClaims",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanDelete",
                table: "RoleClaims",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanUpdate",
                table: "RoleClaims",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanView",
                table: "RoleClaims",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ClaimEnum",
                table: "RoleClaims",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ClaimEnum",
                table: "Claims",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanAdd",
                table: "RoleClaims");

            migrationBuilder.DropColumn(
                name: "CanDelete",
                table: "RoleClaims");

            migrationBuilder.DropColumn(
                name: "CanUpdate",
                table: "RoleClaims");

            migrationBuilder.DropColumn(
                name: "CanView",
                table: "RoleClaims");

            migrationBuilder.DropColumn(
                name: "ClaimEnum",
                table: "RoleClaims");

            migrationBuilder.DropColumn(
                name: "ClaimEnum",
                table: "Claims");
        }
    }
}
