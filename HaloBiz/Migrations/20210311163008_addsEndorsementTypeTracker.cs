using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class addsEndorsementTypeTracker : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EndorsementTypeTrackerS",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PreviousContractServiceId = table.Column<long>(type: "bigint", nullable: false),
                    NewContractServiceId = table.Column<long>(type: "bigint", nullable: false),
                    DescriptionOfChange = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApprovedById = table.Column<long>(type: "bigint", nullable: false),
                    EndorsementTypeId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EndorsementTypeTrackerS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EndorsementTypeTrackerS_EndorsementTypes_EndorsementTypeId",
                        column: x => x.EndorsementTypeId,
                        principalTable: "EndorsementTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_EndorsementTypeTrackerS_UserProfiles_ApprovedById",
                        column: x => x.ApprovedById,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EndorsementTypeTrackerS_ApprovedById",
                table: "EndorsementTypeTrackerS",
                column: "ApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_EndorsementTypeTrackerS_EndorsementTypeId",
                table: "EndorsementTypeTrackerS",
                column: "EndorsementTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EndorsementTypeTrackerS");
        }
    }
}
