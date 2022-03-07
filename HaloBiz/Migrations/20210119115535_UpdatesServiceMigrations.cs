using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class UpdatesServiceMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BillableAmount",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "Discount",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "VAT",
                table: "Services");

            migrationBuilder.AddColumn<bool>(
                name: "IsPublished",
                table: "Services",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRequestedForPublish",
                table: "Services",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PublishedApprovedStatus",
                table: "Services",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ServiceCode",
                table: "Services",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TargetId",
                table: "Services",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Services_TargetId",
                table: "Services",
                column: "TargetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Targets_TargetId",
                table: "Services",
                column: "TargetId",
                principalTable: "Targets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_Targets_TargetId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_TargetId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "IsPublished",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "IsRequestedForPublish",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "PublishedApprovedStatus",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "ServiceCode",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "TargetId",
                table: "Services");

            migrationBuilder.AddColumn<double>(
                name: "BillableAmount",
                table: "Services",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Discount",
                table: "Services",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Services",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Services",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "VAT",
                table: "Services",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
