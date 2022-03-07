using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class addedOtherLeadCaptureInfoModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OtherLeadCaptureInfos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CooperateEstimatedAnnualIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CooperateEstimatedAnnualProfit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CooperateBalanceSheetSize = table.Column<long>(type: "bigint", nullable: false),
                    CooperateStaffStrength = table.Column<long>(type: "bigint", nullable: false),
                    CooperateCashBookSize = table.Column<long>(type: "bigint", nullable: false),
                    IndividualEstimatedAnnualIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IndividualDisposableIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IndividualResidenceSize = table.Column<long>(type: "bigint", nullable: false),
                    GroupTypeId = table.Column<long>(type: "bigint", nullable: false),
                    LeadId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtherLeadCaptureInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OtherLeadCaptureInfos_GroupType_GroupTypeId",
                        column: x => x.GroupTypeId,
                        principalTable: "GroupType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_OtherLeadCaptureInfos_Leads_LeadId",
                        column: x => x.LeadId,
                        principalTable: "Leads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_OtherLeadCaptureInfos_UserProfiles_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OtherLeadCaptureInfos_CreatedById",
                table: "OtherLeadCaptureInfos",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_OtherLeadCaptureInfos_GroupTypeId",
                table: "OtherLeadCaptureInfos",
                column: "GroupTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OtherLeadCaptureInfos_LeadId",
                table: "OtherLeadCaptureInfos",
                column: "LeadId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OtherLeadCaptureInfos");
        }
    }
}
