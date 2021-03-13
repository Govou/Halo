using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class addsContractServiceForEndorsement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EndorsementTypeTrackerS_EndorsementTypes_EndorsementTypeId",
                table: "EndorsementTypeTrackerS");

            migrationBuilder.DropForeignKey(
                name: "FK_EndorsementTypeTrackerS_UserProfiles_ApprovedById",
                table: "EndorsementTypeTrackerS");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EndorsementTypeTrackerS",
                table: "EndorsementTypeTrackerS");

            migrationBuilder.RenameTable(
                name: "EndorsementTypeTrackerS",
                newName: "EndorsementTypeTrackers");

            migrationBuilder.RenameIndex(
                name: "IX_EndorsementTypeTrackerS_EndorsementTypeId",
                table: "EndorsementTypeTrackers",
                newName: "IX_EndorsementTypeTrackers_EndorsementTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_EndorsementTypeTrackerS_ApprovedById",
                table: "EndorsementTypeTrackers",
                newName: "IX_EndorsementTypeTrackers_ApprovedById");

            migrationBuilder.AddColumn<long>(
                name: "ContractServiceForEndorsementId",
                table: "TaskFulfillments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_EndorsementTypeTrackers",
                table: "EndorsementTypeTrackers",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ContractServiceForEndorsements",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EndorsementTypeId = table.Column<long>(type: "bigint", nullable: false),
                    IsRequestedForApproval = table.Column<bool>(type: "bit", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    IsDeclined = table.Column<bool>(type: "bit", nullable: false),
                    ReferenceNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UnitPrice = table.Column<double>(type: "float", nullable: true),
                    Quantity = table.Column<long>(type: "bigint", nullable: false),
                    Discount = table.Column<double>(type: "float", nullable: false),
                    VAT = table.Column<double>(type: "float", nullable: true),
                    BillableAmount = table.Column<double>(type: "float", nullable: true),
                    Budget = table.Column<double>(type: "float", nullable: true),
                    ContractStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ContractEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentCycle = table.Column<int>(type: "int", nullable: true),
                    PaymentCycleInDays = table.Column<long>(type: "bigint", nullable: true),
                    InvoiceCycleInDays = table.Column<long>(type: "bigint", nullable: true),
                    FirstInvoiceSendDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InvoicingInterval = table.Column<int>(type: "int", nullable: true),
                    ProblemStatement = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ActivationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FulfillmentStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FulfillmentEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TentativeDateForSiteSurvey = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PickupDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DropoffDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PickupLocation = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Dropofflocation = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    BeneficiaryName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BeneficiaryIdentificationType = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BenificiaryIdentificationNumber = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TentativeProofOfConceptStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TentativeProofOfConceptEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TentativeFulfillmentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProgramCommencementDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProgramDuration = table.Column<long>(type: "bigint", nullable: true),
                    ProgramEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TentativeDateOfSiteVisit = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ServiceId = table.Column<long>(type: "bigint", nullable: false),
                    ContractId = table.Column<long>(type: "bigint", nullable: false),
                    GroupInvoiceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractServiceForEndorsements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContractServiceForEndorsements_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ContractServiceForEndorsements_EndorsementTypes_EndorsementTypeId",
                        column: x => x.EndorsementTypeId,
                        principalTable: "EndorsementTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ContractServiceForEndorsements_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ContractServiceForEndorsements_UserProfiles_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskFulfillments_ContractServiceForEndorsementId",
                table: "TaskFulfillments",
                column: "ContractServiceForEndorsementId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractServiceForEndorsements_ContractId",
                table: "ContractServiceForEndorsements",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractServiceForEndorsements_CreatedById",
                table: "ContractServiceForEndorsements",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ContractServiceForEndorsements_EndorsementTypeId",
                table: "ContractServiceForEndorsements",
                column: "EndorsementTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractServiceForEndorsements_ServiceId",
                table: "ContractServiceForEndorsements",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_EndorsementTypeTrackers_EndorsementTypes_EndorsementTypeId",
                table: "EndorsementTypeTrackers",
                column: "EndorsementTypeId",
                principalTable: "EndorsementTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_EndorsementTypeTrackers_UserProfiles_ApprovedById",
                table: "EndorsementTypeTrackers",
                column: "ApprovedById",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskFulfillments_ContractServiceForEndorsements_ContractServiceForEndorsementId",
                table: "TaskFulfillments",
                column: "ContractServiceForEndorsementId",
                principalTable: "ContractServiceForEndorsements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EndorsementTypeTrackers_EndorsementTypes_EndorsementTypeId",
                table: "EndorsementTypeTrackers");

            migrationBuilder.DropForeignKey(
                name: "FK_EndorsementTypeTrackers_UserProfiles_ApprovedById",
                table: "EndorsementTypeTrackers");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskFulfillments_ContractServiceForEndorsements_ContractServiceForEndorsementId",
                table: "TaskFulfillments");

            migrationBuilder.DropTable(
                name: "ContractServiceForEndorsements");

            migrationBuilder.DropIndex(
                name: "IX_TaskFulfillments_ContractServiceForEndorsementId",
                table: "TaskFulfillments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EndorsementTypeTrackers",
                table: "EndorsementTypeTrackers");

            migrationBuilder.DropColumn(
                name: "ContractServiceForEndorsementId",
                table: "TaskFulfillments");

            migrationBuilder.RenameTable(
                name: "EndorsementTypeTrackers",
                newName: "EndorsementTypeTrackerS");

            migrationBuilder.RenameIndex(
                name: "IX_EndorsementTypeTrackers_EndorsementTypeId",
                table: "EndorsementTypeTrackerS",
                newName: "IX_EndorsementTypeTrackerS_EndorsementTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_EndorsementTypeTrackers_ApprovedById",
                table: "EndorsementTypeTrackerS",
                newName: "IX_EndorsementTypeTrackerS_ApprovedById");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EndorsementTypeTrackerS",
                table: "EndorsementTypeTrackerS",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EndorsementTypeTrackerS_EndorsementTypes_EndorsementTypeId",
                table: "EndorsementTypeTrackerS",
                column: "EndorsementTypeId",
                principalTable: "EndorsementTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EndorsementTypeTrackerS_UserProfiles_ApprovedById",
                table: "EndorsementTypeTrackerS",
                column: "ApprovedById",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
