using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class AddsRefernceNumberModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TimeMovedToLeadCloseure",
                table: "Leads",
                newName: "TimeMovedToLeadClosure");

            migrationBuilder.CreateTable(
                name: "ReferenceNumbers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceNo = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReferenceNumbers", x => x.Id);
                });
            migrationBuilder.Sql("SET IDENTITY_INSERT ReferenceNumbers ON"); 
            migrationBuilder.Sql("INSERT INTO ReferenceNumbers (Id, ReferenceNo) VALUES (1, 1)"); 

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReferenceNumbers");

            migrationBuilder.RenameColumn(
                name: "TimeMovedToLeadClosure",
                table: "Leads",
                newName: "TimeMovedToLeadCloseure");
        }
    }
}
