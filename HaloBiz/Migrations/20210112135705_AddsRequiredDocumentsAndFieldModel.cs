using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class AddsRequiredDocumentsAndFieldModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SBUAccountMasters_AccountMasters_AccountMasterId",
                table: "SBUAccountMasters");

            migrationBuilder.DropForeignKey(
                name: "FK_SBUAccountMasters_StrategicBusinessUnits_StrategicBusinessUnitId",
                table: "SBUAccountMasters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SBUAccountMasters",
                table: "SBUAccountMasters");

            migrationBuilder.RenameTable(
                name: "SBUAccountMasters",
                newName: "SBUAccountMaster");

            migrationBuilder.RenameIndex(
                name: "IX_SBUAccountMasters_StrategicBusinessUnitId",
                table: "SBUAccountMaster",
                newName: "IX_SBUAccountMaster_StrategicBusinessUnitId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SBUAccountMaster",
                table: "SBUAccountMaster",
                columns: new[] { "AccountMasterId", "StrategicBusinessUnitId" });

            migrationBuilder.CreateTable(
                name: "RequiredServiceDocuments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Caption = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequiredServiceDocuments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RequiredServiceFields",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Caption = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequiredServiceFields", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceRequiredServiceDocument",
                columns: table => new
                {
                    ServicesId = table.Column<long>(type: "bigint", nullable: false),
                    RequiredServiceDocumentId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceRequiredServiceDocument", x => new { x.RequiredServiceDocumentId, x.ServicesId });
                    table.ForeignKey(
                        name: "FK_ServiceRequiredServiceDocument_RequiredServiceDocuments_RequiredServiceDocumentId",
                        column: x => x.RequiredServiceDocumentId,
                        principalTable: "RequiredServiceDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceRequiredServiceDocument_Services_ServicesId",
                        column: x => x.ServicesId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceRequiredServiceField",
                columns: table => new
                {
                    ServicesId = table.Column<long>(type: "bigint", nullable: false),
                    RequiredServiceFieldId = table.Column<long>(type: "bigint", nullable: false),
                    RequiredServiceField = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceRequiredServiceField", x => new { x.RequiredServiceFieldId, x.ServicesId });
                    table.ForeignKey(
                        name: "FK_ServiceRequiredServiceField_RequiredServiceFields_RequiredServiceFieldId",
                        column: x => x.RequiredServiceFieldId,
                        principalTable: "RequiredServiceFields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceRequiredServiceField_Services_ServicesId",
                        column: x => x.ServicesId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequiredServiceDocument_ServicesId",
                table: "ServiceRequiredServiceDocument",
                column: "ServicesId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequiredServiceField_ServicesId",
                table: "ServiceRequiredServiceField",
                column: "ServicesId");

            migrationBuilder.AddForeignKey(
                name: "FK_SBUAccountMaster_AccountMasters_AccountMasterId",
                table: "SBUAccountMaster",
                column: "AccountMasterId",
                principalTable: "AccountMasters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SBUAccountMaster_StrategicBusinessUnits_StrategicBusinessUnitId",
                table: "SBUAccountMaster",
                column: "StrategicBusinessUnitId",
                principalTable: "StrategicBusinessUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SBUAccountMaster_AccountMasters_AccountMasterId",
                table: "SBUAccountMaster");

            migrationBuilder.DropForeignKey(
                name: "FK_SBUAccountMaster_StrategicBusinessUnits_StrategicBusinessUnitId",
                table: "SBUAccountMaster");

            migrationBuilder.DropTable(
                name: "ServiceRequiredServiceDocument");

            migrationBuilder.DropTable(
                name: "ServiceRequiredServiceField");

            migrationBuilder.DropTable(
                name: "RequiredServiceDocuments");

            migrationBuilder.DropTable(
                name: "RequiredServiceFields");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SBUAccountMaster",
                table: "SBUAccountMaster");

            migrationBuilder.RenameTable(
                name: "SBUAccountMaster",
                newName: "SBUAccountMasters");

            migrationBuilder.RenameIndex(
                name: "IX_SBUAccountMaster_StrategicBusinessUnitId",
                table: "SBUAccountMasters",
                newName: "IX_SBUAccountMasters_StrategicBusinessUnitId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SBUAccountMasters",
                table: "SBUAccountMasters",
                columns: new[] { "AccountMasterId", "StrategicBusinessUnitId" });

            migrationBuilder.AddForeignKey(
                name: "FK_SBUAccountMasters_AccountMasters_AccountMasterId",
                table: "SBUAccountMasters",
                column: "AccountMasterId",
                principalTable: "AccountMasters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SBUAccountMasters_StrategicBusinessUnits_StrategicBusinessUnitId",
                table: "SBUAccountMasters",
                column: "StrategicBusinessUnitId",
                principalTable: "StrategicBusinessUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
