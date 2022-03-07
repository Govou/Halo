using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class AddsZoneRegionModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_AccountClasses_AccountClassId",
                table: "Accounts");

            migrationBuilder.RenameColumn(
                name: "AccountClassId",
                table: "Accounts",
                newName: "ControlAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Accounts_AccountClassId",
                table: "Accounts",
                newName: "IX_Accounts_ControlAccountId");

            migrationBuilder.AddColumn<long>(
                name: "AccountId",
                table: "Services",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ControlAccount",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Caption = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Alias = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AccountClassId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ControlAccount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ControlAccount_AccountClasses_AccountClassId",
                        column: x => x.AccountClassId,
                        principalTable: "AccountClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ControlAccount_UserProfiles_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Region",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BranchId = table.Column<long>(type: "bigint", maxLength: 500, nullable: false),
                    HeadId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Region", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Region_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Region_UserProfiles_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Region_UserProfiles_HeadId",
                        column: x => x.HeadId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Zone",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    HeadId = table.Column<long>(type: "bigint", nullable: false),
                    StateId = table.Column<long>(type: "bigint", nullable: false),
                    LGAId = table.Column<long>(type: "bigint", nullable: false),
                    RegionId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zone", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Zone_LGAs_LGAId",
                        column: x => x.LGAId,
                        principalTable: "LGAs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Zone_Region_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Region",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Zone_States_StateId",
                        column: x => x.StateId,
                        principalTable: "States",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Zone_UserProfiles_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Zone_UserProfiles_HeadId",
                        column: x => x.HeadId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Services_AccountId",
                table: "Services",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_ControlAccount_AccountClassId",
                table: "ControlAccount",
                column: "AccountClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ControlAccount_CreatedById",
                table: "ControlAccount",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Region_BranchId",
                table: "Region",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Region_CreatedById",
                table: "Region",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Region_HeadId",
                table: "Region",
                column: "HeadId");

            migrationBuilder.CreateIndex(
                name: "IX_Zone_CreatedById",
                table: "Zone",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Zone_HeadId",
                table: "Zone",
                column: "HeadId");

            migrationBuilder.CreateIndex(
                name: "IX_Zone_LGAId",
                table: "Zone",
                column: "LGAId");

            migrationBuilder.CreateIndex(
                name: "IX_Zone_RegionId",
                table: "Zone",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Zone_StateId",
                table: "Zone",
                column: "StateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_ControlAccount_ControlAccountId",
                table: "Accounts",
                column: "ControlAccountId",
                principalTable: "ControlAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Accounts_AccountId",
                table: "Services",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_ControlAccount_ControlAccountId",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_Accounts_AccountId",
                table: "Services");

            migrationBuilder.DropTable(
                name: "ControlAccount");

            migrationBuilder.DropTable(
                name: "Zone");

            migrationBuilder.DropTable(
                name: "Region");

            migrationBuilder.DropIndex(
                name: "IX_Services_AccountId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Services");

            migrationBuilder.RenameColumn(
                name: "ControlAccountId",
                table: "Accounts",
                newName: "AccountClassId");

            migrationBuilder.RenameIndex(
                name: "IX_Accounts_ControlAccountId",
                table: "Accounts",
                newName: "IX_Accounts_AccountClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_AccountClasses_AccountClassId",
                table: "Accounts",
                column: "AccountClassId",
                principalTable: "AccountClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
