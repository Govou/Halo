using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class UpdatesQuoteAndContractQuote : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DropOff",
                table: "QuoteServices");

            migrationBuilder.DropColumn(
                name: "GuardType",
                table: "QuoteServices");

            migrationBuilder.DropColumn(
                name: "IsGuardTouringRequired",
                table: "QuoteServices");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "QuoteServices");

            migrationBuilder.DropColumn(
                name: "PickUp",
                table: "QuoteServices");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "QuoteServices");

            migrationBuilder.DropColumn(
                name: "DropOff",
                table: "ContractService");

            migrationBuilder.DropColumn(
                name: "GuardType",
                table: "ContractService");

            migrationBuilder.DropColumn(
                name: "IsConvertedToContractService",
                table: "ContractService");

            migrationBuilder.DropColumn(
                name: "IsGuardTouringRequired",
                table: "ContractService");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "ContractService");

            migrationBuilder.DropColumn(
                name: "PickUp",
                table: "ContractService");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "ContractService");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "QuoteServices",
                newName: "TentativeProofOfConceptStartDate");

            migrationBuilder.RenameColumn(
                name: "ServiceDateTime",
                table: "QuoteServices",
                newName: "TentativeProofOfConceptEndDate");

            migrationBuilder.RenameColumn(
                name: "InvoiceSendInterval",
                table: "QuoteServices",
                newName: "InvoicingInterval");

            migrationBuilder.RenameColumn(
                name: "InvoiceSendDate",
                table: "QuoteServices",
                newName: "TentativeFulfillmentDate");

            migrationBuilder.RenameColumn(
                name: "EntryDateTime",
                table: "QuoteServices",
                newName: "TentativeDateOfSiteVisit");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "QuoteServices",
                newName: "TentativeDateForSiteSurvey");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "ContractService",
                newName: "TentativeProofOfConceptStartDate");

            migrationBuilder.RenameColumn(
                name: "ServiceDateTime",
                table: "ContractService",
                newName: "TentativeProofOfConceptEndDate");

            migrationBuilder.RenameColumn(
                name: "InvoiceSendInterval",
                table: "ContractService",
                newName: "InvoicingInterval");

            migrationBuilder.RenameColumn(
                name: "InvoiceSendDate",
                table: "ContractService",
                newName: "TentativeFulfillmentDate");

            migrationBuilder.RenameColumn(
                name: "EntryDateTime",
                table: "ContractService",
                newName: "TentativeDateOfSiteVisit");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "ContractService",
                newName: "TentativeDateForSiteSurvey");

            migrationBuilder.AlterColumn<double>(
                name: "VAT",
                table: "QuoteServices",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<string>(
                name: "ProblemStatement",
                table: "QuoteServices",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PaymentCycle",
                table: "QuoteServices",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Budget",
                table: "QuoteServices",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "BillableAmount",
                table: "QuoteServices",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<string>(
                name: "BeneficiaryIdentificationType",
                table: "QuoteServices",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BeneficiaryName",
                table: "QuoteServices",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BenificiaryIdentificationNumber",
                table: "QuoteServices",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ContractEndDate",
                table: "QuoteServices",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ContractStartDate",
                table: "QuoteServices",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DropoffDateTime",
                table: "QuoteServices",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Dropofflocation",
                table: "QuoteServices",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstInvoiceSendDate",
                table: "QuoteServices",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FulfillmentEndDate",
                table: "QuoteServices",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FulfillmentStartDate",
                table: "QuoteServices",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PaymentCycleInDays",
                table: "QuoteServices",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PickupDateTime",
                table: "QuoteServices",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PickupLocation",
                table: "QuoteServices",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ProgramCommencementDate",
                table: "QuoteServices",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ProgramDuration",
                table: "QuoteServices",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ProgramEndDate",
                table: "QuoteServices",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "UnitPrice",
                table: "QuoteServices",
                type: "float",
                nullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "VAT",
                table: "ContractService",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<string>(
                name: "ProblemStatement",
                table: "ContractService",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PaymentCycle",
                table: "ContractService",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Budget",
                table: "ContractService",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "BillableAmount",
                table: "ContractService",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<string>(
                name: "BeneficiaryIdentificationType",
                table: "ContractService",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BeneficiaryName",
                table: "ContractService",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BenificiaryIdentificationNumber",
                table: "ContractService",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ContractEndDate",
                table: "ContractService",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ContractStartDate",
                table: "ContractService",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DropoffDateTime",
                table: "ContractService",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Dropofflocation",
                table: "ContractService",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstInvoiceSendDate",
                table: "ContractService",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FulfillmentEndDate",
                table: "ContractService",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FulfillmentStartDate",
                table: "ContractService",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PaymentCycleInDays",
                table: "ContractService",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PickupDateTime",
                table: "ContractService",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PickupLocation",
                table: "ContractService",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ProgramCommencementDate",
                table: "ContractService",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ProgramDuration",
                table: "ContractService",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ProgramEndDate",
                table: "ContractService",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "UnitPrice",
                table: "ContractService",
                type: "float",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BeneficiaryIdentificationType",
                table: "QuoteServices");

            migrationBuilder.DropColumn(
                name: "BeneficiaryName",
                table: "QuoteServices");

            migrationBuilder.DropColumn(
                name: "BenificiaryIdentificationNumber",
                table: "QuoteServices");

            migrationBuilder.DropColumn(
                name: "ContractEndDate",
                table: "QuoteServices");

            migrationBuilder.DropColumn(
                name: "ContractStartDate",
                table: "QuoteServices");

            migrationBuilder.DropColumn(
                name: "DropoffDateTime",
                table: "QuoteServices");

            migrationBuilder.DropColumn(
                name: "Dropofflocation",
                table: "QuoteServices");

            migrationBuilder.DropColumn(
                name: "FirstInvoiceSendDate",
                table: "QuoteServices");

            migrationBuilder.DropColumn(
                name: "FulfillmentEndDate",
                table: "QuoteServices");

            migrationBuilder.DropColumn(
                name: "FulfillmentStartDate",
                table: "QuoteServices");

            migrationBuilder.DropColumn(
                name: "PaymentCycleInDays",
                table: "QuoteServices");

            migrationBuilder.DropColumn(
                name: "PickupDateTime",
                table: "QuoteServices");

            migrationBuilder.DropColumn(
                name: "PickupLocation",
                table: "QuoteServices");

            migrationBuilder.DropColumn(
                name: "ProgramCommencementDate",
                table: "QuoteServices");

            migrationBuilder.DropColumn(
                name: "ProgramDuration",
                table: "QuoteServices");

            migrationBuilder.DropColumn(
                name: "ProgramEndDate",
                table: "QuoteServices");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "QuoteServices");

            migrationBuilder.DropColumn(
                name: "BeneficiaryIdentificationType",
                table: "ContractService");

            migrationBuilder.DropColumn(
                name: "BeneficiaryName",
                table: "ContractService");

            migrationBuilder.DropColumn(
                name: "BenificiaryIdentificationNumber",
                table: "ContractService");

            migrationBuilder.DropColumn(
                name: "ContractEndDate",
                table: "ContractService");

            migrationBuilder.DropColumn(
                name: "ContractStartDate",
                table: "ContractService");

            migrationBuilder.DropColumn(
                name: "DropoffDateTime",
                table: "ContractService");

            migrationBuilder.DropColumn(
                name: "Dropofflocation",
                table: "ContractService");

            migrationBuilder.DropColumn(
                name: "FirstInvoiceSendDate",
                table: "ContractService");

            migrationBuilder.DropColumn(
                name: "FulfillmentEndDate",
                table: "ContractService");

            migrationBuilder.DropColumn(
                name: "FulfillmentStartDate",
                table: "ContractService");

            migrationBuilder.DropColumn(
                name: "PaymentCycleInDays",
                table: "ContractService");

            migrationBuilder.DropColumn(
                name: "PickupDateTime",
                table: "ContractService");

            migrationBuilder.DropColumn(
                name: "PickupLocation",
                table: "ContractService");

            migrationBuilder.DropColumn(
                name: "ProgramCommencementDate",
                table: "ContractService");

            migrationBuilder.DropColumn(
                name: "ProgramDuration",
                table: "ContractService");

            migrationBuilder.DropColumn(
                name: "ProgramEndDate",
                table: "ContractService");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "ContractService");

            migrationBuilder.RenameColumn(
                name: "TentativeProofOfConceptStartDate",
                table: "QuoteServices",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "TentativeProofOfConceptEndDate",
                table: "QuoteServices",
                newName: "ServiceDateTime");

            migrationBuilder.RenameColumn(
                name: "TentativeFulfillmentDate",
                table: "QuoteServices",
                newName: "InvoiceSendDate");

            migrationBuilder.RenameColumn(
                name: "TentativeDateOfSiteVisit",
                table: "QuoteServices",
                newName: "EntryDateTime");

            migrationBuilder.RenameColumn(
                name: "TentativeDateForSiteSurvey",
                table: "QuoteServices",
                newName: "EndDate");

            migrationBuilder.RenameColumn(
                name: "InvoicingInterval",
                table: "QuoteServices",
                newName: "InvoiceSendInterval");

            migrationBuilder.RenameColumn(
                name: "TentativeProofOfConceptStartDate",
                table: "ContractService",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "TentativeProofOfConceptEndDate",
                table: "ContractService",
                newName: "ServiceDateTime");

            migrationBuilder.RenameColumn(
                name: "TentativeFulfillmentDate",
                table: "ContractService",
                newName: "InvoiceSendDate");

            migrationBuilder.RenameColumn(
                name: "TentativeDateOfSiteVisit",
                table: "ContractService",
                newName: "EntryDateTime");

            migrationBuilder.RenameColumn(
                name: "TentativeDateForSiteSurvey",
                table: "ContractService",
                newName: "EndDate");

            migrationBuilder.RenameColumn(
                name: "InvoicingInterval",
                table: "ContractService",
                newName: "InvoiceSendInterval");

            migrationBuilder.AlterColumn<double>(
                name: "VAT",
                table: "QuoteServices",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProblemStatement",
                table: "QuoteServices",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "PaymentCycle",
                table: "QuoteServices",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Budget",
                table: "QuoteServices",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "BillableAmount",
                table: "QuoteServices",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DropOff",
                table: "QuoteServices",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GuardType",
                table: "QuoteServices",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsGuardTouringRequired",
                table: "QuoteServices",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "QuoteServices",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PickUp",
                table: "QuoteServices",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "QuoteServices",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<double>(
                name: "VAT",
                table: "ContractService",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProblemStatement",
                table: "ContractService",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "PaymentCycle",
                table: "ContractService",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Budget",
                table: "ContractService",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "BillableAmount",
                table: "ContractService",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DropOff",
                table: "ContractService",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GuardType",
                table: "ContractService",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsConvertedToContractService",
                table: "ContractService",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsGuardTouringRequired",
                table: "ContractService",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "ContractService",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PickUp",
                table: "ContractService",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "ContractService",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
