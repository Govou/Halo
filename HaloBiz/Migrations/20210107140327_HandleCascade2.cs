using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class HandleCascade2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ModificationHistories_UserProfiles_ChangedById",
                table: "ModificationHistories");

            migrationBuilder.AlterColumn<long>(
                name: "ChangedById",
                table: "ModificationHistories",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ModificationHistories_UserProfiles_ChangedById",
                table: "ModificationHistories",
                column: "ChangedById",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ModificationHistories_UserProfiles_ChangedById",
                table: "ModificationHistories");

            migrationBuilder.AlterColumn<long>(
                name: "ChangedById",
                table: "ModificationHistories",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_ModificationHistories_UserProfiles_ChangedById",
                table: "ModificationHistories",
                column: "ChangedById",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
