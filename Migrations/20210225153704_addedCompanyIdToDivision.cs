using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class addedCompanyIdToDivision : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CompanyId",
                table: "Divisions",
                type: "bigint",
                nullable: false,
                defaultValue: 1L);

            migrationBuilder.CreateIndex(
                name: "IX_Divisions_CompanyId",
                table: "Divisions",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Divisions_Companies_CompanyId",
                table: "Divisions",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Divisions_Companies_CompanyId",
                table: "Divisions");

            migrationBuilder.DropIndex(
                name: "IX_Divisions_CompanyId",
                table: "Divisions");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Divisions");
        }
    }
}
