using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class updatesContractServiceForEndorsementField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateForNewContractToTakeEffect",
                table: "ContractServiceForEndorsements",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateForNewContractToTakeEffect",
                table: "ContractServiceForEndorsements");
        }
    }
}
