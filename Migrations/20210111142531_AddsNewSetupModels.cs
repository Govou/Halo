using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class AddsNewSetupModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "SBUId",
                table: "UserProfiles",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AccountClasses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1000000000, 1000000000"),
                    Caption = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    AccountClassAlias = table.Column<long>(type: "bigint", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountClasses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountClasses_UserProfiles_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "FinanceVoucherTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "50, 2"),
                    VoucherType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinanceVoucherTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinanceVoucherTypes_UserProfiles_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "LeadTypes",
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
                    table.PrimaryKey("PK_LeadTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeadTypes_UserProfiles_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ServiceCategoryTasks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Caption = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ServiceCategoryId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceCategoryTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceCategoryTasks_ServiceCategories_ServiceCategoryId",
                        column: x => x.ServiceCategoryId,
                        principalTable: "ServiceCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceCategoryTasks_UserProfiles_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Alias = table.Column<long>(type: "bigint", nullable: false),
                    IsDebitBalance = table.Column<bool>(type: "bit", nullable: false),
                    AccountClassId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_AccountClasses_AccountClassId",
                        column: x => x.AccountClassId,
                        principalTable: "AccountClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Accounts_UserProfiles_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "LeadOrigins",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Caption = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    LeadTypeId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeadOrigins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeadOrigins_LeadTypes_LeadTypeId",
                        column: x => x.LeadTypeId,
                        principalTable: "LeadTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeadOrigins_UserProfiles_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "AccountMasters",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    AccountMasterAlias = table.Column<long>(type: "bigint", nullable: false),
                    IntegrationFlag = table.Column<bool>(type: "bit", nullable: false),
                    VoucherId = table.Column<long>(type: "bigint", nullable: false),
                    ChartofAccountSubId = table.Column<long>(type: "bigint", nullable: false),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    OfficeId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountMasters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountMasters_Accounts_ChartofAccountSubId",
                        column: x => x.ChartofAccountSubId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountMasters_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_AccountMasters_Offices_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "Offices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_AccountMasters_UserProfiles_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "AccountDetails",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    AccountDetailsAlias = table.Column<long>(type: "bigint", nullable: false),
                    IntegrationFlag = table.Column<bool>(type: "bit", nullable: false),
                    VoucherId = table.Column<long>(type: "bigint", nullable: false),
                    TransactionId = table.Column<long>(type: "bigint", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Credit = table.Column<double>(type: "float", nullable: false),
                    Debit = table.Column<double>(type: "float", nullable: false),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    OfficeId = table.Column<long>(type: "bigint", nullable: false),
                    AccountMasterId = table.Column<long>(type: "bigint", nullable: false),
                    AccountClassAlias = table.Column<long>(type: "bigint", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountDetails_AccountMasters_AccountMasterId",
                        column: x => x.AccountMasterId,
                        principalTable: "AccountMasters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountDetails_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_AccountDetails_Offices_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "Offices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_AccountDetails_UserProfiles_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "SBUAccountMasters",
                columns: table => new
                {
                    StrategicBusinessUnitId = table.Column<long>(type: "bigint", nullable: false),
                    AccountMasterId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SBUAccountMasters", x => new { x.AccountMasterId, x.StrategicBusinessUnitId });
                    table.ForeignKey(
                        name: "FK_SBUAccountMasters_AccountMasters_AccountMasterId",
                        column: x => x.AccountMasterId,
                        principalTable: "AccountMasters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SBUAccountMasters_StrategicBusinessUnits_StrategicBusinessUnitId",
                        column: x => x.StrategicBusinessUnitId,
                        principalTable: "StrategicBusinessUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_SBUId",
                table: "UserProfiles",
                column: "SBUId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountClasses_CreatedById",
                table: "AccountClasses",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_AccountDetails_AccountMasterId",
                table: "AccountDetails",
                column: "AccountMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountDetails_BranchId",
                table: "AccountDetails",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountDetails_CreatedById",
                table: "AccountDetails",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_AccountDetails_OfficeId",
                table: "AccountDetails",
                column: "OfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountMasters_BranchId",
                table: "AccountMasters",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountMasters_ChartofAccountSubId",
                table: "AccountMasters",
                column: "ChartofAccountSubId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountMasters_CreatedById",
                table: "AccountMasters",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_AccountMasters_OfficeId",
                table: "AccountMasters",
                column: "OfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_AccountClassId",
                table: "Accounts",
                column: "AccountClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_CreatedById",
                table: "Accounts",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_FinanceVoucherTypes_CreatedById",
                table: "FinanceVoucherTypes",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_LeadOrigins_CreatedById",
                table: "LeadOrigins",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_LeadOrigins_LeadTypeId",
                table: "LeadOrigins",
                column: "LeadTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadTypes_CreatedById",
                table: "LeadTypes",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SBUAccountMasters_StrategicBusinessUnitId",
                table: "SBUAccountMasters",
                column: "StrategicBusinessUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceCategoryTasks_CreatedById",
                table: "ServiceCategoryTasks",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceCategoryTasks_ServiceCategoryId",
                table: "ServiceCategoryTasks",
                column: "ServiceCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_StrategicBusinessUnits_SBUId",
                table: "UserProfiles",
                column: "SBUId",
                principalTable: "StrategicBusinessUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_StrategicBusinessUnits_SBUId",
                table: "UserProfiles");

            migrationBuilder.DropTable(
                name: "AccountDetails");

            migrationBuilder.DropTable(
                name: "FinanceVoucherTypes");

            migrationBuilder.DropTable(
                name: "LeadOrigins");

            migrationBuilder.DropTable(
                name: "SBUAccountMasters");

            migrationBuilder.DropTable(
                name: "ServiceCategoryTasks");

            migrationBuilder.DropTable(
                name: "LeadTypes");

            migrationBuilder.DropTable(
                name: "AccountMasters");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "AccountClasses");

            migrationBuilder.DropIndex(
                name: "IX_UserProfiles_SBUId",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "SBUId",
                table: "UserProfiles");
        }
    }
}
