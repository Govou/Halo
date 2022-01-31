using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class AddsNegotiationDocumentmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UserInvolvedId",
                table: "SBUToQuoteServiceProportions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "UserInvolvedId",
                table: "SBUToContractServiceProportions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "NegotiationDocuments",
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
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NegotiationDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NegotiationDocuments_QuoteServices_QuoteServiceId",
                        column: x => x.QuoteServiceId,
                        principalTable: "QuoteServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NegotiationDocuments_UserProfiles_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SBUToQuoteServiceProportions_UserInvolvedId",
                table: "SBUToQuoteServiceProportions",
                column: "UserInvolvedId");

            migrationBuilder.CreateIndex(
                name: "IX_SBUToContractServiceProportions_UserInvolvedId",
                table: "SBUToContractServiceProportions",
                column: "UserInvolvedId");

            migrationBuilder.CreateIndex(
                name: "IX_NegotiationDocuments_CreatedById",
                table: "NegotiationDocuments",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_NegotiationDocuments_QuoteServiceId",
                table: "NegotiationDocuments",
                column: "QuoteServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_SBUToContractServiceProportions_UserProfiles_UserInvolvedId",
                table: "SBUToContractServiceProportions",
                column: "UserInvolvedId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SBUToQuoteServiceProportions_UserProfiles_UserInvolvedId",
                table: "SBUToQuoteServiceProportions",
                column: "UserInvolvedId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SBUToContractServiceProportions_UserProfiles_UserInvolvedId",
                table: "SBUToContractServiceProportions");

            migrationBuilder.DropForeignKey(
                name: "FK_SBUToQuoteServiceProportions_UserProfiles_UserInvolvedId",
                table: "SBUToQuoteServiceProportions");

            migrationBuilder.DropTable(
                name: "NegotiationDocuments");

            migrationBuilder.DropIndex(
                name: "IX_SBUToQuoteServiceProportions_UserInvolvedId",
                table: "SBUToQuoteServiceProportions");

            migrationBuilder.DropIndex(
                name: "IX_SBUToContractServiceProportions_UserInvolvedId",
                table: "SBUToContractServiceProportions");

            migrationBuilder.DropColumn(
                name: "UserInvolvedId",
                table: "SBUToQuoteServiceProportions");

            migrationBuilder.DropColumn(
                name: "UserInvolvedId",
                table: "SBUToContractServiceProportions");
        }
    }
}
