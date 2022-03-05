using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class variousLeadChangesSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("SET IDENTITY_INSERT Designations ON");
            migrationBuilder.Sql(@"IF NOT EXISTS(SELECT 1 FROM dbo.Designations WITH(NOLOCK))
                                    BEGIN 
                                        INSERT INTO Designations (Id, Caption, Description, IsDeleted, CreatedById) VALUES 
                                        (1, 'CEO', 'CEO', 0, 1),
                                        (2, 'CFO', 'CFO', 0, 1),
                                        (3, 'CTO', 'CTO', 0, 1)                                   
                                    END
            ");
            migrationBuilder.Sql("SET IDENTITY_INSERT Designations OFF");

            migrationBuilder.Sql("SET IDENTITY_INSERT EngagementTypes ON");
            migrationBuilder.Sql(@"IF NOT EXISTS(SELECT 1 FROM dbo.EngagementTypes WITH(NOLOCK))
                                    BEGIN 
                                        INSERT INTO EngagementTypes (Id, Caption, Description, IsDeleted, CreatedById) VALUES 
                                        (1, 'Email', 'Email', 0, 1),
                                        (2, 'Phone Call', 'Phone Call', 0, 1),
                                        (3, 'Client Visit', 'Client Visit', 0, 1),                                   
                                        (4, 'Visit', 'Visit', 0, 1)                                   
                                    END
            ");
            migrationBuilder.Sql("SET IDENTITY_INSERT EngagementTypes OFF");

            migrationBuilder.Sql("SET IDENTITY_INSERT ClientContactQualifications ON");
            migrationBuilder.Sql(@"IF NOT EXISTS(SELECT 1 FROM dbo.ClientContactQualifications WITH(NOLOCK))
                                    BEGIN 
                                        INSERT INTO ClientContactQualifications (Id, Caption, Description, IsDeleted, CreatedById) VALUES 
                                        (1, 'Influencer', 'Influencer', 0, 1),
                                        (2, 'Decision Maker', 'Decision Maker', 0, 1)                       
                                    END
            ");
            migrationBuilder.Sql("SET IDENTITY_INSERT ClientContactQualifications OFF");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
