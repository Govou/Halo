using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class custdiv : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "LGAId",
                table: "CustomerDivisions",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "StateId",
                table: "CustomerDivisions",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Street",
                table: "CustomerDivisions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerDivisions_LGAId",
                table: "CustomerDivisions",
                column: "LGAId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerDivisions_StateId",
                table: "CustomerDivisions",
                column: "StateId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerDivisions_LGAs_LGAId",
                table: "CustomerDivisions",
                column: "LGAId",
                principalTable: "LGAs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerDivisions_States_StateId",
                table: "CustomerDivisions",
                column: "StateId",
                principalTable: "States",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerDivisions_LGAs_LGAId",
                table: "CustomerDivisions");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerDivisions_States_StateId",
                table: "CustomerDivisions");

            migrationBuilder.DropIndex(
                name: "IX_CustomerDivisions_LGAId",
                table: "CustomerDivisions");

            migrationBuilder.DropIndex(
                name: "IX_CustomerDivisions_StateId",
                table: "CustomerDivisions");

            migrationBuilder.DropColumn(
                name: "LGAId",
                table: "CustomerDivisions");

            migrationBuilder.DropColumn(
                name: "StateId",
                table: "CustomerDivisions");

            migrationBuilder.DropColumn(
                name: "Street",
                table: "CustomerDivisions");
        }
    }
}
