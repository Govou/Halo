using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class AddsLAMSTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RCNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GroupTypeId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_GroupType_GroupTypeId",
                        column: x => x.GroupTypeId,
                        principalTable: "GroupType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Customers_UserProfiles_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "DropReasons",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DropReasons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DropReasons_UserProfiles_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "LeadContacts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MobileNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Designation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeadContacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeadContacts_UserProfiles_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "LeadDivisionContacts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MobileNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Designation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeadDivisionContacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeadDivisionContacts_UserProfiles_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "CustomerDivisions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Industry = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RCNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DivisionName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LogoUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerDivisions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerDivisions_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerDivisions_UserProfiles_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Leads",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LeadTypeId = table.Column<long>(type: "bigint", nullable: false),
                    LeadOriginId = table.Column<long>(type: "bigint", nullable: false),
                    Industry = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RCNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GroupName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    GroupTypeId = table.Column<long>(type: "bigint", nullable: false),
                    LogoUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    LeadCaptureStatus = table.Column<bool>(type: "bit", nullable: false),
                    LeadQualificationStatus = table.Column<bool>(type: "bit", nullable: false),
                    LeadOpportunityStatus = table.Column<bool>(type: "bit", nullable: false),
                    LeadConversionStatus = table.Column<bool>(type: "bit", nullable: false),
                    IsLeadDropped = table.Column<bool>(type: "bit", nullable: false),
                    DropLearning = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DropReasonId = table.Column<long>(type: "bigint", nullable: true),
                    PrimaryContactId = table.Column<long>(type: "bigint", nullable: false),
                    SecondaryContactId = table.Column<long>(type: "bigint", nullable: true),
                    CustomerId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leads", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Leads_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Leads_DropReasons_DropReasonId",
                        column: x => x.DropReasonId,
                        principalTable: "DropReasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Leads_GroupType_GroupTypeId",
                        column: x => x.GroupTypeId,
                        principalTable: "GroupType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Leads_LeadContacts_PrimaryContactId",
                        column: x => x.PrimaryContactId,
                        principalTable: "LeadContacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Leads_LeadContacts_SecondaryContactId",
                        column: x => x.SecondaryContactId,
                        principalTable: "LeadContacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Leads_LeadOrigins_LeadOriginId",
                        column: x => x.LeadOriginId,
                        principalTable: "LeadOrigins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Leads_LeadTypes_LeadTypeId",
                        column: x => x.LeadTypeId,
                        principalTable: "LeadTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Leads_UserProfiles_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "LeadDivisions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeadOriginId = table.Column<long>(type: "bigint", maxLength: 50, nullable: false),
                    Industry = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RCNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DivisionName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LogoUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PrimaryContactId = table.Column<long>(type: "bigint", nullable: false),
                    SecondaryContactId = table.Column<long>(type: "bigint", nullable: true),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    OfficeId = table.Column<long>(type: "bigint", nullable: false),
                    LeadId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeadDivisions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeadDivisions_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeadDivisions_LeadDivisionContacts_PrimaryContactId",
                        column: x => x.PrimaryContactId,
                        principalTable: "LeadDivisionContacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeadDivisions_LeadDivisionContacts_SecondaryContactId",
                        column: x => x.SecondaryContactId,
                        principalTable: "LeadDivisionContacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LeadDivisions_LeadOrigins_LeadOriginId",
                        column: x => x.LeadOriginId,
                        principalTable: "LeadOrigins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_LeadDivisions_Leads_LeadId",
                        column: x => x.LeadId,
                        principalTable: "Leads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeadDivisions_Offices_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "Offices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeadDivisions_UserProfiles_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "LeadKeyPeople",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeadId = table.Column<long>(type: "bigint", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MobileNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Designation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeadKeyPeople", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeadKeyPeople_Leads_LeadId",
                        column: x => x.LeadId,
                        principalTable: "Leads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeadKeyPeople_UserProfiles_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "LeadDivisionKeyPeople",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeadDivisionId = table.Column<long>(type: "bigint", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MobileNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Designation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeadDivisionKeyPeople", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeadDivisionKeyPeople_LeadDivisions_LeadDivisionId",
                        column: x => x.LeadDivisionId,
                        principalTable: "LeadDivisions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeadDivisionKeyPeople_UserProfiles_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Quotes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LeadDivisionId = table.Column<long>(type: "bigint", nullable: false),
                    IsConvertedToContract = table.Column<bool>(type: "bit", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Quotes_LeadDivisions_LeadDivisionId",
                        column: x => x.LeadDivisionId,
                        principalTable: "LeadDivisions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Quotes_UserProfiles_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Contract",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CustomerDivisionId = table.Column<long>(type: "bigint", nullable: false),
                    QuoteId = table.Column<long>(type: "bigint", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contract", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contract_CustomerDivisions_CustomerDivisionId",
                        column: x => x.CustomerDivisionId,
                        principalTable: "CustomerDivisions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Contract_Quotes_QuoteId",
                        column: x => x.QuoteId,
                        principalTable: "Quotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Contract_UserProfiles_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "QuoteServices",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActivationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GuardType = table.Column<int>(type: "int", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    EntryDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PickUp = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DropOff = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ServiceDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Quantity = table.Column<long>(type: "bigint", nullable: false),
                    ProblemStatement = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsGuardTouringRequired = table.Column<bool>(type: "bit", nullable: false),
                    Discount = table.Column<double>(type: "float", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    VAT = table.Column<double>(type: "float", nullable: false),
                    BillableAmount = table.Column<double>(type: "float", nullable: false),
                    Budget = table.Column<double>(type: "float", nullable: false),
                    PaymentCycle = table.Column<long>(type: "bigint", nullable: true),
                    InvoiceSendInterval = table.Column<int>(type: "int", nullable: true),
                    InvoiceSendDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsConvertedToContractService = table.Column<bool>(type: "bit", nullable: false),
                    ServiceId = table.Column<long>(type: "bigint", nullable: false),
                    QuoteId = table.Column<long>(type: "bigint", nullable: false),
                    OfficeId = table.Column<long>(type: "bigint", nullable: false),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ContractId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuoteServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuoteServices_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_QuoteServices_Contract_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contract",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuoteServices_Offices_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "Offices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_QuoteServices_Quotes_QuoteId",
                        column: x => x.QuoteId,
                        principalTable: "Quotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_QuoteServices_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_QuoteServices_UserProfiles_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ContractService",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActivationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GuardType = table.Column<int>(type: "int", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    EntryDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PickUp = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DropOff = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ServiceDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Quantity = table.Column<long>(type: "bigint", nullable: false),
                    ProblemStatement = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsGuardTouringRequired = table.Column<bool>(type: "bit", nullable: false),
                    Discount = table.Column<double>(type: "float", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    VAT = table.Column<double>(type: "float", nullable: false),
                    BillableAmount = table.Column<double>(type: "float", nullable: false),
                    Budget = table.Column<double>(type: "float", nullable: false),
                    PaymentCycle = table.Column<long>(type: "bigint", nullable: true),
                    InvoiceSendInterval = table.Column<int>(type: "int", nullable: true),
                    InvoiceSendDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsConvertedToContractService = table.Column<bool>(type: "bit", nullable: false),
                    ServiceId = table.Column<long>(type: "bigint", nullable: false),
                    QuoteId = table.Column<long>(type: "bigint", nullable: false),
                    OfficeId = table.Column<long>(type: "bigint", nullable: false),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    QuoteServiceId = table.Column<long>(type: "bigint", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ContractId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractService", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContractService_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ContractService_Contract_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contract",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContractService_Offices_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "Offices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ContractService_Quotes_QuoteId",
                        column: x => x.QuoteId,
                        principalTable: "Quotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ContractService_QuoteServices_QuoteServiceId",
                        column: x => x.QuoteServiceId,
                        principalTable: "QuoteServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ContractService_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ContractService_UserProfiles_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "QuoteServiceDocuments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuoteServiceId = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Caption = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DocumentUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuoteServiceDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuoteServiceDocuments_QuoteServices_QuoteServiceId",
                        column: x => x.QuoteServiceId,
                        principalTable: "QuoteServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_QuoteServiceDocuments_UserProfiles_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "SBUToQuoteServiceProportions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuoteServiceId = table.Column<long>(type: "bigint", nullable: false),
                    Proportion = table.Column<double>(type: "float", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StrategicBusinessUnitId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SBUToQuoteServiceProportions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SBUToQuoteServiceProportions_QuoteServices_QuoteServiceId",
                        column: x => x.QuoteServiceId,
                        principalTable: "QuoteServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SBUToQuoteServiceProportions_StrategicBusinessUnits_StrategicBusinessUnitId",
                        column: x => x.StrategicBusinessUnitId,
                        principalTable: "StrategicBusinessUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SBUToQuoteServiceProportions_UserProfiles_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ClosureDocuments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractServiceId = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Caption = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DocumentUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClosureDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClosureDocuments_ContractService_ContractServiceId",
                        column: x => x.ContractServiceId,
                        principalTable: "ContractService",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClosureDocuments_UserProfiles_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "SBUToContractServiceProportions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractServiceId = table.Column<long>(type: "bigint", nullable: false),
                    Proportion = table.Column<double>(type: "float", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StrategicBusinessUnitId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SBUToContractServiceProportions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SBUToContractServiceProportions_ContractService_ContractServiceId",
                        column: x => x.ContractServiceId,
                        principalTable: "ContractService",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SBUToContractServiceProportions_StrategicBusinessUnits_StrategicBusinessUnitId",
                        column: x => x.StrategicBusinessUnitId,
                        principalTable: "StrategicBusinessUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SBUToContractServiceProportions_UserProfiles_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClosureDocuments_ContractServiceId",
                table: "ClosureDocuments",
                column: "ContractServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ClosureDocuments_CreatedById",
                table: "ClosureDocuments",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Contract_CreatedById",
                table: "Contract",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Contract_CustomerDivisionId",
                table: "Contract",
                column: "CustomerDivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_Contract_QuoteId",
                table: "Contract",
                column: "QuoteId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractService_BranchId",
                table: "ContractService",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractService_ContractId",
                table: "ContractService",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractService_CreatedById",
                table: "ContractService",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ContractService_OfficeId",
                table: "ContractService",
                column: "OfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractService_QuoteId",
                table: "ContractService",
                column: "QuoteId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractService_QuoteServiceId",
                table: "ContractService",
                column: "QuoteServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractService_ServiceId",
                table: "ContractService",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerDivisions_CreatedById",
                table: "CustomerDivisions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerDivisions_CustomerId",
                table: "CustomerDivisions",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CreatedById",
                table: "Customers",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_GroupTypeId",
                table: "Customers",
                column: "GroupTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DropReasons_CreatedById",
                table: "DropReasons",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_LeadContacts_CreatedById",
                table: "LeadContacts",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_LeadDivisionContacts_CreatedById",
                table: "LeadDivisionContacts",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_LeadDivisionKeyPeople_CreatedById",
                table: "LeadDivisionKeyPeople",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_LeadDivisionKeyPeople_LeadDivisionId",
                table: "LeadDivisionKeyPeople",
                column: "LeadDivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadDivisions_BranchId",
                table: "LeadDivisions",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadDivisions_CreatedById",
                table: "LeadDivisions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_LeadDivisions_LeadId",
                table: "LeadDivisions",
                column: "LeadId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadDivisions_LeadOriginId",
                table: "LeadDivisions",
                column: "LeadOriginId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadDivisions_OfficeId",
                table: "LeadDivisions",
                column: "OfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadDivisions_PrimaryContactId",
                table: "LeadDivisions",
                column: "PrimaryContactId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadDivisions_SecondaryContactId",
                table: "LeadDivisions",
                column: "SecondaryContactId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadKeyPeople_CreatedById",
                table: "LeadKeyPeople",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_LeadKeyPeople_LeadId",
                table: "LeadKeyPeople",
                column: "LeadId");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_CreatedById",
                table: "Leads",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_CustomerId",
                table: "Leads",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_DropReasonId",
                table: "Leads",
                column: "DropReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_GroupTypeId",
                table: "Leads",
                column: "GroupTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_LeadOriginId",
                table: "Leads",
                column: "LeadOriginId");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_LeadTypeId",
                table: "Leads",
                column: "LeadTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_PrimaryContactId",
                table: "Leads",
                column: "PrimaryContactId");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_SecondaryContactId",
                table: "Leads",
                column: "SecondaryContactId");

            migrationBuilder.CreateIndex(
                name: "IX_Quotes_CreatedById",
                table: "Quotes",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Quotes_LeadDivisionId",
                table: "Quotes",
                column: "LeadDivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuoteServiceDocuments_CreatedById",
                table: "QuoteServiceDocuments",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_QuoteServiceDocuments_QuoteServiceId",
                table: "QuoteServiceDocuments",
                column: "QuoteServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_QuoteServices_BranchId",
                table: "QuoteServices",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_QuoteServices_ContractId",
                table: "QuoteServices",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_QuoteServices_CreatedById",
                table: "QuoteServices",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_QuoteServices_OfficeId",
                table: "QuoteServices",
                column: "OfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_QuoteServices_QuoteId",
                table: "QuoteServices",
                column: "QuoteId");

            migrationBuilder.CreateIndex(
                name: "IX_QuoteServices_ServiceId",
                table: "QuoteServices",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_SBUToContractServiceProportions_ContractServiceId",
                table: "SBUToContractServiceProportions",
                column: "ContractServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_SBUToContractServiceProportions_CreatedById",
                table: "SBUToContractServiceProportions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SBUToContractServiceProportions_StrategicBusinessUnitId",
                table: "SBUToContractServiceProportions",
                column: "StrategicBusinessUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_SBUToQuoteServiceProportions_CreatedById",
                table: "SBUToQuoteServiceProportions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SBUToQuoteServiceProportions_QuoteServiceId",
                table: "SBUToQuoteServiceProportions",
                column: "QuoteServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_SBUToQuoteServiceProportions_StrategicBusinessUnitId",
                table: "SBUToQuoteServiceProportions",
                column: "StrategicBusinessUnitId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClosureDocuments");

            migrationBuilder.DropTable(
                name: "LeadDivisionKeyPeople");

            migrationBuilder.DropTable(
                name: "LeadKeyPeople");

            migrationBuilder.DropTable(
                name: "QuoteServiceDocuments");

            migrationBuilder.DropTable(
                name: "SBUToContractServiceProportions");

            migrationBuilder.DropTable(
                name: "SBUToQuoteServiceProportions");

            migrationBuilder.DropTable(
                name: "ContractService");

            migrationBuilder.DropTable(
                name: "QuoteServices");

            migrationBuilder.DropTable(
                name: "Contract");

            migrationBuilder.DropTable(
                name: "CustomerDivisions");

            migrationBuilder.DropTable(
                name: "Quotes");

            migrationBuilder.DropTable(
                name: "LeadDivisions");

            migrationBuilder.DropTable(
                name: "LeadDivisionContacts");

            migrationBuilder.DropTable(
                name: "Leads");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "DropReasons");

            migrationBuilder.DropTable(
                name: "LeadContacts");
        }
    }
}
