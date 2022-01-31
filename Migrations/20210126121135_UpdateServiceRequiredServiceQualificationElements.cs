using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class UpdateServiceRequiredServiceQualificationElements : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequredServiceQualificationElements_ServiceCategories_ServiceCategoryId",
                table: "RequredServiceQualificationElements");

            migrationBuilder.DropIndex(
                name: "IX_RequredServiceQualificationElements_ServiceCategoryId",
                table: "RequredServiceQualificationElements");

            migrationBuilder.DropColumn(
                name: "ServiceCategoryId",
                table: "RequredServiceQualificationElements");

            migrationBuilder.CreateTable(
                name: "ServiceRequredServiceQualificationElement",
                columns: table => new
                {
                    ServicesId = table.Column<long>(type: "bigint", nullable: false),
                    RequredServiceQualificationElementId = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceRequredServiceQualificationElement", x => new { x.RequredServiceQualificationElementId, x.ServicesId });
                    table.ForeignKey(
                        name: "FK_ServiceRequredServiceQualificationElement_RequredServiceQualificationElements_RequredServiceQualificationElementId",
                        column: x => x.RequredServiceQualificationElementId,
                        principalTable: "RequredServiceQualificationElements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceRequredServiceQualificationElement_Services_ServicesId",
                        column: x => x.ServicesId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequredServiceQualificationElement_ServicesId",
                table: "ServiceRequredServiceQualificationElement",
                column: "ServicesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceRequredServiceQualificationElement");

            migrationBuilder.AddColumn<long>(
                name: "ServiceCategoryId",
                table: "RequredServiceQualificationElements",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_RequredServiceQualificationElements_ServiceCategoryId",
                table: "RequredServiceQualificationElements",
                column: "ServiceCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequredServiceQualificationElements_ServiceCategories_ServiceCategoryId",
                table: "RequredServiceQualificationElements",
                column: "ServiceCategoryId",
                principalTable: "ServiceCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
