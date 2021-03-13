using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class AddsSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            //Seeds AccountClasses Table
            migrationBuilder.Sql("SET IDENTITY_INSERT AccountClasses ON"); 
            migrationBuilder.Sql(@"IF NOT EXISTS(SELECT 1 FROM dbo.AccountClasses WITH(NOLOCK))
                                    BEGIN 
                                        INSERT INTO AccountClasses (Id, Caption, Description, AccountClassAlias, CreatedById, IsDeleted) VALUES (1000000000, 'ASSETS', 'Assets Account Class', '1', 1, 0),
                                        (2000000000, 'EQUITY', 'Equity Account Class', '2', 1, 0), (3000000000, 'LIABILITY', 'Liability Account Class', '3', 1, 0),  (4000000000, 'REVENUE', 'Revenue Account Class', '4', 1, 0),
                                        (5000000000, 'EXPENSES', 'Expense Account Class', '5', 1, 0)
                                    END
            ");
            migrationBuilder.Sql("SET IDENTITY_INSERT AccountClasses OFF");
            // migrationBuilder.Sql("DELETE FROM AccountClasses");
            // migrationBuilder.Sql("SET IDENTITY_INSERT AccountClasses ON"); 
            // migrationBuilder.Sql("INSERT INTO AccountClasses (Id, Caption, Description, AccountClassAlias, CreatedById) VALUES (1000000000, 'ASSETS', 'Assets Account Class', '1', 1)"); 
            // migrationBuilder.Sql("INSERT INTO AccountClasses (Id, Caption, Description, AccountClassAlias, CreatedById) VALUES (2000000000, 'EQUITY', 'Equity Account Class', '2', 1)"); 
            // migrationBuilder.Sql("INSERT INTO AccountClasses (Id, Caption, Description, AccountClassAlias, CreatedById) VALUES (3000000000, 'LIABILITY', 'Liability Account Class', '3', 1)"); 
            // migrationBuilder.Sql("INSERT INTO AccountClasses (Id, Caption, Description, AccountClassAlias, CreatedById) VALUES (4000000000, 'REVENUE', 'Revenue Account Class', '4', 1)"); 
            // migrationBuilder.Sql("INSERT INTO AccountClasses (Id, Caption, Description, AccountClassAlias, CreatedById) VALUES (5000000000, 'EXPENSES', 'Expense Account Class', '5', 1)"); 
            // migrationBuilder.Sql("SET IDENTITY_INSERT AccountClasses OFF");

            //Seeds ApprovalLevels Table
            // migrationBuilder.Sql("TRUNCATE TABLE ApproverLevels");
            // migrationBuilder.Sql("SET IDENTITY_INSERT ApproverLevels ON"); 
            // migrationBuilder.Sql("INSERT INTO ApproverLevels (Id, Caption, Description, Alias, IsDeleted, CreatedById) VALUES (4, 'Branch Head', 'Head of a region/branch','Branch Head', 0, 1)"); 
            // migrationBuilder.Sql("INSERT INTO ApproverLevels (Id, Caption, Description, Alias, IsDeleted, CreatedById) VALUES (1, 'CEO', 'CEO','CEO', 0, 1)"); 
            // migrationBuilder.Sql("INSERT INTO ApproverLevels (Id, Caption, Description, Alias, IsDeleted, CreatedById) VALUES (2, 'Division Head', 'Head of a division','Division Head', 0, 1)"); 
            // migrationBuilder.Sql("INSERT INTO ApproverLevels (Id, Caption, Description, Alias, IsDeleted, CreatedById) VALUES (3, 'Operating Entity Head', 'Head of an Operating Entity','Operating Entity Head', 0, 1)"); 
            // migrationBuilder.Sql("SET IDENTITY_INSERT ApproverLevels OFF"); 

            migrationBuilder.Sql("SET IDENTITY_INSERT ApproverLevels ON"); 
            migrationBuilder.Sql(@"IF NOT EXISTS(SELECT 1 FROM dbo.ApproverLevels WITH(NOLOCK))
                                    BEGIN 
                                        INSERT INTO ApproverLevels (Id, Caption, Description, Alias, IsDeleted, CreatedById) VALUES (4, 'Branch Head', 'Head of a region/branch','Branch Head', 0, 1),
                                        (1, 'CEO', 'CEO','CEO', 0, 1),
                                        (2, 'Division Head', 'Head of a division','Division Head', 0, 1),
                                        (3, 'Operating Entity Head', 'Head of an Operating Entity','Operating Entity Head', 0, 1)                                   
                                    END
            ");
            migrationBuilder.Sql("SET IDENTITY_INSERT ApproverLevels OFF"); 



            //Seeds FinanceVoucherType
            // migrationBuilder.Sql("TRUNCATE TABLE FinanceVoucherTypes");
            // migrationBuilder.Sql("SET IDENTITY_INSERT FinanceVoucherTypes ON"); 
            // migrationBuilder.Sql("INSERT INTO FinanceVoucherTypes (Id, VoucherType, Description, CreatedById) VALUES (1, 'Sales Receipt', 'Sales Receipt', 1)"); 
            // migrationBuilder.Sql("INSERT INTO FinanceVoucherTypes (Id, VoucherType, Description, CreatedById) VALUES (2, 'Sales Invoice', 'Sales Invoice', 1)"); 
            // migrationBuilder.Sql("SET IDENTITY_INSERT FinanceVoucherTypes OFF"); 

            migrationBuilder.Sql("SET IDENTITY_INSERT FinanceVoucherTypes ON"); 
            migrationBuilder.Sql(@"IF NOT EXISTS(SELECT 1 FROM dbo.FinanceVoucherTypes WITH(NOLOCK))
                                    BEGIN 
                                        INSERT INTO FinanceVoucherTypes (Id, VoucherType, Description, CreatedById, IsDeleted) VALUES (1, 'Sales Receipt', 'Sales Receipt', 1, 0),
                                        (2, 'Sales Invoice', 'Sales Invoice', 1, 0)
                                    END
            ");
             migrationBuilder.Sql("SET IDENTITY_INSERT FinanceVoucherTypes OFF"); 



            //Seeds ProcessesRequiringApprovals Table
            // migrationBuilder.Sql("TRUNCATE TABLE ProcessesRequiringApprovals");
            // migrationBuilder.Sql("SET IDENTITY_INSERT ProcessesRequiringApprovals ON"); 
            // migrationBuilder.Sql("INSERT INTO ProcessesRequiringApprovals (Id, Caption, Description, CreatedById) VALUES (1, 'Contract Creation', 'Contract Creation', 1)"); 
            // migrationBuilder.Sql("INSERT INTO ProcessesRequiringApprovals (Id, Caption, Description, CreatedById) VALUES (2, 'Service Creation', 'Service Creation', 1)"); 
            // migrationBuilder.Sql("SET IDENTITY_INSERT ProcessesRequiringApprovals OFF"); 

            migrationBuilder.Sql("SET IDENTITY_INSERT ProcessesRequiringApprovals ON"); 
            migrationBuilder.Sql(@"IF NOT EXISTS(SELECT 1 FROM dbo.ProcessesRequiringApprovals WITH(NOLOCK))
                                    BEGIN 
                                        INSERT INTO ProcessesRequiringApprovals (Id, Caption, Description, CreatedById, IsDeleted) VALUES (1, 'Contract Creation', 'Contract Creation', 1, 0),
                                        (2, 'Service Creation', 'Service Creation', 1, 0)
                                    END
            ");
             migrationBuilder.Sql("SET IDENTITY_INSERT ProcessesRequiringApprovals OFF"); 




            //Seeds GroupTypes
            // migrationBuilder.Sql("TRUNCATE TABLE GroupTypes");
            // migrationBuilder.Sql("SET IDENTITY_INSERT GroupTypes ON"); 
            // migrationBuilder.Sql("INSERT INTO GroupTypes (Id, Caption, Description, CreatedById) VALUES (1, 'Corporate', 'Corporate', 1)"); 
            // migrationBuilder.Sql("INSERT INTO GroupTypes (Id, Caption, Description, CreatedById) VALUES (2, 'Government', 'Government', 1)"); 
            // migrationBuilder.Sql("INSERT INTO GroupTypes (Id, Caption, Description, CreatedById) VALUES (3, 'Individual', 'Individual', 1)"); 
            // migrationBuilder.Sql("INSERT INTO GroupTypes (Id, Caption, Description, CreatedById) VALUES (4, 'SME', 'SME', 1)"); 
            // migrationBuilder.Sql("SET IDENTITY_INSERT GroupTypes OFF"); 

            migrationBuilder.Sql("SET IDENTITY_INSERT GroupType ON"); 
                        migrationBuilder.Sql(@"IF NOT EXISTS(SELECT 1 FROM dbo.GroupType WITH(NOLOCK))
                                    BEGIN 
                                        INSERT INTO GroupType (Id, Caption, Description, CreatedById, IsDeleted) VALUES (1, 'Corporate', 'Corporate', 1, 0),
                                        (2, 'Government', 'Government', 1, 0),
                                        (3, 'Individual', 'Individual', 1, 0),
                                        (4, 'SME', 'SME', 1, 0)
                                    END
            ");
             migrationBuilder.Sql("SET IDENTITY_INSERT GroupType OFF"); 



            //Meeans of Identification
            // migrationBuilder.Sql("TRUNCATE TABLE MeansOfIdentification");
            // migrationBuilder.Sql("SET IDENTITY_INSERT MeansOfIdentification ON"); 
            // migrationBuilder.Sql("INSERT INTO MeansOfIdentification (Id, Caption, Description, CreatedById) VALUES (1, 'International Passport', 'This is a document issued to citizens of for international travel.', 1)"); 
            // migrationBuilder.Sql("INSERT INTO MeansOfIdentification (Id, Caption, Description, CreatedById) VALUES (2, 'Driver\'s License', 'This is a document permitting a person or persons to drive a vehicle within a defined geographical location', 1)"); 
            // migrationBuilder.Sql("INSERT INTO MeansOfIdentification (Id, Caption, Description, CreatedById) VALUES (3, 'Voter\'s Card', 'This is a document issued to citizens that have come of voting age as defined by the National constitution', 1)"); 
            // migrationBuilder.Sql("INSERT INTO MeansOfIdentification (Id, Caption, Description, CreatedById) VALUES (4, 'National Identity Card', 'This is a document issued to bona fide citizens of the country', 1)"); 
            // migrationBuilder.Sql("INSERT INTO MeansOfIdentification (Id, Caption, Description, CreatedById) VALUES (5, 'Tax Card', 'This is a document issued to bona fide taxpayers in a particular jurisdiction', 1)"); 
            // migrationBuilder.Sql("SET IDENTITY_INSERT MeansOfIdentification OFF"); 

            migrationBuilder.Sql("SET IDENTITY_INSERT MeansOfIdentification ON"); 
            migrationBuilder.Sql(@"IF NOT EXISTS(SELECT 1 FROM dbo.MeansOfIdentification WITH(NOLOCK))
                                    BEGIN 
                                        INSERT INTO MeansOfIdentification (Id, Caption, Description, CreatedById, IsDeleted) VALUES (1, 'International Passport', 'This is a document issued to citizens of for international travel.', 1, 0), 
                                        (3, 'Voter''s Card', 'This is a document issued to citizens that have come of voting age as defined by the National constitution', 1, 0), 
                                        (4, 'National Identity Card', 'This is a document issued to bona fide citizens of the country', 1, 0), 
                                        (2, 'Driver''s License', 'This is a document permitting a person or persons to drive a vehicle within a defined geographical location', 1, 0), 
                                        (5, 'Tax Card', 'This is a document issued to bona fide taxpayers in a particular jurisdiction', 1, 0)
                                    END
            ");
            migrationBuilder.Sql("SET IDENTITY_INSERT MeansOfIdentification OFF");


            //Relationship
            // migrationBuilder.Sql("TRUNCATE TABLE Relationships");
            // migrationBuilder.Sql("SET IDENTITY_INSERT Relationships ON"); 
            // migrationBuilder.Sql("INSERT INTO Relationships (Id, Caption, Description, CreatedById) VALUES (1, 'Father', 'One\'s biological male parent', 1)"); 
            // migrationBuilder.Sql("INSERT INTO Relationships (Id, Caption, Description, CreatedById) VALUES (2, 'Mother', 'One\'s biological female parent', 1)"); 
            // migrationBuilder.Sql("INSERT INTO Relationships (Id, Caption, Description, CreatedById) VALUES (3, 'Uncle', 'Brother to your biological male or female parent', 1)"); 
            // migrationBuilder.Sql("INSERT INTO Relationships (Id, Caption, Description, CreatedById) VALUES (4, 'Aunty', 'Sister to your biological male or female parent', 1)"); 
            // migrationBuilder.Sql("INSERT INTO Relationships (Id, Caption, Description, CreatedById) VALUES (5, 'Cousin', 'Son or Daughter of an Uncle or Aunty', 1)"); 
            // migrationBuilder.Sql("INSERT INTO Relationships (Id, Caption, Description, CreatedById) VALUES (6, 'Brother', 'One\'s male sibling from the same parents', 1)"); 
            // migrationBuilder.Sql("INSERT INTO Relationships (Id, Caption, Description, CreatedById) VALUES (7, 'Sister', 'One\'s female sibling from the same parents', 1)"); 
            // migrationBuilder.Sql("INSERT INTO Relationships (Id, Caption, Description, CreatedById) VALUES (8, 'Half-Brother', 'One\'s male sibling from at least one of your parents', 1)"); 
            // migrationBuilder.Sql("INSERT INTO Relationships (Id, Caption, Description, CreatedById) VALUES (9, 'Half-Sister', 'One\'s female sibling from at least one of your parents', 1)"); 
            // migrationBuilder.Sql("INSERT INTO Relationships (Id, Caption, Description, CreatedById) VALUES (10, 'Nephew', 'Son of one\'s brother or sister, or of one\'s brother-in-law or sister-in-law', 1)"); 
            // migrationBuilder.Sql("INSERT INTO Relationships (Id, Caption, Description, CreatedById) VALUES (11, 'Neice', 'Daughter of one\'s brother or sister, or of one\'s brother-in-law or sister-in-law', 1)"); 
            // migrationBuilder.Sql("INSERT INTO Relationships (Id, Caption, Description, CreatedById) VALUES (12, 'Grand Father', 'Male parent to any of one\'s biological parents', 1)"); 
            // migrationBuilder.Sql("INSERT INTO Relationships (Id, Caption, Description, CreatedById) VALUES (13, 'Grand Mother', 'Female parent to any of one\'s biological parents, 1)"); 
            // migrationBuilder.Sql("SET IDENTITY_INSERT Relationships OFF"); 

            migrationBuilder.Sql("SET IDENTITY_INSERT Relationships ON"); 
            migrationBuilder.Sql(@"IF NOT EXISTS(SELECT 1 FROM dbo.Relationships WITH(NOLOCK))
                                    BEGIN 
                                        INSERT INTO Relationships (Id, Caption, Description, CreatedById, IsDeleted) VALUES (1, 'Father', 'One''s biological male parent', 1, 0),
                                        (2, 'Mother', 'One''s biological female parent', 1, 0),
                                        (3, 'Uncle', 'Brother to your biological male or female parent', 1, 0),
                                        (4, 'Aunty', 'Sister to your biological male or female parent', 1, 0),
                                        (5, 'Cousin', 'Son or Daughter of an Uncle or Aunty', 1, 0),
                                        (6, 'Brother', 'One''s male sibling from the same parents', 1, 0),
                                        (7, 'Sister', 'One''s female sibling from the same parents', 1, 0),
                                        (8, 'Half-Brother', 'One''s male sibling from at least one of your parents', 1, 0),
                                        (9, 'Half-Sister', 'One''s female sibling from at least one of your parents', 1, 0),
                                        (10, 'Nephew', 'Son of one''s brother or sister, or of one''s brother-in-law or sister-in-law', 1, 0),
                                        (11, 'Neice', 'Daughter of one''s brother or sister, or of one''s brother-in-law or sister-in-law', 1, 0),
                                        (12, 'Grand Father', 'Male parent to any of one''s biological parents', 1, 0),
                                        (13, 'Grand Mother', 'Female parent to any of one''s biological parents', 1, 0)
                                    END
            ");
            migrationBuilder.Sql("SET IDENTITY_INSERT Relationships OFF");


            //Targets
            // migrationBuilder.Sql("TRUNCATE TABLE Targets");
            // migrationBuilder.Sql("SET IDENTITY_INSERT Targets ON"); 
            // migrationBuilder.Sql("INSERT INTO Targets (Id, Caption, Description, CreatedById) VALUES (1, 'Retail Only', 'Retail Only', 1)"); 
            // migrationBuilder.Sql("INSERT INTO Targets (Id, Caption, Description, CreatedById) VALUES (2, 'Corporate and Government', 'Corporate and Government', 1)"); 
            // migrationBuilder.Sql("INSERT INTO Targets (Id, Caption, Description, CreatedById) VALUES (3, 'Corporate Only', 'Corporate Only', 1)"); 
            // migrationBuilder.Sql("INSERT INTO Targets (Id, Caption, Description, CreatedById) VALUES (4, 'Government Only', 'Government Only', 1)"); 
            // migrationBuilder.Sql("INSERT INTO Targets (Id, Caption, Description, CreatedById) VALUES (5, 'Corporate and Retail', 'Corporate and Retail', 1)"); 
            // migrationBuilder.Sql("INSERT INTO Targets (Id, Caption, Description, CreatedById) VALUES (6, 'Government and Retail', 'Government and Retail', 1)"); 
            // migrationBuilder.Sql("INSERT INTO Targets (Id, Caption, Description, CreatedById) VALUES (7, 'All', 'All Group Types', 1)"); 
            // migrationBuilder.Sql("SET IDENTITY_INSERT Targets OFF"); 


            migrationBuilder.Sql("SET IDENTITY_INSERT Targets ON"); 
            migrationBuilder.Sql(@"IF NOT EXISTS(SELECT 1 FROM dbo.Targets WITH(NOLOCK))
                                    BEGIN 
                                        INSERT INTO Targets (Id, Caption, Description, CreatedById, IsDeleted) VALUES (1, 'Retail Only', 'Retail Only', 1, 0),
                                        (2, 'Corporate and Government', 'Corporate and Government', 1, 0),
                                        (3, 'Corporate Only', 'Corporate Only', 1, 0),
                                        (4, 'Government Only', 'Government Only', 1, 0),
                                        (5, 'Corporate and Retail', 'Corporate and Retail', 1, 0),
                                        (6, 'Government and Retail', 'Government and Retail', 1, 0),
                                        (7, 'All', 'All Group Types', 1, 0)
                                    END
            ");
            migrationBuilder.Sql("SET IDENTITY_INSERT Targets OFF");



            //Service Type Maintainance
            // migrationBuilder.Sql("TRUNCATE TABLE ServiceTypes");
            // migrationBuilder.Sql("SET IDENTITY_INSERT ServiceTypes ON"); 
            // migrationBuilder.Sql("INSERT INTO ServiceTypes (Id, Caption, Description, CreatedById) VALUES (1, 'Trade', 'Trade', 1)"); 
            // migrationBuilder.Sql("INSERT INTO ServiceTypes (Id, Caption, Description, CreatedById) VALUES (2, 'Trade and Maintainance', 'Trade and Maintainance', 1)"); 
            // migrationBuilder.Sql("INSERT INTO ServiceTypes (Id, Caption, Description, CreatedById) VALUES (3, 'Maintainance', 'Maintainance', 1)"); 
            // migrationBuilder.Sql("SET IDENTITY_INSERT ServiceTypes OFF"); 


            migrationBuilder.Sql("SET IDENTITY_INSERT ServiceTypes ON"); 
            migrationBuilder.Sql(@"IF NOT EXISTS(SELECT 1 FROM dbo.ServiceTypes WITH(NOLOCK))
                                    BEGIN 
                                        INSERT INTO ServiceTypes (Id, Caption, Description, CreatedById, IsDeleted) VALUES (1, 'Trade', 'Trade', 1, 0),
                                        (2, 'Trade and Maintainance', 'Trade and Maintainance', 1, 0),
                                        (3, 'Maintainance', 'Maintainance', 1, 0)
                                    END
            ");
            migrationBuilder.Sql("SET IDENTITY_INSERT ServiceTypes OFF");



       
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
