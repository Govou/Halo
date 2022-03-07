using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class MordifiesContractQuoteQuoteServiceAndContractServiceModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClosureDocuments_ContractService_ContractServiceId",
                table: "ClosureDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_Contract_CustomerDivisions_CustomerDivisionId",
                table: "Contract");

            migrationBuilder.DropForeignKey(
                name: "FK_Contract_Quotes_QuoteId",
                table: "Contract");

            migrationBuilder.DropForeignKey(
                name: "FK_Contract_UserProfiles_CreatedById",
                table: "Contract");

            migrationBuilder.DropForeignKey(
                name: "FK_ContractService_Branches_BranchId",
                table: "ContractService");

            migrationBuilder.DropForeignKey(
                name: "FK_ContractService_Contract_ContractId",
                table: "ContractService");

            migrationBuilder.DropForeignKey(
                name: "FK_ContractService_Offices_OfficeId",
                table: "ContractService");

            migrationBuilder.DropForeignKey(
                name: "FK_ContractService_Quotes_QuoteId",
                table: "ContractService");

            migrationBuilder.DropForeignKey(
                name: "FK_ContractService_QuoteServices_QuoteServiceId",
                table: "ContractService");

            migrationBuilder.DropForeignKey(
                name: "FK_ContractService_Services_ServiceId",
                table: "ContractService");

            migrationBuilder.DropForeignKey(
                name: "FK_ContractService_UserProfiles_CreatedById",
                table: "ContractService");

            migrationBuilder.DropForeignKey(
                name: "FK_QuoteServices_Contract_ContractId",
                table: "QuoteServices");

            migrationBuilder.DropForeignKey(
                name: "FK_SBUToContractServiceProportions_ContractService_ContractServiceId",
                table: "SBUToContractServiceProportions");

            migrationBuilder.DropIndex(
                name: "IX_QuoteServices_ContractId",
                table: "QuoteServices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContractService",
                table: "ContractService");

            migrationBuilder.DropIndex(
                name: "IX_ContractService_QuoteId",
                table: "ContractService");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Contract",
                table: "Contract");

            migrationBuilder.DropColumn(
                name: "ContractId",
                table: "QuoteServices");

            migrationBuilder.DropColumn(
                name: "QuoteId",
                table: "ContractService");

            migrationBuilder.RenameTable(
                name: "ContractService",
                newName: "ContractServices");

            migrationBuilder.RenameTable(
                name: "Contract",
                newName: "Contracts");

            migrationBuilder.RenameIndex(
                name: "IX_ContractService_ServiceId",
                table: "ContractServices",
                newName: "IX_ContractServices_ServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_ContractService_QuoteServiceId",
                table: "ContractServices",
                newName: "IX_ContractServices_QuoteServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_ContractService_OfficeId",
                table: "ContractServices",
                newName: "IX_ContractServices_OfficeId");

            migrationBuilder.RenameIndex(
                name: "IX_ContractService_CreatedById",
                table: "ContractServices",
                newName: "IX_ContractServices_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_ContractService_ContractId",
                table: "ContractServices",
                newName: "IX_ContractServices_ContractId");

            migrationBuilder.RenameIndex(
                name: "IX_ContractService_BranchId",
                table: "ContractServices",
                newName: "IX_ContractServices_BranchId");

            migrationBuilder.RenameIndex(
                name: "IX_Contract_QuoteId",
                table: "Contracts",
                newName: "IX_Contracts_QuoteId");

            migrationBuilder.RenameIndex(
                name: "IX_Contract_CustomerDivisionId",
                table: "Contracts",
                newName: "IX_Contracts_CustomerDivisionId");

            migrationBuilder.RenameIndex(
                name: "IX_Contract_CreatedById",
                table: "Contracts",
                newName: "IX_Contracts_CreatedById");

            migrationBuilder.AddColumn<string>(
                name: "ReferenceNumber",
                table: "QuoteServices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "LeadDivisionContacts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "ContractServices",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "ContractServices",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<long>(
                name: "ContractId",
                table: "ContractServices",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferenceNo",
                table: "ContractServices",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Contracts",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Contracts",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContractServices",
                table: "ContractServices",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Contracts",
                table: "Contracts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClosureDocuments_ContractServices_ContractServiceId",
                table: "ClosureDocuments",
                column: "ContractServiceId",
                principalTable: "ContractServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_CustomerDivisions_CustomerDivisionId",
                table: "Contracts",
                column: "CustomerDivisionId",
                principalTable: "CustomerDivisions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Quotes_QuoteId",
                table: "Contracts",
                column: "QuoteId",
                principalTable: "Quotes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_UserProfiles_CreatedById",
                table: "Contracts",
                column: "CreatedById",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_ContractServices_Branches_BranchId",
                table: "ContractServices",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_ContractServices_Contracts_ContractId",
                table: "ContractServices",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContractServices_Offices_OfficeId",
                table: "ContractServices",
                column: "OfficeId",
                principalTable: "Offices",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_ContractServices_QuoteServices_QuoteServiceId",
                table: "ContractServices",
                column: "QuoteServiceId",
                principalTable: "QuoteServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContractServices_Services_ServiceId",
                table: "ContractServices",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContractServices_UserProfiles_CreatedById",
                table: "ContractServices",
                column: "CreatedById",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_SBUToContractServiceProportions_ContractServices_ContractServiceId",
                table: "SBUToContractServiceProportions",
                column: "ContractServiceId",
                principalTable: "ContractServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClosureDocuments_ContractServices_ContractServiceId",
                table: "ClosureDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_CustomerDivisions_CustomerDivisionId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Quotes_QuoteId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_UserProfiles_CreatedById",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_ContractServices_Branches_BranchId",
                table: "ContractServices");

            migrationBuilder.DropForeignKey(
                name: "FK_ContractServices_Contracts_ContractId",
                table: "ContractServices");

            migrationBuilder.DropForeignKey(
                name: "FK_ContractServices_Offices_OfficeId",
                table: "ContractServices");

            migrationBuilder.DropForeignKey(
                name: "FK_ContractServices_QuoteServices_QuoteServiceId",
                table: "ContractServices");

            migrationBuilder.DropForeignKey(
                name: "FK_ContractServices_Services_ServiceId",
                table: "ContractServices");

            migrationBuilder.DropForeignKey(
                name: "FK_ContractServices_UserProfiles_CreatedById",
                table: "ContractServices");

            migrationBuilder.DropForeignKey(
                name: "FK_SBUToContractServiceProportions_ContractServices_ContractServiceId",
                table: "SBUToContractServiceProportions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContractServices",
                table: "ContractServices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Contracts",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "ReferenceNumber",
                table: "QuoteServices");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "LeadDivisionContacts");

            migrationBuilder.DropColumn(
                name: "ReferenceNo",
                table: "ContractServices");

            migrationBuilder.RenameTable(
                name: "ContractServices",
                newName: "ContractService");

            migrationBuilder.RenameTable(
                name: "Contracts",
                newName: "Contract");

            migrationBuilder.RenameIndex(
                name: "IX_ContractServices_ServiceId",
                table: "ContractService",
                newName: "IX_ContractService_ServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_ContractServices_QuoteServiceId",
                table: "ContractService",
                newName: "IX_ContractService_QuoteServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_ContractServices_OfficeId",
                table: "ContractService",
                newName: "IX_ContractService_OfficeId");

            migrationBuilder.RenameIndex(
                name: "IX_ContractServices_CreatedById",
                table: "ContractService",
                newName: "IX_ContractService_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_ContractServices_ContractId",
                table: "ContractService",
                newName: "IX_ContractService_ContractId");

            migrationBuilder.RenameIndex(
                name: "IX_ContractServices_BranchId",
                table: "ContractService",
                newName: "IX_ContractService_BranchId");

            migrationBuilder.RenameIndex(
                name: "IX_Contracts_QuoteId",
                table: "Contract",
                newName: "IX_Contract_QuoteId");

            migrationBuilder.RenameIndex(
                name: "IX_Contracts_CustomerDivisionId",
                table: "Contract",
                newName: "IX_Contract_CustomerDivisionId");

            migrationBuilder.RenameIndex(
                name: "IX_Contracts_CreatedById",
                table: "Contract",
                newName: "IX_Contract_CreatedById");

            migrationBuilder.AddColumn<long>(
                name: "ContractId",
                table: "QuoteServices",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "ContractService",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "ContractService",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<long>(
                name: "ContractId",
                table: "ContractService",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "QuoteId",
                table: "ContractService",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Contract",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Contract",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContractService",
                table: "ContractService",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Contract",
                table: "Contract",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_QuoteServices_ContractId",
                table: "QuoteServices",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractService_QuoteId",
                table: "ContractService",
                column: "QuoteId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClosureDocuments_ContractService_ContractServiceId",
                table: "ClosureDocuments",
                column: "ContractServiceId",
                principalTable: "ContractService",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contract_CustomerDivisions_CustomerDivisionId",
                table: "Contract",
                column: "CustomerDivisionId",
                principalTable: "CustomerDivisions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contract_Quotes_QuoteId",
                table: "Contract",
                column: "QuoteId",
                principalTable: "Quotes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contract_UserProfiles_CreatedById",
                table: "Contract",
                column: "CreatedById",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_ContractService_Branches_BranchId",
                table: "ContractService",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_ContractService_Contract_ContractId",
                table: "ContractService",
                column: "ContractId",
                principalTable: "Contract",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ContractService_Offices_OfficeId",
                table: "ContractService",
                column: "OfficeId",
                principalTable: "Offices",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_ContractService_Quotes_QuoteId",
                table: "ContractService",
                column: "QuoteId",
                principalTable: "Quotes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContractService_QuoteServices_QuoteServiceId",
                table: "ContractService",
                column: "QuoteServiceId",
                principalTable: "QuoteServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContractService_Services_ServiceId",
                table: "ContractService",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContractService_UserProfiles_CreatedById",
                table: "ContractService",
                column: "CreatedById",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuoteServices_Contract_ContractId",
                table: "QuoteServices",
                column: "ContractId",
                principalTable: "Contract",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SBUToContractServiceProportions_ContractService_ContractServiceId",
                table: "SBUToContractServiceProportions",
                column: "ContractServiceId",
                principalTable: "ContractService",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
