using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class AddRequiredServiceQualificationModelAndUpdatesServiceType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceRequiredServiceField");

            migrationBuilder.DropTable(
                name: "RequiredServiceFields");

            migrationBuilder.AddColumn<long>(
                name: "ServiceTypeId",
                table: "Services",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ServiceRequiredServiceDocument",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ServiceRequiredServiceDocument",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "ServiceRequiredServiceDocument",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.CreateTable(
                name: "RequredServiceQualificationElements",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceCategoryId = table.Column<long>(type: "bigint", nullable: false),
                    ServiceCategoryTaskId = table.Column<long>(type: "bigint", nullable: true),
                    Caption = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequredServiceQualificationElements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequredServiceQualificationElements_ServiceCategories_ServiceCategoryId",
                        column: x => x.ServiceCategoryId,
                        principalTable: "ServiceCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RequredServiceQualificationElements_ServiceCategoryTasks_ServiceCategoryTaskId",
                        column: x => x.ServiceCategoryTaskId,
                        principalTable: "ServiceCategoryTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequredServiceQualificationElements_UserProfiles_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Services_ServiceTypeId",
                table: "Services",
                column: "ServiceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RequredServiceQualificationElements_CreatedById",
                table: "RequredServiceQualificationElements",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_RequredServiceQualificationElements_ServiceCategoryId",
                table: "RequredServiceQualificationElements",
                column: "ServiceCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_RequredServiceQualificationElements_ServiceCategoryTaskId",
                table: "RequredServiceQualificationElements",
                column: "ServiceCategoryTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_ServiceTypes_ServiceTypeId",
                table: "Services",
                column: "ServiceTypeId",
                principalTable: "ServiceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_ServiceTypes_ServiceTypeId",
                table: "Services");

            migrationBuilder.DropTable(
                name: "RequredServiceQualificationElements");

            migrationBuilder.DropIndex(
                name: "IX_Services_ServiceTypeId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "ServiceTypeId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ServiceRequiredServiceDocument");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ServiceRequiredServiceDocument");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "ServiceRequiredServiceDocument");

            migrationBuilder.CreateTable(
                name: "RequiredServiceFields",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Caption = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequiredServiceFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequiredServiceFields_UserProfiles_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceRequiredServiceField",
                columns: table => new
                {
                    RequiredServiceFieldId = table.Column<long>(type: "bigint", nullable: false),
                    ServicesId = table.Column<long>(type: "bigint", nullable: false),
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
                name: "IX_RequiredServiceFields_CreatedById",
                table: "RequiredServiceFields",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequiredServiceField_ServicesId",
                table: "ServiceRequiredServiceField",
                column: "ServicesId");
        }
    }
}
