using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class addedCreatedByToClientBeneficiaryAndEndorsementType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CreatedById",
                table: "EndorsementTypes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "CreatedById",
                table: "ClientBeneficiaries",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "EndorsementTypes");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "ClientBeneficiaries");
        }
    }
}
