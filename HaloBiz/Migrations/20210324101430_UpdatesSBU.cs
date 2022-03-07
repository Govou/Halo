using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class UpdatesSBU : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Alias",
                table: "StrategicBusinessUnits",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Alias",
                table: "StrategicBusinessUnits");
        }
    }
}
