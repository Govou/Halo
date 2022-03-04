using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class AddsFieldToLeadModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leads_LeadContacts_PrimaryContactId",
                table: "Leads");

            migrationBuilder.AlterColumn<long>(
                name: "PrimaryContactId",
                table: "Leads",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeConvertedToClient",
                table: "Leads",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeMovedToLeadCloseure",
                table: "Leads",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeMovedToLeadOpportunity",
                table: "Leads",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeMovedToLeadQualification",
                table: "Leads",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_LeadContacts_PrimaryContactId",
                table: "Leads",
                column: "PrimaryContactId",
                principalTable: "LeadContacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leads_LeadContacts_PrimaryContactId",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "TimeConvertedToClient",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "TimeMovedToLeadCloseure",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "TimeMovedToLeadOpportunity",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "TimeMovedToLeadQualification",
                table: "Leads");

            migrationBuilder.AlterColumn<long>(
                name: "PrimaryContactId",
                table: "Leads",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_LeadContacts_PrimaryContactId",
                table: "Leads",
                column: "PrimaryContactId",
                principalTable: "LeadContacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
