using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class AddsSeedUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("SET IDENTITY_INSERT UserProfiles ON"); 
            migrationBuilder.Sql("INSERT INTO UserProfiles (Id, FirstName, LastName, Email, MobileNumber, ImageUrl, StaffId, ProfileStatus, DateOfBirth) VALUES (1, 'Seeder', 'Seeder', 'seeder@halogen-group.com', '09000000000','https://firebasestorage.googleapis.com/v0/b/halo-biz.appspot.com/o/serviceImage%2FGuard.png?alt=media&token=2c6c2279-5508-4d8a-8169-a468595bb2f6',1, 0, '2021-02-10 12:12:03.2800000')"); 
            migrationBuilder.Sql("SET IDENTITY_INSERT UserProfiles OFF"); 
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
