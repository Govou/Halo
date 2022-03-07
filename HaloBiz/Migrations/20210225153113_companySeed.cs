using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class companySeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("SET IDENTITY_INSERT Companies ON");

            migrationBuilder.Sql("INSERT INTO Companies (Id, Name, Description, Address, IsDeleted) VALUES(1, 'Halogen', '', '', 0)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
