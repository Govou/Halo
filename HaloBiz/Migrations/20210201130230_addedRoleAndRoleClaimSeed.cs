using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class addedRoleAndRoleClaimSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("SET IDENTITY_INSERT Roles ON");

            migrationBuilder.Sql("INSERT INTO Roles (Id, Name) VALUES(1, 'SuperAdmin')");
            migrationBuilder.Sql("INSERT INTO Roles (Id, Name) VALUES(2, 'UnAssigned')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
