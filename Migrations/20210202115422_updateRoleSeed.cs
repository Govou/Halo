using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class updateRoleSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE Roles SET Description = 'Super Admin Role' WHERE Id=1");
            migrationBuilder.Sql("UPDATE Roles SET Description = 'UnAssigned Role' WHERE Id=2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
