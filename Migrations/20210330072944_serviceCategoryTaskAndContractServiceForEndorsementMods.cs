using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class serviceCategoryTaskAndContractServiceForEndorsementMods : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "EndorsementTypeId",
                table: "ServiceCategoryTasks",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EndorsementDescription",
                table: "ContractServiceForEndorsements",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceCategoryTasks_EndorsementTypeId",
                table: "ServiceCategoryTasks",
                column: "EndorsementTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceCategoryTasks_EndorsementTypes_EndorsementTypeId",
                table: "ServiceCategoryTasks",
                column: "EndorsementTypeId",
                principalTable: "EndorsementTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceCategoryTasks_EndorsementTypes_EndorsementTypeId",
                table: "ServiceCategoryTasks");

            migrationBuilder.DropIndex(
                name: "IX_ServiceCategoryTasks_EndorsementTypeId",
                table: "ServiceCategoryTasks");

            migrationBuilder.DropColumn(
                name: "EndorsementTypeId",
                table: "ServiceCategoryTasks");

            migrationBuilder.DropColumn(
                name: "EndorsementDescription",
                table: "ContractServiceForEndorsements");
        }
    }
}
