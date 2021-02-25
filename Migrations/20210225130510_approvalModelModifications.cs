using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class approvalModelModifications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Approvals_Contracts_ContractId",
                table: "Approvals");

            migrationBuilder.DropForeignKey(
                name: "FK_Approvals_ContractServices_ContractServiceId",
                table: "Approvals");

            migrationBuilder.DropForeignKey(
                name: "FK_Approvals_Services_ServicesId",
                table: "Approvals");

            migrationBuilder.AlterColumn<long>(
                name: "ServicesId",
                table: "Approvals",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateTimeApproved",
                table: "Approvals",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<long>(
                name: "ContractServiceId",
                table: "Approvals",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "ContractId",
                table: "Approvals",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_Approvals_Contracts_ContractId",
                table: "Approvals",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Approvals_ContractServices_ContractServiceId",
                table: "Approvals",
                column: "ContractServiceId",
                principalTable: "ContractServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Approvals_Services_ServicesId",
                table: "Approvals",
                column: "ServicesId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Approvals_Contracts_ContractId",
                table: "Approvals");

            migrationBuilder.DropForeignKey(
                name: "FK_Approvals_ContractServices_ContractServiceId",
                table: "Approvals");

            migrationBuilder.DropForeignKey(
                name: "FK_Approvals_Services_ServicesId",
                table: "Approvals");

            migrationBuilder.AlterColumn<long>(
                name: "ServicesId",
                table: "Approvals",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateTimeApproved",
                table: "Approvals",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ContractServiceId",
                table: "Approvals",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ContractId",
                table: "Approvals",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Approvals_Contracts_ContractId",
                table: "Approvals",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Approvals_ContractServices_ContractServiceId",
                table: "Approvals",
                column: "ContractServiceId",
                principalTable: "ContractServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Approvals_Services_ServicesId",
                table: "Approvals",
                column: "ServicesId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
