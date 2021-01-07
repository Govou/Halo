﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class HandleCascade : Migration
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

            migrationBuilder.AddForeignKey(
                name: "FK_Branches_UserProfiles_HeadId",
                table: "Branches",
                column: "HeadId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Divisions_UserProfiles_HeadId",
                table: "Divisions",
                column: "HeadId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LGAs_States_StateId",
                table: "LGAs",
                column: "StateId",
                principalTable: "States",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Offices_LGAs_LGAId",
                table: "Offices",
                column: "LGAId",
                principalTable: "LGAs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Offices_States_StateId",
                table: "Offices",
                column: "StateId",
                principalTable: "States",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Offices_UserProfiles_HeadId",
                table: "Offices",
                column: "HeadId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OperatingEntities_UserProfiles_HeadId",
                table: "OperatingEntities",
                column: "HeadId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
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
