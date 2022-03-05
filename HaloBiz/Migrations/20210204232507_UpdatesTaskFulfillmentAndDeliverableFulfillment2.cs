using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class UpdatesTaskFulfillmentAndDeliverableFulfillment2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskFulfillment_ContractServices_ContractServiceId",
                table: "TaskFulfillment");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskFulfillment_CustomerDivisions_CustomerDivisionId",
                table: "TaskFulfillment");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskFulfillment_UserProfiles_AccountableId",
                table: "TaskFulfillment");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskFulfillment_UserProfiles_ConsultedId",
                table: "TaskFulfillment");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskFulfillment_UserProfiles_CreatedById",
                table: "TaskFulfillment");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskFulfillment_UserProfiles_InformedId",
                table: "TaskFulfillment");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskFulfillment_UserProfiles_ResponsibleId",
                table: "TaskFulfillment");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskFulfillment_UserProfiles_SupportId",
                table: "TaskFulfillment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskFulfillment",
                table: "TaskFulfillment");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "Accounts");

            migrationBuilder.RenameTable(
                name: "TaskFulfillment",
                newName: "TaskFulfillments");

            migrationBuilder.RenameIndex(
                name: "IX_TaskFulfillment_SupportId",
                table: "TaskFulfillments",
                newName: "IX_TaskFulfillments_SupportId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskFulfillment_ResponsibleId",
                table: "TaskFulfillments",
                newName: "IX_TaskFulfillments_ResponsibleId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskFulfillment_InformedId",
                table: "TaskFulfillments",
                newName: "IX_TaskFulfillments_InformedId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskFulfillment_CustomerDivisionId",
                table: "TaskFulfillments",
                newName: "IX_TaskFulfillments_CustomerDivisionId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskFulfillment_CreatedById",
                table: "TaskFulfillments",
                newName: "IX_TaskFulfillments_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_TaskFulfillment_ContractServiceId",
                table: "TaskFulfillments",
                newName: "IX_TaskFulfillments_ContractServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskFulfillment_ConsultedId",
                table: "TaskFulfillments",
                newName: "IX_TaskFulfillments_ConsultedId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskFulfillment_AccountableId",
                table: "TaskFulfillments",
                newName: "IX_TaskFulfillments_AccountableId");

            migrationBuilder.AlterColumn<string>(
                name: "Alias",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "TaskFulfillments",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TaskCompletionDateTime",
                table: "TaskFulfillments",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateTimePicked",
                table: "TaskFulfillments",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "TaskFulfillments",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<double>(
                name: "Budget",
                table: "TaskFulfillments",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskFulfillments",
                table: "TaskFulfillments",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "DeliverableFulfillments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Caption = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaskFullfillmentId = table.Column<long>(type: "bigint", nullable: false),
                    ResponsibleId = table.Column<long>(type: "bigint", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsPicked = table.Column<bool>(type: "bit", nullable: false),
                    DateAndTimePicked = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TaskCompletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TaskCompletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WasReassigned = table.Column<bool>(type: "bit", nullable: false),
                    DateTimeReassigned = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsRequestedForValidation = table.Column<bool>(type: "bit", nullable: false),
                    DateTimeRequestedForValidation = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeliverableStatus = table.Column<bool>(type: "bit", nullable: false),
                    Budget = table.Column<double>(type: "float", nullable: true),
                    DeliverableCompletionReferenceNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeliverableCompletionReferenceUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateAndTimeOfProvidedEvidence = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeliverableCompletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeliverableCompletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliverableFulfillments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeliverableFulfillments_TaskFulfillments_TaskFullfillmentId",
                        column: x => x.TaskFullfillmentId,
                        principalTable: "TaskFulfillments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeliverableFulfillments_UserProfiles_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_DeliverableFulfillments_UserProfiles_ResponsibleId",
                        column: x => x.ResponsibleId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeliverableFulfillments_CreatedById",
                table: "DeliverableFulfillments",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_DeliverableFulfillments_ResponsibleId",
                table: "DeliverableFulfillments",
                column: "ResponsibleId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliverableFulfillments_TaskFullfillmentId",
                table: "DeliverableFulfillments",
                column: "TaskFullfillmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskFulfillments_ContractServices_ContractServiceId",
                table: "TaskFulfillments",
                column: "ContractServiceId",
                principalTable: "ContractServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskFulfillments_CustomerDivisions_CustomerDivisionId",
                table: "TaskFulfillments",
                column: "CustomerDivisionId",
                principalTable: "CustomerDivisions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskFulfillments_UserProfiles_AccountableId",
                table: "TaskFulfillments",
                column: "AccountableId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskFulfillments_UserProfiles_ConsultedId",
                table: "TaskFulfillments",
                column: "ConsultedId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskFulfillments_UserProfiles_CreatedById",
                table: "TaskFulfillments",
                column: "CreatedById",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskFulfillments_UserProfiles_InformedId",
                table: "TaskFulfillments",
                column: "InformedId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskFulfillments_UserProfiles_ResponsibleId",
                table: "TaskFulfillments",
                column: "ResponsibleId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskFulfillments_UserProfiles_SupportId",
                table: "TaskFulfillments",
                column: "SupportId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskFulfillments_ContractServices_ContractServiceId",
                table: "TaskFulfillments");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskFulfillments_CustomerDivisions_CustomerDivisionId",
                table: "TaskFulfillments");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskFulfillments_UserProfiles_AccountableId",
                table: "TaskFulfillments");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskFulfillments_UserProfiles_ConsultedId",
                table: "TaskFulfillments");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskFulfillments_UserProfiles_CreatedById",
                table: "TaskFulfillments");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskFulfillments_UserProfiles_InformedId",
                table: "TaskFulfillments");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskFulfillments_UserProfiles_ResponsibleId",
                table: "TaskFulfillments");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskFulfillments_UserProfiles_SupportId",
                table: "TaskFulfillments");

            migrationBuilder.DropTable(
                name: "DeliverableFulfillments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskFulfillments",
                table: "TaskFulfillments");

            migrationBuilder.RenameTable(
                name: "TaskFulfillments",
                newName: "TaskFulfillment");

            migrationBuilder.RenameIndex(
                name: "IX_TaskFulfillments_SupportId",
                table: "TaskFulfillment",
                newName: "IX_TaskFulfillment_SupportId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskFulfillments_ResponsibleId",
                table: "TaskFulfillment",
                newName: "IX_TaskFulfillment_ResponsibleId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskFulfillments_InformedId",
                table: "TaskFulfillment",
                newName: "IX_TaskFulfillment_InformedId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskFulfillments_CustomerDivisionId",
                table: "TaskFulfillment",
                newName: "IX_TaskFulfillment_CustomerDivisionId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskFulfillments_CreatedById",
                table: "TaskFulfillment",
                newName: "IX_TaskFulfillment_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_TaskFulfillments_ContractServiceId",
                table: "TaskFulfillment",
                newName: "IX_TaskFulfillment_ContractServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskFulfillments_ConsultedId",
                table: "TaskFulfillment",
                newName: "IX_TaskFulfillment_ConsultedId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskFulfillments_AccountableId",
                table: "TaskFulfillment",
                newName: "IX_TaskFulfillment_AccountableId");

            migrationBuilder.AlterColumn<long>(
                name: "Alias",
                table: "Accounts",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<long>(
                name: "Number",
                table: "Accounts",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "TaskFulfillment",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TaskCompletionDateTime",
                table: "TaskFulfillment",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateTimePicked",
                table: "TaskFulfillment",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "TaskFulfillment",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<double>(
                name: "Budget",
                table: "TaskFulfillment",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskFulfillment",
                table: "TaskFulfillment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskFulfillment_ContractServices_ContractServiceId",
                table: "TaskFulfillment",
                column: "ContractServiceId",
                principalTable: "ContractServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskFulfillment_CustomerDivisions_CustomerDivisionId",
                table: "TaskFulfillment",
                column: "CustomerDivisionId",
                principalTable: "CustomerDivisions",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskFulfillment_UserProfiles_AccountableId",
                table: "TaskFulfillment",
                column: "AccountableId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskFulfillment_UserProfiles_ConsultedId",
                table: "TaskFulfillment",
                column: "ConsultedId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskFulfillment_UserProfiles_CreatedById",
                table: "TaskFulfillment",
                column: "CreatedById",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskFulfillment_UserProfiles_InformedId",
                table: "TaskFulfillment",
                column: "InformedId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskFulfillment_UserProfiles_ResponsibleId",
                table: "TaskFulfillment",
                column: "ResponsibleId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskFulfillment_UserProfiles_SupportId",
                table: "TaskFulfillment",
                column: "SupportId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
