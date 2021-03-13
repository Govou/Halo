using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class AddsNotificationAndTaskEscalationModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "TaskFulfillments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ServiceCode",
                table: "TaskFulfillments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "TaskFulfillments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "EscallationTimeDurationForPicking",
                table: "DeliverableFulfillments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "ServiceCode",
                table: "DeliverableFulfillments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CustomerDivisionId",
                table: "AccountMasters",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsMailSent = table.Column<bool>(type: "bit", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SenderId = table.Column<long>(type: "bigint", nullable: false),
                    Recepients = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bcc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_UserProfiles_SenderId",
                        column: x => x.SenderId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskEscalationTiming",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Duration = table.Column<long>(type: "bigint", nullable: false),
                    Module = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Stage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceCategoryId = table.Column<long>(type: "bigint", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskEscalationTiming", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskEscalationTiming_ServiceCategories_ServiceCategoryId",
                        column: x => x.ServiceCategoryId,
                        principalTable: "ServiceCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountMasters_CustomerDivisionId",
                table: "AccountMasters",
                column: "CustomerDivisionId",
                unique: true,
                filter: "[CustomerDivisionId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_SenderId",
                table: "Notifications",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskEscalationTiming_ServiceCategoryId",
                table: "TaskEscalationTiming",
                column: "ServiceCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountMasters_CustomerDivisions_CustomerDivisionId",
                table: "AccountMasters",
                column: "CustomerDivisionId",
                principalTable: "CustomerDivisions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountMasters_CustomerDivisions_CustomerDivisionId",
                table: "AccountMasters");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "TaskEscalationTiming");

            migrationBuilder.DropIndex(
                name: "IX_AccountMasters_CustomerDivisionId",
                table: "AccountMasters");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "TaskFulfillments");

            migrationBuilder.DropColumn(
                name: "ServiceCode",
                table: "TaskFulfillments");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "TaskFulfillments");

            migrationBuilder.DropColumn(
                name: "EscallationTimeDurationForPicking",
                table: "DeliverableFulfillments");

            migrationBuilder.DropColumn(
                name: "ServiceCode",
                table: "DeliverableFulfillments");

            migrationBuilder.DropColumn(
                name: "CustomerDivisionId",
                table: "AccountMasters");
        }
    }
}
