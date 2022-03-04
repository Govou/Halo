using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class UpdateSomeFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_ControlAccount_ControlAccountId",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_ControlAccount_AccountClasses_AccountClassId",
                table: "ControlAccount");

            migrationBuilder.DropForeignKey(
                name: "FK_ControlAccount_UserProfiles_CreatedById",
                table: "ControlAccount");

            migrationBuilder.DropForeignKey(
                name: "FK_Region_Branches_BranchId",
                table: "Region");

            migrationBuilder.DropForeignKey(
                name: "FK_Region_UserProfiles_CreatedById",
                table: "Region");

            migrationBuilder.DropForeignKey(
                name: "FK_Region_UserProfiles_HeadId",
                table: "Region");

            migrationBuilder.DropForeignKey(
                name: "FK_RequredServiceQualificationElements_ServiceCategoryTasks_ServiceCategoryTaskId",
                table: "RequredServiceQualificationElements");

            migrationBuilder.DropForeignKey(
                name: "FK_Zone_LGAs_LGAId",
                table: "Zone");

            migrationBuilder.DropForeignKey(
                name: "FK_Zone_Region_RegionId",
                table: "Zone");

            migrationBuilder.DropForeignKey(
                name: "FK_Zone_States_StateId",
                table: "Zone");

            migrationBuilder.DropForeignKey(
                name: "FK_Zone_UserProfiles_CreatedById",
                table: "Zone");

            migrationBuilder.DropForeignKey(
                name: "FK_Zone_UserProfiles_HeadId",
                table: "Zone");

            migrationBuilder.DropIndex(
                name: "IX_RequredServiceQualificationElements_ServiceCategoryTaskId",
                table: "RequredServiceQualificationElements");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Zone",
                table: "Zone");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Region",
                table: "Region");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ControlAccount",
                table: "ControlAccount");

            migrationBuilder.DropColumn(
                name: "ServiceCategoryTaskId",
                table: "RequredServiceQualificationElements");

            migrationBuilder.RenameTable(
                name: "Zone",
                newName: "Zones");

            migrationBuilder.RenameTable(
                name: "Region",
                newName: "Regions");

            migrationBuilder.RenameTable(
                name: "ControlAccount",
                newName: "ControlAccounts");

            migrationBuilder.RenameIndex(
                name: "IX_Zone_StateId",
                table: "Zones",
                newName: "IX_Zones_StateId");

            migrationBuilder.RenameIndex(
                name: "IX_Zone_RegionId",
                table: "Zones",
                newName: "IX_Zones_RegionId");

            migrationBuilder.RenameIndex(
                name: "IX_Zone_LGAId",
                table: "Zones",
                newName: "IX_Zones_LGAId");

            migrationBuilder.RenameIndex(
                name: "IX_Zone_HeadId",
                table: "Zones",
                newName: "IX_Zones_HeadId");

            migrationBuilder.RenameIndex(
                name: "IX_Zone_CreatedById",
                table: "Zones",
                newName: "IX_Zones_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Region_HeadId",
                table: "Regions",
                newName: "IX_Regions_HeadId");

            migrationBuilder.RenameIndex(
                name: "IX_Region_CreatedById",
                table: "Regions",
                newName: "IX_Regions_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Region_BranchId",
                table: "Regions",
                newName: "IX_Regions_BranchId");

            migrationBuilder.RenameIndex(
                name: "IX_ControlAccount_CreatedById",
                table: "ControlAccounts",
                newName: "IX_ControlAccounts_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_ControlAccount_AccountClassId",
                table: "ControlAccounts",
                newName: "IX_ControlAccounts_AccountClassId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Zones",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Zones",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Regions",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Regions",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "ControlAccounts",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "ControlAccounts",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<long>(
                name: "Alias",
                table: "ControlAccounts",
                type: "bigint",
                maxLength: 100,
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Zones",
                table: "Zones",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Regions",
                table: "Regions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ControlAccounts",
                table: "ControlAccounts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_ControlAccounts_ControlAccountId",
                table: "Accounts",
                column: "ControlAccountId",
                principalTable: "ControlAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ControlAccounts_AccountClasses_AccountClassId",
                table: "ControlAccounts",
                column: "AccountClassId",
                principalTable: "AccountClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ControlAccounts_UserProfiles_CreatedById",
                table: "ControlAccounts",
                column: "CreatedById",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Regions_Branches_BranchId",
                table: "Regions",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Regions_UserProfiles_CreatedById",
                table: "Regions",
                column: "CreatedById",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Regions_UserProfiles_HeadId",
                table: "Regions",
                column: "HeadId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Zones_LGAs_LGAId",
                table: "Zones",
                column: "LGAId",
                principalTable: "LGAs",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Zones_Regions_RegionId",
                table: "Zones",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Zones_States_StateId",
                table: "Zones",
                column: "StateId",
                principalTable: "States",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Zones_UserProfiles_CreatedById",
                table: "Zones",
                column: "CreatedById",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Zones_UserProfiles_HeadId",
                table: "Zones",
                column: "HeadId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_ControlAccounts_ControlAccountId",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_ControlAccounts_AccountClasses_AccountClassId",
                table: "ControlAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_ControlAccounts_UserProfiles_CreatedById",
                table: "ControlAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Regions_Branches_BranchId",
                table: "Regions");

            migrationBuilder.DropForeignKey(
                name: "FK_Regions_UserProfiles_CreatedById",
                table: "Regions");

            migrationBuilder.DropForeignKey(
                name: "FK_Regions_UserProfiles_HeadId",
                table: "Regions");

            migrationBuilder.DropForeignKey(
                name: "FK_Zones_LGAs_LGAId",
                table: "Zones");

            migrationBuilder.DropForeignKey(
                name: "FK_Zones_Regions_RegionId",
                table: "Zones");

            migrationBuilder.DropForeignKey(
                name: "FK_Zones_States_StateId",
                table: "Zones");

            migrationBuilder.DropForeignKey(
                name: "FK_Zones_UserProfiles_CreatedById",
                table: "Zones");

            migrationBuilder.DropForeignKey(
                name: "FK_Zones_UserProfiles_HeadId",
                table: "Zones");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Zones",
                table: "Zones");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Regions",
                table: "Regions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ControlAccounts",
                table: "ControlAccounts");

            migrationBuilder.RenameTable(
                name: "Zones",
                newName: "Zone");

            migrationBuilder.RenameTable(
                name: "Regions",
                newName: "Region");

            migrationBuilder.RenameTable(
                name: "ControlAccounts",
                newName: "ControlAccount");

            migrationBuilder.RenameIndex(
                name: "IX_Zones_StateId",
                table: "Zone",
                newName: "IX_Zone_StateId");

            migrationBuilder.RenameIndex(
                name: "IX_Zones_RegionId",
                table: "Zone",
                newName: "IX_Zone_RegionId");

            migrationBuilder.RenameIndex(
                name: "IX_Zones_LGAId",
                table: "Zone",
                newName: "IX_Zone_LGAId");

            migrationBuilder.RenameIndex(
                name: "IX_Zones_HeadId",
                table: "Zone",
                newName: "IX_Zone_HeadId");

            migrationBuilder.RenameIndex(
                name: "IX_Zones_CreatedById",
                table: "Zone",
                newName: "IX_Zone_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Regions_HeadId",
                table: "Region",
                newName: "IX_Region_HeadId");

            migrationBuilder.RenameIndex(
                name: "IX_Regions_CreatedById",
                table: "Region",
                newName: "IX_Region_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Regions_BranchId",
                table: "Region",
                newName: "IX_Region_BranchId");

            migrationBuilder.RenameIndex(
                name: "IX_ControlAccounts_CreatedById",
                table: "ControlAccount",
                newName: "IX_ControlAccount_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_ControlAccounts_AccountClassId",
                table: "ControlAccount",
                newName: "IX_ControlAccount_AccountClassId");

            migrationBuilder.AddColumn<long>(
                name: "ServiceCategoryTaskId",
                table: "RequredServiceQualificationElements",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Zone",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Zone",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Region",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Region",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "ControlAccount",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "ControlAccount",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<string>(
                name: "Alias",
                table: "ControlAccount",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldMaxLength: 100);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Zone",
                table: "Zone",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Region",
                table: "Region",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ControlAccount",
                table: "ControlAccount",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_RequredServiceQualificationElements_ServiceCategoryTaskId",
                table: "RequredServiceQualificationElements",
                column: "ServiceCategoryTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_ControlAccount_ControlAccountId",
                table: "Accounts",
                column: "ControlAccountId",
                principalTable: "ControlAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ControlAccount_AccountClasses_AccountClassId",
                table: "ControlAccount",
                column: "AccountClassId",
                principalTable: "AccountClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ControlAccount_UserProfiles_CreatedById",
                table: "ControlAccount",
                column: "CreatedById",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Region_Branches_BranchId",
                table: "Region",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Region_UserProfiles_CreatedById",
                table: "Region",
                column: "CreatedById",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Region_UserProfiles_HeadId",
                table: "Region",
                column: "HeadId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RequredServiceQualificationElements_ServiceCategoryTasks_ServiceCategoryTaskId",
                table: "RequredServiceQualificationElements",
                column: "ServiceCategoryTaskId",
                principalTable: "ServiceCategoryTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Zone_LGAs_LGAId",
                table: "Zone",
                column: "LGAId",
                principalTable: "LGAs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Zone_Region_RegionId",
                table: "Zone",
                column: "RegionId",
                principalTable: "Region",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Zone_States_StateId",
                table: "Zone",
                column: "StateId",
                principalTable: "States",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Zone_UserProfiles_CreatedById",
                table: "Zone",
                column: "CreatedById",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Zone_UserProfiles_HeadId",
                table: "Zone",
                column: "HeadId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
