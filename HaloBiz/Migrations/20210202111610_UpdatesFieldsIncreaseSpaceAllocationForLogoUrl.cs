using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class UpdatesFieldsIncreaseSpaceAllocationForLogoUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContractServices_Branches_BranchId",
                table: "ContractServices");

            migrationBuilder.DropForeignKey(
                name: "FK_ContractServices_Offices_OfficeId",
                table: "ContractServices");

            migrationBuilder.DropForeignKey(
                name: "FK_QuoteServices_Branches_BranchId",
                table: "QuoteServices");

            migrationBuilder.DropForeignKey(
                name: "FK_QuoteServices_Offices_OfficeId",
                table: "QuoteServices");

            migrationBuilder.DropIndex(
                name: "IX_QuoteServices_BranchId",
                table: "QuoteServices");

            migrationBuilder.DropIndex(
                name: "IX_QuoteServices_OfficeId",
                table: "QuoteServices");

            migrationBuilder.DropIndex(
                name: "IX_ContractServices_BranchId",
                table: "ContractServices");

            migrationBuilder.DropIndex(
                name: "IX_ContractServices_OfficeId",
                table: "ContractServices");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "QuoteServices");

            migrationBuilder.DropColumn(
                name: "OfficeId",
                table: "QuoteServices");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "ContractServices");

            migrationBuilder.DropColumn(
                name: "OfficeId",
                table: "ContractServices");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Services",
                type: "nvarchar(max)",
                maxLength: 5000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LogoUrl",
                table: "Leads",
                type: "nvarchar(max)",
                maxLength: 5000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LogoUrl",
                table: "LeadDivisions",
                type: "nvarchar(max)",
                maxLength: 5000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LogoUrl",
                table: "Customers",
                type: "nvarchar(max)",
                maxLength: 5000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "LogoUrl",
                table: "CustomerDivisions",
                type: "nvarchar(max)",
                maxLength: 5000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Services",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 5000,
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "BranchId",
                table: "QuoteServices",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "OfficeId",
                table: "QuoteServices",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LogoUrl",
                table: "Leads",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 5000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LogoUrl",
                table: "LeadDivisions",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 5000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LogoUrl",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 5000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LogoUrl",
                table: "CustomerDivisions",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 5000,
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "BranchId",
                table: "ContractServices",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "OfficeId",
                table: "ContractServices",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_QuoteServices_BranchId",
                table: "QuoteServices",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_QuoteServices_OfficeId",
                table: "QuoteServices",
                column: "OfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractServices_BranchId",
                table: "ContractServices",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractServices_OfficeId",
                table: "ContractServices",
                column: "OfficeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContractServices_Branches_BranchId",
                table: "ContractServices",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContractServices_Offices_OfficeId",
                table: "ContractServices",
                column: "OfficeId",
                principalTable: "Offices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuoteServices_Branches_BranchId",
                table: "QuoteServices",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QuoteServices_Offices_OfficeId",
                table: "QuoteServices",
                column: "OfficeId",
                principalTable: "Offices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
