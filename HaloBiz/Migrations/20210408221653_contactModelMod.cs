using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class contactModelMod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Designation",
                table: "LeadKeyPeople");

            migrationBuilder.DropColumn(
                name: "Designation",
                table: "LeadDivisionKeyPeople");

            migrationBuilder.DropColumn(
                name: "Designation",
                table: "LeadDivisionContacts");

            migrationBuilder.DropColumn(
                name: "Designation",
                table: "LeadContacts");

            migrationBuilder.AddColumn<long>(
                name: "ClientContactQualificationId",
                table: "LeadKeyPeople",
                type: "bigint",
                nullable: false,
                defaultValue: 1L);

            migrationBuilder.AddColumn<long>(
                name: "DesignationId",
                table: "LeadKeyPeople",
                type: "bigint",
                nullable: false,
                defaultValue: 1L);

            migrationBuilder.AddColumn<long>(
                name: "ClientContactQualificationId",
                table: "LeadDivisionKeyPeople",
                type: "bigint",
                nullable: false,
                defaultValue: 1L);

            migrationBuilder.AddColumn<long>(
                name: "DesignationId",
                table: "LeadDivisionKeyPeople",
                type: "bigint",
                nullable: false,
                defaultValue: 1L);

            migrationBuilder.AddColumn<long>(
                name: "ClientContactQualificationId",
                table: "LeadDivisionContacts",
                type: "bigint",
                nullable: false,
                defaultValue: 1L);

            migrationBuilder.AddColumn<long>(
                name: "DesignationId",
                table: "LeadDivisionContacts",
                type: "bigint",
                nullable: false,
                defaultValue: 1L);

            migrationBuilder.AddColumn<long>(
                name: "ClientContactQualificationId",
                table: "LeadContacts",
                type: "bigint",
                nullable: false,
                defaultValue: 1L);

            migrationBuilder.AddColumn<long>(
                name: "DesignationId",
                table: "LeadContacts",
                type: "bigint",
                nullable: false,
                defaultValue: 1L);

            migrationBuilder.CreateIndex(
                name: "IX_LeadKeyPeople_ClientContactQualificationId",
                table: "LeadKeyPeople",
                column: "ClientContactQualificationId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadKeyPeople_DesignationId",
                table: "LeadKeyPeople",
                column: "DesignationId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadDivisionKeyPeople_ClientContactQualificationId",
                table: "LeadDivisionKeyPeople",
                column: "ClientContactQualificationId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadDivisionKeyPeople_DesignationId",
                table: "LeadDivisionKeyPeople",
                column: "DesignationId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadDivisionContacts_ClientContactQualificationId",
                table: "LeadDivisionContacts",
                column: "ClientContactQualificationId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadDivisionContacts_DesignationId",
                table: "LeadDivisionContacts",
                column: "DesignationId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadContacts_ClientContactQualificationId",
                table: "LeadContacts",
                column: "ClientContactQualificationId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadContacts_DesignationId",
                table: "LeadContacts",
                column: "DesignationId");

            migrationBuilder.AddForeignKey(
                name: "FK_LeadContacts_ClientContactQualifications_ClientContactQualificationId",
                table: "LeadContacts",
                column: "ClientContactQualificationId",
                principalTable: "ClientContactQualifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_LeadContacts_Designations_DesignationId",
                table: "LeadContacts",
                column: "DesignationId",
                principalTable: "Designations",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_LeadDivisionContacts_ClientContactQualifications_ClientContactQualificationId",
                table: "LeadDivisionContacts",
                column: "ClientContactQualificationId",
                principalTable: "ClientContactQualifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_LeadDivisionContacts_Designations_DesignationId",
                table: "LeadDivisionContacts",
                column: "DesignationId",
                principalTable: "Designations",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_LeadDivisionKeyPeople_ClientContactQualifications_ClientContactQualificationId",
                table: "LeadDivisionKeyPeople",
                column: "ClientContactQualificationId",
                principalTable: "ClientContactQualifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_LeadDivisionKeyPeople_Designations_DesignationId",
                table: "LeadDivisionKeyPeople",
                column: "DesignationId",
                principalTable: "Designations",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_LeadKeyPeople_ClientContactQualifications_ClientContactQualificationId",
                table: "LeadKeyPeople",
                column: "ClientContactQualificationId",
                principalTable: "ClientContactQualifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_LeadKeyPeople_Designations_DesignationId",
                table: "LeadKeyPeople",
                column: "DesignationId",
                principalTable: "Designations",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeadContacts_ClientContactQualifications_ClientContactQualificationId",
                table: "LeadContacts");

            migrationBuilder.DropForeignKey(
                name: "FK_LeadContacts_Designations_DesignationId",
                table: "LeadContacts");

            migrationBuilder.DropForeignKey(
                name: "FK_LeadDivisionContacts_ClientContactQualifications_ClientContactQualificationId",
                table: "LeadDivisionContacts");

            migrationBuilder.DropForeignKey(
                name: "FK_LeadDivisionContacts_Designations_DesignationId",
                table: "LeadDivisionContacts");

            migrationBuilder.DropForeignKey(
                name: "FK_LeadDivisionKeyPeople_ClientContactQualifications_ClientContactQualificationId",
                table: "LeadDivisionKeyPeople");

            migrationBuilder.DropForeignKey(
                name: "FK_LeadDivisionKeyPeople_Designations_DesignationId",
                table: "LeadDivisionKeyPeople");

            migrationBuilder.DropForeignKey(
                name: "FK_LeadKeyPeople_ClientContactQualifications_ClientContactQualificationId",
                table: "LeadKeyPeople");

            migrationBuilder.DropForeignKey(
                name: "FK_LeadKeyPeople_Designations_DesignationId",
                table: "LeadKeyPeople");

            migrationBuilder.DropIndex(
                name: "IX_LeadKeyPeople_ClientContactQualificationId",
                table: "LeadKeyPeople");

            migrationBuilder.DropIndex(
                name: "IX_LeadKeyPeople_DesignationId",
                table: "LeadKeyPeople");

            migrationBuilder.DropIndex(
                name: "IX_LeadDivisionKeyPeople_ClientContactQualificationId",
                table: "LeadDivisionKeyPeople");

            migrationBuilder.DropIndex(
                name: "IX_LeadDivisionKeyPeople_DesignationId",
                table: "LeadDivisionKeyPeople");

            migrationBuilder.DropIndex(
                name: "IX_LeadDivisionContacts_ClientContactQualificationId",
                table: "LeadDivisionContacts");

            migrationBuilder.DropIndex(
                name: "IX_LeadDivisionContacts_DesignationId",
                table: "LeadDivisionContacts");

            migrationBuilder.DropIndex(
                name: "IX_LeadContacts_ClientContactQualificationId",
                table: "LeadContacts");

            migrationBuilder.DropIndex(
                name: "IX_LeadContacts_DesignationId",
                table: "LeadContacts");

            migrationBuilder.DropColumn(
                name: "ClientContactQualificationId",
                table: "LeadKeyPeople");

            migrationBuilder.DropColumn(
                name: "DesignationId",
                table: "LeadKeyPeople");

            migrationBuilder.DropColumn(
                name: "ClientContactQualificationId",
                table: "LeadDivisionKeyPeople");

            migrationBuilder.DropColumn(
                name: "DesignationId",
                table: "LeadDivisionKeyPeople");

            migrationBuilder.DropColumn(
                name: "ClientContactQualificationId",
                table: "LeadDivisionContacts");

            migrationBuilder.DropColumn(
                name: "DesignationId",
                table: "LeadDivisionContacts");

            migrationBuilder.DropColumn(
                name: "ClientContactQualificationId",
                table: "LeadContacts");

            migrationBuilder.DropColumn(
                name: "DesignationId",
                table: "LeadContacts");

            migrationBuilder.AddColumn<string>(
                name: "Designation",
                table: "LeadKeyPeople",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Designation",
                table: "LeadDivisionKeyPeople",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Designation",
                table: "LeadDivisionContacts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Designation",
                table: "LeadContacts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
