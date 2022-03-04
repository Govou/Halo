using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class AddsNewFieldsToContactsAndRequiredQuoteServiceDoc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "QuoteServiceDocuments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "LeadKeyPeople",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "LeadKeyPeople",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "LeadDivisionKeyPeople",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "LeadDivisionKeyPeople",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "LeadDivisionContacts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "LeadDivisionContacts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "LeadContacts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "LeadContacts",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "QuoteServiceDocuments");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "LeadKeyPeople");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "LeadKeyPeople");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "LeadDivisionKeyPeople");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "LeadDivisionKeyPeople");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "LeadDivisionContacts");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "LeadDivisionContacts");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "LeadContacts");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "LeadContacts");
        }
    }
}
