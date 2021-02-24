using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class UpdatesDeliverableFulfillmentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApproverLevel",
                table: "ApprovalLimits");

            migrationBuilder.DropColumn(
                name: "ModuleCaptured",
                table: "ApprovalLimits");

            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "DeliverableFulfillments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedById",
                table: "ApproverLevels",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<long>(
                name: "Sequence",
                table: "ApprovalLimits",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<long>(
                name: "ApproverLevelId",
                table: "ApprovalLimits",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "CreatedById",
                table: "ApprovalLimits",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ProcessesRequiringApprovalId",
                table: "ApprovalLimits",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "ProcessesRequiringApprovals",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Caption = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessesRequiringApprovals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcessesRequiringApprovals_UserProfiles_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApproverLevels_CreatedById",
                table: "ApproverLevels",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalLimits_ApproverLevelId",
                table: "ApprovalLimits",
                column: "ApproverLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalLimits_CreatedById",
                table: "ApprovalLimits",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalLimits_ProcessesRequiringApprovalId",
                table: "ApprovalLimits",
                column: "ProcessesRequiringApprovalId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessesRequiringApprovals_CreatedById",
                table: "ProcessesRequiringApprovals",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovalLimits_ApproverLevels_ApproverLevelId",
                table: "ApprovalLimits",
                column: "ApproverLevelId",
                principalTable: "ApproverLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovalLimits_ProcessesRequiringApprovals_ProcessesRequiringApprovalId",
                table: "ApprovalLimits",
                column: "ProcessesRequiringApprovalId",
                principalTable: "ProcessesRequiringApprovals",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovalLimits_UserProfiles_CreatedById",
                table: "ApprovalLimits",
                column: "CreatedById",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_ApproverLevels_UserProfiles_CreatedById",
                table: "ApproverLevels",
                column: "CreatedById",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApprovalLimits_ApproverLevels_ApproverLevelId",
                table: "ApprovalLimits");

            migrationBuilder.DropForeignKey(
                name: "FK_ApprovalLimits_ProcessesRequiringApprovals_ProcessesRequiringApprovalId",
                table: "ApprovalLimits");

            migrationBuilder.DropForeignKey(
                name: "FK_ApprovalLimits_UserProfiles_CreatedById",
                table: "ApprovalLimits");

            migrationBuilder.DropForeignKey(
                name: "FK_ApproverLevels_UserProfiles_CreatedById",
                table: "ApproverLevels");

            migrationBuilder.DropTable(
                name: "ProcessesRequiringApprovals");

            migrationBuilder.DropIndex(
                name: "IX_ApproverLevels_CreatedById",
                table: "ApproverLevels");

            migrationBuilder.DropIndex(
                name: "IX_ApprovalLimits_ApproverLevelId",
                table: "ApprovalLimits");

            migrationBuilder.DropIndex(
                name: "IX_ApprovalLimits_CreatedById",
                table: "ApprovalLimits");

            migrationBuilder.DropIndex(
                name: "IX_ApprovalLimits_ProcessesRequiringApprovalId",
                table: "ApprovalLimits");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "DeliverableFulfillments");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "ApproverLevels");

            migrationBuilder.DropColumn(
                name: "ApproverLevelId",
                table: "ApprovalLimits");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "ApprovalLimits");

            migrationBuilder.DropColumn(
                name: "ProcessesRequiringApprovalId",
                table: "ApprovalLimits");

            migrationBuilder.AlterColumn<string>(
                name: "Sequence",
                table: "ApprovalLimits",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<string>(
                name: "ApproverLevel",
                table: "ApprovalLimits",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ModuleCaptured",
                table: "ApprovalLimits",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
