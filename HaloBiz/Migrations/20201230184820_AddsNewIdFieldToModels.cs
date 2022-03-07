using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class AddsNewIdFieldToModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Branches_UserProfiles_HeadId",
                table: "Branches");

            migrationBuilder.DropForeignKey(
                name: "FK_Divisions_UserProfiles_HeadId",
                table: "Divisions");

            migrationBuilder.DropForeignKey(
                name: "FK_LGAs_States_StateId",
                table: "LGAs");

            migrationBuilder.DropForeignKey(
                name: "FK_Offices_LGAs_LGAId",
                table: "Offices");

            migrationBuilder.DropForeignKey(
                name: "FK_Offices_States_StateId",
                table: "Offices");

            migrationBuilder.DropForeignKey(
                name: "FK_Offices_UserProfiles_HeadId",
                table: "Offices");

            migrationBuilder.DropForeignKey(
                name: "FK_OperatingEntities_UserProfiles_HeadId",
                table: "OperatingEntities");

            migrationBuilder.AddColumn<long>(
                name: "DivisionId",
                table: "Services",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "OperatingEntityId",
                table: "Services",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ServiceGroupId",
                table: "Services",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "DivisionId",
                table: "ServiceGroups",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "DivisionId",
                table: "ServiceCategories",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "OperatingEntityId",
                table: "ServiceCategories",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddForeignKey(
                name: "FK_Branches_UserProfiles_HeadId",
                table: "Branches",
                column: "HeadId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Divisions_UserProfiles_HeadId",
                table: "Divisions",
                column: "HeadId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_LGAs_States_StateId",
                table: "LGAs",
                column: "StateId",
                principalTable: "States",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Offices_LGAs_LGAId",
                table: "Offices",
                column: "LGAId",
                principalTable: "LGAs",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Offices_States_StateId",
                table: "Offices",
                column: "StateId",
                principalTable: "States",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Offices_UserProfiles_HeadId",
                table: "Offices",
                column: "HeadId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_OperatingEntities_UserProfiles_HeadId",
                table: "OperatingEntities",
                column: "HeadId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Branches_UserProfiles_HeadId",
                table: "Branches");

            migrationBuilder.DropForeignKey(
                name: "FK_Divisions_UserProfiles_HeadId",
                table: "Divisions");

            migrationBuilder.DropForeignKey(
                name: "FK_LGAs_States_StateId",
                table: "LGAs");

            migrationBuilder.DropForeignKey(
                name: "FK_Offices_LGAs_LGAId",
                table: "Offices");

            migrationBuilder.DropForeignKey(
                name: "FK_Offices_States_StateId",
                table: "Offices");

            migrationBuilder.DropForeignKey(
                name: "FK_Offices_UserProfiles_HeadId",
                table: "Offices");

            migrationBuilder.DropForeignKey(
                name: "FK_OperatingEntities_UserProfiles_HeadId",
                table: "OperatingEntities");

            migrationBuilder.DropColumn(
                name: "DivisionId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "OperatingEntityId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "ServiceGroupId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "DivisionId",
                table: "ServiceGroups");

            migrationBuilder.DropColumn(
                name: "DivisionId",
                table: "ServiceCategories");

            migrationBuilder.DropColumn(
                name: "OperatingEntityId",
                table: "ServiceCategories");

            migrationBuilder.AddForeignKey(
                name: "FK_Branches_UserProfiles_HeadId",
                table: "Branches",
                column: "HeadId",
                principalTable: "UserProfiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Divisions_UserProfiles_HeadId",
                table: "Divisions",
                column: "HeadId",
                principalTable: "UserProfiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LGAs_States_StateId",
                table: "LGAs",
                column: "StateId",
                principalTable: "States",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Offices_LGAs_LGAId",
                table: "Offices",
                column: "LGAId",
                principalTable: "LGAs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Offices_States_StateId",
                table: "Offices",
                column: "StateId",
                principalTable: "States",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Offices_UserProfiles_HeadId",
                table: "Offices",
                column: "HeadId",
                principalTable: "UserProfiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OperatingEntities_UserProfiles_HeadId",
                table: "OperatingEntities",
                column: "HeadId",
                principalTable: "UserProfiles",
                principalColumn: "Id");
        }
    }
}
