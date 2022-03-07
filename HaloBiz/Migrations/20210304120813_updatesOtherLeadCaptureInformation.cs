using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class updatesOtherLeadCaptureInformation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OtherLeadCaptureInfos_Leads_LeadId",
                table: "OtherLeadCaptureInfos");

            migrationBuilder.DropIndex(
                name: "IX_OtherLeadCaptureInfos_LeadId",
                table: "OtherLeadCaptureInfos");

            migrationBuilder.RenameColumn(
                name: "LeadId",
                table: "OtherLeadCaptureInfos",
                newName: "LeadDivisionId");

            migrationBuilder.AlterColumn<double>(
                name: "IndividualEstimatedAnnualIncome",
                table: "OtherLeadCaptureInfos",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<double>(
                name: "IndividualDisposableIncome",
                table: "OtherLeadCaptureInfos",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<double>(
                name: "CooperateEstimatedAnnualProfit",
                table: "OtherLeadCaptureInfos",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<double>(
                name: "CooperateEstimatedAnnualIncome",
                table: "OtherLeadCaptureInfos",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<long>(
                name: "CustomerDivisionId",
                table: "OtherLeadCaptureInfos",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OtherLeadCaptureInfos_CustomerDivisionId",
                table: "OtherLeadCaptureInfos",
                column: "CustomerDivisionId",
                unique: true,
                filter: "[CustomerDivisionId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_OtherLeadCaptureInfos_LeadDivisionId",
                table: "OtherLeadCaptureInfos",
                column: "LeadDivisionId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OtherLeadCaptureInfos_CustomerDivisions_CustomerDivisionId",
                table: "OtherLeadCaptureInfos",
                column: "CustomerDivisionId",
                principalTable: "CustomerDivisions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OtherLeadCaptureInfos_LeadDivisions_LeadDivisionId",
                table: "OtherLeadCaptureInfos",
                column: "LeadDivisionId",
                principalTable: "LeadDivisions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OtherLeadCaptureInfos_CustomerDivisions_CustomerDivisionId",
                table: "OtherLeadCaptureInfos");

            migrationBuilder.DropForeignKey(
                name: "FK_OtherLeadCaptureInfos_LeadDivisions_LeadDivisionId",
                table: "OtherLeadCaptureInfos");

            migrationBuilder.DropIndex(
                name: "IX_OtherLeadCaptureInfos_CustomerDivisionId",
                table: "OtherLeadCaptureInfos");

            migrationBuilder.DropIndex(
                name: "IX_OtherLeadCaptureInfos_LeadDivisionId",
                table: "OtherLeadCaptureInfos");

            migrationBuilder.DropColumn(
                name: "CustomerDivisionId",
                table: "OtherLeadCaptureInfos");

            migrationBuilder.RenameColumn(
                name: "LeadDivisionId",
                table: "OtherLeadCaptureInfos",
                newName: "LeadId");

            migrationBuilder.AlterColumn<decimal>(
                name: "IndividualEstimatedAnnualIncome",
                table: "OtherLeadCaptureInfos",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "IndividualDisposableIncome",
                table: "OtherLeadCaptureInfos",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "CooperateEstimatedAnnualProfit",
                table: "OtherLeadCaptureInfos",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "CooperateEstimatedAnnualIncome",
                table: "OtherLeadCaptureInfos",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.CreateIndex(
                name: "IX_OtherLeadCaptureInfos_LeadId",
                table: "OtherLeadCaptureInfos",
                column: "LeadId");

            migrationBuilder.AddForeignKey(
                name: "FK_OtherLeadCaptureInfos_Leads_LeadId",
                table: "OtherLeadCaptureInfos",
                column: "LeadId",
                principalTable: "Leads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
