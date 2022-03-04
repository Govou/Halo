using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class AllFieldUpdatesForDtrack : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Alias",
                table: "OperatingEntities",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LGAId",
                table: "LeadDivisions",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "StateId",
                table: "LeadDivisions",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Street",
                table: "LeadDivisions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Alias",
                table: "FinanceVoucherTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ModeOfTransports",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Caption = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModeOfTransports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModeOfTransports_UserProfiles_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LeadDivisions_LGAId",
                table: "LeadDivisions",
                column: "LGAId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadDivisions_StateId",
                table: "LeadDivisions",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_ModeOfTransports_CreatedById",
                table: "ModeOfTransports",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_LeadDivisions_LGAs_LGAId",
                table: "LeadDivisions",
                column: "LGAId",
                principalTable: "LGAs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LeadDivisions_States_StateId",
                table: "LeadDivisions",
                column: "StateId",
                principalTable: "States",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeadDivisions_LGAs_LGAId",
                table: "LeadDivisions");

            migrationBuilder.DropForeignKey(
                name: "FK_LeadDivisions_States_StateId",
                table: "LeadDivisions");

            migrationBuilder.DropTable(
                name: "ModeOfTransports");

            migrationBuilder.DropIndex(
                name: "IX_LeadDivisions_LGAId",
                table: "LeadDivisions");

            migrationBuilder.DropIndex(
                name: "IX_LeadDivisions_StateId",
                table: "LeadDivisions");

            migrationBuilder.DropColumn(
                name: "Alias",
                table: "OperatingEntities");

            migrationBuilder.DropColumn(
                name: "LGAId",
                table: "LeadDivisions");

            migrationBuilder.DropColumn(
                name: "StateId",
                table: "LeadDivisions");

            migrationBuilder.DropColumn(
                name: "Street",
                table: "LeadDivisions");

            migrationBuilder.DropColumn(
                name: "Alias",
                table: "FinanceVoucherTypes");
        }
    }
}
