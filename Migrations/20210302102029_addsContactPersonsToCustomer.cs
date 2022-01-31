using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class addsContactPersonsToCustomer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CustomerId",
                table: "LeadKeyPeople",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PrimaryContactId",
                table: "Customers",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SecondaryContactId",
                table: "Customers",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LeadKeyPeople_CustomerId",
                table: "LeadKeyPeople",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_PrimaryContactId",
                table: "Customers",
                column: "PrimaryContactId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_SecondaryContactId",
                table: "Customers",
                column: "SecondaryContactId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_LeadContacts_PrimaryContactId",
                table: "Customers",
                column: "PrimaryContactId",
                principalTable: "LeadContacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_LeadContacts_SecondaryContactId",
                table: "Customers",
                column: "SecondaryContactId",
                principalTable: "LeadContacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_LeadKeyPeople_Customers_CustomerId",
                table: "LeadKeyPeople",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_LeadContacts_PrimaryContactId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_LeadContacts_SecondaryContactId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_LeadKeyPeople_Customers_CustomerId",
                table: "LeadKeyPeople");

            migrationBuilder.DropIndex(
                name: "IX_LeadKeyPeople_CustomerId",
                table: "LeadKeyPeople");

            migrationBuilder.DropIndex(
                name: "IX_Customers_PrimaryContactId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_SecondaryContactId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "LeadKeyPeople");

            migrationBuilder.DropColumn(
                name: "PrimaryContactId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "SecondaryContactId",
                table: "Customers");
        }
    }
}
