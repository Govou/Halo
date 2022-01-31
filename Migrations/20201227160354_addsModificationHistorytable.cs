using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class addsModificationHistorytable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ModificationHistories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModelChanged = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChangedById = table.Column<long>(type: "bigint", nullable: true),
                    ChangeSummary = table.Column<string>(type: "nvarchar(max)", maxLength: 20000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModificationHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModificationHistories_UserProfiles_ChangedById",
                        column: x => x.ChangedById,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ModificationHistories_ChangedById",
                table: "ModificationHistories",
                column: "ChangedById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ModificationHistories");
        }
    }
}
