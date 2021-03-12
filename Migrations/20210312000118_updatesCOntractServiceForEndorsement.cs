using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class updatesCOntractServiceForEndorsement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "BranchId",
                table: "ContractServices",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "OfficeId",
                table: "ContractServices",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "BranchId",
                table: "ContractServiceForEndorsements",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CustomerDivisionId",
                table: "ContractServiceForEndorsements",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "OfficeId",
                table: "ContractServiceForEndorsements",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PreviousContractServiceId",
                table: "ContractServiceForEndorsements",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContractServices_BranchId",
                table: "ContractServices",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractServices_OfficeId",
                table: "ContractServices",
                column: "OfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractServiceForEndorsements_BranchId",
                table: "ContractServiceForEndorsements",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractServiceForEndorsements_CustomerDivisionId",
                table: "ContractServiceForEndorsements",
                column: "CustomerDivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractServiceForEndorsements_OfficeId",
                table: "ContractServiceForEndorsements",
                column: "OfficeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContractServiceForEndorsements_Branches_BranchId",
                table: "ContractServiceForEndorsements",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ContractServiceForEndorsements_CustomerDivisions_CustomerDivisionId",
                table: "ContractServiceForEndorsements",
                column: "CustomerDivisionId",
                principalTable: "CustomerDivisions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContractServiceForEndorsements_Offices_OfficeId",
                table: "ContractServiceForEndorsements",
                column: "OfficeId",
                principalTable: "Offices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ContractServices_Branches_BranchId",
                table: "ContractServices",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ContractServices_Offices_OfficeId",
                table: "ContractServices",
                column: "OfficeId",
                principalTable: "Offices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContractServiceForEndorsements_Branches_BranchId",
                table: "ContractServiceForEndorsements");

            migrationBuilder.DropForeignKey(
                name: "FK_ContractServiceForEndorsements_CustomerDivisions_CustomerDivisionId",
                table: "ContractServiceForEndorsements");

            migrationBuilder.DropForeignKey(
                name: "FK_ContractServiceForEndorsements_Offices_OfficeId",
                table: "ContractServiceForEndorsements");

            migrationBuilder.DropForeignKey(
                name: "FK_ContractServices_Branches_BranchId",
                table: "ContractServices");

            migrationBuilder.DropForeignKey(
                name: "FK_ContractServices_Offices_OfficeId",
                table: "ContractServices");

            migrationBuilder.DropIndex(
                name: "IX_ContractServices_BranchId",
                table: "ContractServices");

            migrationBuilder.DropIndex(
                name: "IX_ContractServices_OfficeId",
                table: "ContractServices");

            migrationBuilder.DropIndex(
                name: "IX_ContractServiceForEndorsements_BranchId",
                table: "ContractServiceForEndorsements");

            migrationBuilder.DropIndex(
                name: "IX_ContractServiceForEndorsements_CustomerDivisionId",
                table: "ContractServiceForEndorsements");

            migrationBuilder.DropIndex(
                name: "IX_ContractServiceForEndorsements_OfficeId",
                table: "ContractServiceForEndorsements");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "ContractServices");

            migrationBuilder.DropColumn(
                name: "OfficeId",
                table: "ContractServices");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "ContractServiceForEndorsements");

            migrationBuilder.DropColumn(
                name: "CustomerDivisionId",
                table: "ContractServiceForEndorsements");

            migrationBuilder.DropColumn(
                name: "OfficeId",
                table: "ContractServiceForEndorsements");

            migrationBuilder.DropColumn(
                name: "PreviousContractServiceId",
                table: "ContractServiceForEndorsements");
        }
    }
}
