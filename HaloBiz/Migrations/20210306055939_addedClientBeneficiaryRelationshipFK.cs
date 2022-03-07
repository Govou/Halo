using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class addedClientBeneficiaryRelationshipFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ClientBeneficiaries_RelationshipId",
                table: "ClientBeneficiaries",
                column: "RelationshipId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientBeneficiaries_Relationships_RelationshipId",
                table: "ClientBeneficiaries",
                column: "RelationshipId",
                principalTable: "Relationships",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientBeneficiaries_Relationships_RelationshipId",
                table: "ClientBeneficiaries");

            migrationBuilder.DropIndex(
                name: "IX_ClientBeneficiaries_RelationshipId",
                table: "ClientBeneficiaries");
        }
    }
}
