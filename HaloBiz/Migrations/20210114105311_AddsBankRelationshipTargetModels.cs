using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class AddsBankRelationshipTargetModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "StandardSLAForOperatingEntities",
                newName: "CreatedById");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "RequiredServiceFields",
                newName: "CreatedById");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "RequiredServiceDocuments",
                newName: "CreatedById");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "MeansOfIdentification",
                newName: "CreatedById");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "GroupType",
                newName: "CreatedById");

            migrationBuilder.CreateTable(
                name: "Bank",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Slogan = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Alias = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bank", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bank_UserProfiles_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Relationship",
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
                    table.PrimaryKey("PK_Relationship", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Relationship_UserProfiles_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Target",
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
                    table.PrimaryKey("PK_Target", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Target_UserProfiles_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StandardSLAForOperatingEntities_CreatedById",
                table: "StandardSLAForOperatingEntities",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_RequiredServiceFields_CreatedById",
                table: "RequiredServiceFields",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_RequiredServiceDocuments_CreatedById",
                table: "RequiredServiceDocuments",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_MeansOfIdentification_CreatedById",
                table: "MeansOfIdentification",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_GroupType_CreatedById",
                table: "GroupType",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Bank_CreatedById",
                table: "Bank",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Relationship_CreatedById",
                table: "Relationship",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Target_CreatedById",
                table: "Target",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupType_UserProfiles_CreatedById",
                table: "GroupType",
                column: "CreatedById",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MeansOfIdentification_UserProfiles_CreatedById",
                table: "MeansOfIdentification",
                column: "CreatedById",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RequiredServiceDocuments_UserProfiles_CreatedById",
                table: "RequiredServiceDocuments",
                column: "CreatedById",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RequiredServiceFields_UserProfiles_CreatedById",
                table: "RequiredServiceFields",
                column: "CreatedById",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StandardSLAForOperatingEntities_UserProfiles_CreatedById",
                table: "StandardSLAForOperatingEntities",
                column: "CreatedById",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupType_UserProfiles_CreatedById",
                table: "GroupType");

            migrationBuilder.DropForeignKey(
                name: "FK_MeansOfIdentification_UserProfiles_CreatedById",
                table: "MeansOfIdentification");

            migrationBuilder.DropForeignKey(
                name: "FK_RequiredServiceDocuments_UserProfiles_CreatedById",
                table: "RequiredServiceDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_RequiredServiceFields_UserProfiles_CreatedById",
                table: "RequiredServiceFields");

            migrationBuilder.DropForeignKey(
                name: "FK_StandardSLAForOperatingEntities_UserProfiles_CreatedById",
                table: "StandardSLAForOperatingEntities");

            migrationBuilder.DropTable(
                name: "Bank");

            migrationBuilder.DropTable(
                name: "Relationship");

            migrationBuilder.DropTable(
                name: "Target");

            migrationBuilder.DropIndex(
                name: "IX_StandardSLAForOperatingEntities_CreatedById",
                table: "StandardSLAForOperatingEntities");

            migrationBuilder.DropIndex(
                name: "IX_RequiredServiceFields_CreatedById",
                table: "RequiredServiceFields");

            migrationBuilder.DropIndex(
                name: "IX_RequiredServiceDocuments_CreatedById",
                table: "RequiredServiceDocuments");

            migrationBuilder.DropIndex(
                name: "IX_MeansOfIdentification_CreatedById",
                table: "MeansOfIdentification");

            migrationBuilder.DropIndex(
                name: "IX_GroupType_CreatedById",
                table: "GroupType");

            migrationBuilder.RenameColumn(
                name: "CreatedById",
                table: "StandardSLAForOperatingEntities",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "CreatedById",
                table: "RequiredServiceFields",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "CreatedById",
                table: "RequiredServiceDocuments",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "CreatedById",
                table: "MeansOfIdentification",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "CreatedById",
                table: "GroupType",
                newName: "CreatedBy");
        }
    }
}
