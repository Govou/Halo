using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class variousLeadChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClientContactQualifications",
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
                    table.PrimaryKey("PK_ClientContactQualifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientContactQualifications_UserProfiles_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "EngagementTypes",
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
                    table.PrimaryKey("PK_EngagementTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EngagementTypes_UserProfiles_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ClientEngagements",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Caption = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EngagementDiscussion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EngagementTypeId = table.Column<long>(type: "bigint", nullable: false),
                    LeadKeyContactId = table.Column<long>(type: "bigint", nullable: true),
                    LeadKeyPersonId = table.Column<long>(type: "bigint", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CustomerDivisionId = table.Column<long>(type: "bigint", nullable: false),
                    EngagementOutcome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContractServicesDiscussed = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientEngagements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientEngagements_CustomerDivisions_CustomerDivisionId",
                        column: x => x.CustomerDivisionId,
                        principalTable: "CustomerDivisions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ClientEngagements_EngagementTypes_EngagementTypeId",
                        column: x => x.EngagementTypeId,
                        principalTable: "EngagementTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ClientEngagements_LeadContacts_LeadKeyContactId",
                        column: x => x.LeadKeyContactId,
                        principalTable: "LeadContacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ClientEngagements_LeadKeyPeople_LeadKeyPersonId",
                        column: x => x.LeadKeyPersonId,
                        principalTable: "LeadKeyPeople",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ClientEngagements_UserProfiles_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "LeadEngagements",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Caption = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EngagementDiscussion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EngagementTypeId = table.Column<long>(type: "bigint", nullable: false),
                    LeadKeyContactId = table.Column<long>(type: "bigint", nullable: true),
                    LeadKeyPersonId = table.Column<long>(type: "bigint", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LeadId = table.Column<long>(type: "bigint", nullable: false),
                    LeadCaptureStage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EngagementOutcome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeadEngagements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeadEngagements_EngagementTypes_EngagementTypeId",
                        column: x => x.EngagementTypeId,
                        principalTable: "EngagementTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_LeadEngagements_LeadContacts_LeadKeyContactId",
                        column: x => x.LeadKeyContactId,
                        principalTable: "LeadContacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_LeadEngagements_LeadKeyPeople_LeadKeyPersonId",
                        column: x => x.LeadKeyPersonId,
                        principalTable: "LeadKeyPeople",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_LeadEngagements_Leads_LeadId",
                        column: x => x.LeadId,
                        principalTable: "Leads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_LeadEngagements_UserProfiles_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientContactQualifications_CreatedById",
                table: "ClientContactQualifications",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ClientEngagements_CreatedById",
                table: "ClientEngagements",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ClientEngagements_CustomerDivisionId",
                table: "ClientEngagements",
                column: "CustomerDivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientEngagements_EngagementTypeId",
                table: "ClientEngagements",
                column: "EngagementTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientEngagements_LeadKeyContactId",
                table: "ClientEngagements",
                column: "LeadKeyContactId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientEngagements_LeadKeyPersonId",
                table: "ClientEngagements",
                column: "LeadKeyPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_EngagementTypes_CreatedById",
                table: "EngagementTypes",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_LeadEngagements_CreatedById",
                table: "LeadEngagements",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_LeadEngagements_EngagementTypeId",
                table: "LeadEngagements",
                column: "EngagementTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadEngagements_LeadId",
                table: "LeadEngagements",
                column: "LeadId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadEngagements_LeadKeyContactId",
                table: "LeadEngagements",
                column: "LeadKeyContactId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadEngagements_LeadKeyPersonId",
                table: "LeadEngagements",
                column: "LeadKeyPersonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientContactQualifications");

            migrationBuilder.DropTable(
                name: "ClientEngagements");

            migrationBuilder.DropTable(
                name: "LeadEngagements");

            migrationBuilder.DropTable(
                name: "EngagementTypes");
        }
    }
}
