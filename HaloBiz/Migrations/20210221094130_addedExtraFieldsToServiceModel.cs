using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class addedExtraFieldsToServiceModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CreatedById",
                table: "Services",
                type: "bigint",
                nullable: false,
                defaultValue: 31L);

            migrationBuilder.CreateIndex(
                name: "IX_Services_CreatedById",
                table: "Services",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Services_DivisionId",
                table: "Services",
                column: "DivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_OperatingEntityId",
                table: "Services",
                column: "OperatingEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_ServiceGroupId",
                table: "Services",
                column: "ServiceGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Divisions_DivisionId",
                table: "Services",
                column: "DivisionId",
                principalTable: "Divisions",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_OperatingEntities_OperatingEntityId",
                table: "Services",
                column: "OperatingEntityId",
                principalTable: "OperatingEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_ServiceGroups_ServiceGroupId",
                table: "Services",
                column: "ServiceGroupId",
                principalTable: "ServiceGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_UserProfiles_CreatedById",
                table: "Services",
                column: "CreatedById",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_Divisions_DivisionId",
                table: "Services");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_OperatingEntities_OperatingEntityId",
                table: "Services");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_ServiceGroups_ServiceGroupId",
                table: "Services");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_UserProfiles_CreatedById",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_CreatedById",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_DivisionId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_OperatingEntityId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_ServiceGroupId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Services");
        }
    }
}
