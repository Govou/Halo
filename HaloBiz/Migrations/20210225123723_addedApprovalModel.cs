using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class addedApprovalModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Alias",
                table: "ApproverLevels",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Approvals",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Caption = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DateTimeApproved = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    ResponsibleId = table.Column<long>(type: "bigint", nullable: false),
                    Sequence = table.Column<long>(type: "bigint", nullable: false),
                    QuoteServiceId = table.Column<long>(type: "bigint", nullable: false),
                    QuoteId = table.Column<long>(type: "bigint", nullable: false),
                    ContractId = table.Column<long>(type: "bigint", nullable: false),
                    ContractServiceId = table.Column<long>(type: "bigint", nullable: false),
                    ServicesId = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Approvals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Approvals_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Approvals_ContractServices_ContractServiceId",
                        column: x => x.ContractServiceId,
                        principalTable: "ContractServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Approvals_Quotes_QuoteId",
                        column: x => x.QuoteId,
                        principalTable: "Quotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Approvals_QuoteServices_QuoteServiceId",
                        column: x => x.QuoteServiceId,
                        principalTable: "QuoteServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Approvals_Services_ServicesId",
                        column: x => x.ServicesId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Approvals_UserProfiles_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Approvals_UserProfiles_ResponsibleId",
                        column: x => x.ResponsibleId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Approvals_ContractId",
                table: "Approvals",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_Approvals_ContractServiceId",
                table: "Approvals",
                column: "ContractServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Approvals_CreatedById",
                table: "Approvals",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Approvals_QuoteId",
                table: "Approvals",
                column: "QuoteId");

            migrationBuilder.CreateIndex(
                name: "IX_Approvals_QuoteServiceId",
                table: "Approvals",
                column: "QuoteServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Approvals_ResponsibleId",
                table: "Approvals",
                column: "ResponsibleId");

            migrationBuilder.CreateIndex(
                name: "IX_Approvals_ServicesId",
                table: "Approvals",
                column: "ServicesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Approvals");

            migrationBuilder.DropColumn(
                name: "Alias",
                table: "ApproverLevels");
        }
    }
}
