using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class UpdatesTaskFulfillmentAndDeliverableFulfillment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountMasters_Accounts_ChartofAccountSubId",
                table: "AccountMasters");

            migrationBuilder.DropColumn(
                name: "AccountClassAlias",
                table: "AccountDetails");

            migrationBuilder.RenameColumn(
                name: "ChartofAccountSubId",
                table: "AccountMasters",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_AccountMasters_ChartofAccountSubId",
                table: "AccountMasters",
                newName: "IX_AccountMasters_AccountId");

            migrationBuilder.AddColumn<long>(
                name: "LeadTypeId",
                table: "LeadDivisions",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TaskFulfillment",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Caption = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerDivisionId = table.Column<long>(type: "bigint", nullable: false),
                    ContractServiceId = table.Column<long>(type: "bigint", nullable: false),
                    ResponsibleId = table.Column<long>(type: "bigint", nullable: true),
                    AccountableId = table.Column<long>(type: "bigint", nullable: true),
                    ConsultedId = table.Column<long>(type: "bigint", nullable: true),
                    InformedId = table.Column<long>(type: "bigint", nullable: true),
                    SupportId = table.Column<long>(type: "bigint", nullable: true),
                    Budget = table.Column<double>(type: "float", nullable: false),
                    IsPicked = table.Column<bool>(type: "bit", nullable: false),
                    DateTimePicked = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsAllDeliverableAssigned = table.Column<bool>(type: "bit", nullable: false),
                    TaskCompletionDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TaskCompletionStatus = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskFulfillment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskFulfillment_ContractServices_ContractServiceId",
                        column: x => x.ContractServiceId,
                        principalTable: "ContractServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_TaskFulfillment_CustomerDivisions_CustomerDivisionId",
                        column: x => x.CustomerDivisionId,
                        principalTable: "CustomerDivisions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskFulfillment_UserProfiles_AccountableId",
                        column: x => x.AccountableId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_TaskFulfillment_UserProfiles_ConsultedId",
                        column: x => x.ConsultedId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaskFulfillment_UserProfiles_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_TaskFulfillment_UserProfiles_InformedId",
                        column: x => x.InformedId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaskFulfillment_UserProfiles_ResponsibleId",
                        column: x => x.ResponsibleId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaskFulfillment_UserProfiles_SupportId",
                        column: x => x.SupportId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LeadDivisions_LeadTypeId",
                table: "LeadDivisions",
                column: "LeadTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountMasters_VoucherId",
                table: "AccountMasters",
                column: "VoucherId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskFulfillment_AccountableId",
                table: "TaskFulfillment",
                column: "AccountableId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskFulfillment_ConsultedId",
                table: "TaskFulfillment",
                column: "ConsultedId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskFulfillment_ContractServiceId",
                table: "TaskFulfillment",
                column: "ContractServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskFulfillment_CreatedById",
                table: "TaskFulfillment",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_TaskFulfillment_CustomerDivisionId",
                table: "TaskFulfillment",
                column: "CustomerDivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskFulfillment_InformedId",
                table: "TaskFulfillment",
                column: "InformedId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskFulfillment_ResponsibleId",
                table: "TaskFulfillment",
                column: "ResponsibleId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskFulfillment_SupportId",
                table: "TaskFulfillment",
                column: "SupportId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountMasters_Accounts_AccountId",
                table: "AccountMasters",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountMasters_FinanceVoucherTypes_VoucherId",
                table: "AccountMasters",
                column: "VoucherId",
                principalTable: "FinanceVoucherTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LeadDivisions_LeadTypes_LeadTypeId",
                table: "LeadDivisions",
                column: "LeadTypeId",
                principalTable: "LeadTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountMasters_Accounts_AccountId",
                table: "AccountMasters");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountMasters_FinanceVoucherTypes_VoucherId",
                table: "AccountMasters");

            migrationBuilder.DropForeignKey(
                name: "FK_LeadDivisions_LeadTypes_LeadTypeId",
                table: "LeadDivisions");

            migrationBuilder.DropTable(
                name: "TaskFulfillment");

            migrationBuilder.DropIndex(
                name: "IX_LeadDivisions_LeadTypeId",
                table: "LeadDivisions");

            migrationBuilder.DropIndex(
                name: "IX_AccountMasters_VoucherId",
                table: "AccountMasters");

            migrationBuilder.DropColumn(
                name: "LeadTypeId",
                table: "LeadDivisions");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "AccountMasters",
                newName: "ChartofAccountSubId");

            migrationBuilder.RenameIndex(
                name: "IX_AccountMasters_AccountId",
                table: "AccountMasters",
                newName: "IX_AccountMasters_ChartofAccountSubId");

            migrationBuilder.AddColumn<long>(
                name: "AccountClassAlias",
                table: "AccountDetails",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountMasters_Accounts_ChartofAccountSubId",
                table: "AccountMasters",
                column: "ChartofAccountSubId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
