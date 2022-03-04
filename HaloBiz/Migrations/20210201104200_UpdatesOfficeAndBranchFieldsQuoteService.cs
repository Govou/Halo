using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class UpdatesOfficeAndBranchFieldsQuoteService : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeadDivisions_Branches_BranchId",
                table: "LeadDivisions");

            migrationBuilder.DropForeignKey(
                name: "FK_LeadDivisions_LeadDivisionContacts_PrimaryContactId",
                table: "LeadDivisions");

            migrationBuilder.DropForeignKey(
                name: "FK_LeadDivisions_Offices_OfficeId",
                table: "LeadDivisions");

            migrationBuilder.DropForeignKey(
                name: "FK_QuoteServices_Branches_BranchId",
                table: "QuoteServices");

            migrationBuilder.DropForeignKey(
                name: "FK_QuoteServices_Offices_OfficeId",
                table: "QuoteServices");

            migrationBuilder.AlterColumn<long>(
                name: "OfficeId",
                table: "QuoteServices",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "BranchId",
                table: "QuoteServices",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "PrimaryContactId",
                table: "LeadDivisions",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "OfficeId",
                table: "LeadDivisions",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "BranchId",
                table: "LeadDivisions",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_LeadDivisions_Branches_BranchId",
                table: "LeadDivisions",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LeadDivisions_LeadDivisionContacts_PrimaryContactId",
                table: "LeadDivisions",
                column: "PrimaryContactId",
                principalTable: "LeadDivisionContacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LeadDivisions_Offices_OfficeId",
                table: "LeadDivisions",
                column: "OfficeId",
                principalTable: "Offices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeadDivisions_Branches_BranchId",
                table: "LeadDivisions");

            migrationBuilder.DropForeignKey(
                name: "FK_LeadDivisions_LeadDivisionContacts_PrimaryContactId",
                table: "LeadDivisions");

            migrationBuilder.DropForeignKey(
                name: "FK_LeadDivisions_Offices_OfficeId",
                table: "LeadDivisions");

            migrationBuilder.DropForeignKey(
                name: "FK_QuoteServices_Branches_BranchId",
                table: "QuoteServices");

            migrationBuilder.DropForeignKey(
                name: "FK_QuoteServices_Offices_OfficeId",
                table: "QuoteServices");

            migrationBuilder.AlterColumn<long>(
                name: "OfficeId",
                table: "QuoteServices",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "BranchId",
                table: "QuoteServices",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "PrimaryContactId",
                table: "LeadDivisions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "OfficeId",
                table: "LeadDivisions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "BranchId",
                table: "LeadDivisions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LeadDivisions_Branches_BranchId",
                table: "LeadDivisions",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LeadDivisions_LeadDivisionContacts_PrimaryContactId",
                table: "LeadDivisions",
                column: "PrimaryContactId",
                principalTable: "LeadDivisionContacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LeadDivisions_Offices_OfficeId",
                table: "LeadDivisions",
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
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuoteServices_Offices_OfficeId",
                table: "QuoteServices",
                column: "OfficeId",
                principalTable: "Offices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
