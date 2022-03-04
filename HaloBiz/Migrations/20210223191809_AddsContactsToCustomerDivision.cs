using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class AddsContactsToCustomerDivision : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CustomerDivisionId",
                table: "LeadKeyPeople",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CustomerDivisionId",
                table: "LeadDivisionKeyPeople",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PrimaryContactId",
                table: "CustomerDivisions",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SecondaryContactId",
                table: "CustomerDivisions",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LeadKeyPeople_CustomerDivisionId",
                table: "LeadKeyPeople",
                column: "CustomerDivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadDivisionKeyPeople_CustomerDivisionId",
                table: "LeadDivisionKeyPeople",
                column: "CustomerDivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerDivisions_PrimaryContactId",
                table: "CustomerDivisions",
                column: "PrimaryContactId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerDivisions_SecondaryContactId",
                table: "CustomerDivisions",
                column: "SecondaryContactId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerDivisions_LeadDivisionContacts_PrimaryContactId",
                table: "CustomerDivisions",
                column: "PrimaryContactId",
                principalTable: "LeadDivisionContacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerDivisions_LeadDivisionContacts_SecondaryContactId",
                table: "CustomerDivisions",
                column: "SecondaryContactId",
                principalTable: "LeadDivisionContacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_LeadDivisionKeyPeople_CustomerDivisions_CustomerDivisionId",
                table: "LeadDivisionKeyPeople",
                column: "CustomerDivisionId",
                principalTable: "CustomerDivisions",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_LeadKeyPeople_CustomerDivisions_CustomerDivisionId",
                table: "LeadKeyPeople",
                column: "CustomerDivisionId",
                principalTable: "CustomerDivisions",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerDivisions_LeadDivisionContacts_PrimaryContactId",
                table: "CustomerDivisions");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerDivisions_LeadDivisionContacts_SecondaryContactId",
                table: "CustomerDivisions");

            migrationBuilder.DropForeignKey(
                name: "FK_LeadDivisionKeyPeople_CustomerDivisions_CustomerDivisionId",
                table: "LeadDivisionKeyPeople");

            migrationBuilder.DropForeignKey(
                name: "FK_LeadKeyPeople_CustomerDivisions_CustomerDivisionId",
                table: "LeadKeyPeople");

            migrationBuilder.DropIndex(
                name: "IX_LeadKeyPeople_CustomerDivisionId",
                table: "LeadKeyPeople");

            migrationBuilder.DropIndex(
                name: "IX_LeadDivisionKeyPeople_CustomerDivisionId",
                table: "LeadDivisionKeyPeople");

            migrationBuilder.DropIndex(
                name: "IX_CustomerDivisions_PrimaryContactId",
                table: "CustomerDivisions");

            migrationBuilder.DropIndex(
                name: "IX_CustomerDivisions_SecondaryContactId",
                table: "CustomerDivisions");

            migrationBuilder.DropColumn(
                name: "CustomerDivisionId",
                table: "LeadKeyPeople");

            migrationBuilder.DropColumn(
                name: "CustomerDivisionId",
                table: "LeadDivisionKeyPeople");

            migrationBuilder.DropColumn(
                name: "PrimaryContactId",
                table: "CustomerDivisions");

            migrationBuilder.DropColumn(
                name: "SecondaryContactId",
                table: "CustomerDivisions");
        }
    }
}
