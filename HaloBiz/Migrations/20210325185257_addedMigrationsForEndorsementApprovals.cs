using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class addedMigrationsForEndorsementApprovals : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ContractServiceForEndorsementId",
                table: "Approvals",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Approvals_ContractServiceForEndorsementId",
                table: "Approvals",
                column: "ContractServiceForEndorsementId");

            migrationBuilder.AddForeignKey(
                name: "FK_Approvals_ContractServiceForEndorsements_ContractServiceForEndorsementId",
                table: "Approvals",
                column: "ContractServiceForEndorsementId",
                principalTable: "ContractServiceForEndorsements",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Approvals_ContractServiceForEndorsements_ContractServiceForEndorsementId",
                table: "Approvals");

            migrationBuilder.DropIndex(
                name: "IX_Approvals_ContractServiceForEndorsementId",
                table: "Approvals");

            migrationBuilder.DropColumn(
                name: "ContractServiceForEndorsementId",
                table: "Approvals");
        }
    }
}
