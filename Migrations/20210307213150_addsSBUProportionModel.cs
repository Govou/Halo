using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class addsSBUProportionModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SBUProportions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OperatingEntityId = table.Column<long>(type: "bigint", nullable: false),
                    LeadClosureProportion = table.Column<int>(type: "int", nullable: false),
                    LeadGenerationProportion = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SBUProportions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SBUProportions_OperatingEntities_OperatingEntityId",
                        column: x => x.OperatingEntityId,
                        principalTable: "OperatingEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_SBUProportions_UserProfiles_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SBUProportions_CreatedById",
                table: "SBUProportions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SBUProportions_OperatingEntityId",
                table: "SBUProportions",
                column: "OperatingEntityId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SBUProportions");
        }
    }
}
