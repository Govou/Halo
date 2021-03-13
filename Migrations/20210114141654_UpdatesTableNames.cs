using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class UpdatesTableNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bank_UserProfiles_CreatedById",
                table: "Bank");

            migrationBuilder.DropForeignKey(
                name: "FK_Relationship_UserProfiles_CreatedById",
                table: "Relationship");

            migrationBuilder.DropForeignKey(
                name: "FK_Target_UserProfiles_CreatedById",
                table: "Target");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Target",
                table: "Target");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Relationship",
                table: "Relationship");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bank",
                table: "Bank");

            migrationBuilder.RenameTable(
                name: "Target",
                newName: "Targets");

            migrationBuilder.RenameTable(
                name: "Relationship",
                newName: "Relationships");

            migrationBuilder.RenameTable(
                name: "Bank",
                newName: "Banks");

            migrationBuilder.RenameIndex(
                name: "IX_Target_CreatedById",
                table: "Targets",
                newName: "IX_Targets_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Relationship_CreatedById",
                table: "Relationships",
                newName: "IX_Relationships_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Bank_CreatedById",
                table: "Banks",
                newName: "IX_Banks_CreatedById");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Targets",
                table: "Targets",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Relationships",
                table: "Relationships",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Banks",
                table: "Banks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Banks_UserProfiles_CreatedById",
                table: "Banks",
                column: "CreatedById",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Relationships_UserProfiles_CreatedById",
                table: "Relationships",
                column: "CreatedById",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Targets_UserProfiles_CreatedById",
                table: "Targets",
                column: "CreatedById",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Banks_UserProfiles_CreatedById",
                table: "Banks");

            migrationBuilder.DropForeignKey(
                name: "FK_Relationships_UserProfiles_CreatedById",
                table: "Relationships");

            migrationBuilder.DropForeignKey(
                name: "FK_Targets_UserProfiles_CreatedById",
                table: "Targets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Targets",
                table: "Targets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Relationships",
                table: "Relationships");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Banks",
                table: "Banks");

            migrationBuilder.RenameTable(
                name: "Targets",
                newName: "Target");

            migrationBuilder.RenameTable(
                name: "Relationships",
                newName: "Relationship");

            migrationBuilder.RenameTable(
                name: "Banks",
                newName: "Bank");

            migrationBuilder.RenameIndex(
                name: "IX_Targets_CreatedById",
                table: "Target",
                newName: "IX_Target_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Relationships_CreatedById",
                table: "Relationship",
                newName: "IX_Relationship_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Banks_CreatedById",
                table: "Bank",
                newName: "IX_Bank_CreatedById");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Target",
                table: "Target",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Relationship",
                table: "Relationship",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bank",
                table: "Bank",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bank_UserProfiles_CreatedById",
                table: "Bank",
                column: "CreatedById",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Relationship_UserProfiles_CreatedById",
                table: "Relationship",
                column: "CreatedById",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Target_UserProfiles_CreatedById",
                table: "Target",
                column: "CreatedById",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
