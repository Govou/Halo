using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class ChangeRelationshipBtwAccountMasterAndCustomerDivision : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AccountMasters_CustomerDivisionId",
                table: "AccountMasters");

            migrationBuilder.CreateIndex(
                name: "IX_AccountMasters_CustomerDivisionId",
                table: "AccountMasters",
                column: "CustomerDivisionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AccountMasters_CustomerDivisionId",
                table: "AccountMasters");

            migrationBuilder.CreateIndex(
                name: "IX_AccountMasters_CustomerDivisionId",
                table: "AccountMasters",
                column: "CustomerDivisionId",
                unique: true,
                filter: "[CustomerDivisionId] IS NOT NULL");
        }
    }
}
