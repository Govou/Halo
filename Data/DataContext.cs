using HaloBiz.Model;
using HaloBiz.Model.AccountsModel;
using HaloBiz.Model.LAMS;
using HaloBiz.Model.ManyToManyRelationship;
using Microsoft.EntityFrameworkCore;

namespace HaloBiz.Data
{
    public class DataContext : DbContext
    {

    public DataContext(DbContextOptions<DataContext> options) : base(options) {}
    public DbSet<State> States { get; set; }
    public DbSet<LGA> LGAs { get; set; }
    public DbSet<Office> Offices { get; set; }
    public DbSet<Branch> Branches { get; set; }
    public DbSet<Division> Divisions { get; set; }
    public DbSet<OperatingEntity> OperatingEntities { get; set; }
    public DbSet<ServiceCategory> ServiceCategories { get; set; }
    public DbSet<ServiceCategoryTask> ServiceCategoryTasks { get; set; }
    public DbSet<ServiceGroup> ServiceGroups { get; set; }
    public DbSet<Services> Services { get; set; }
    public DbSet<StrategicBusinessUnit> StrategicBusinessUnits { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<ModificationHistory> ModificationHistories { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<AccountClass> AccountClasses { get; set; }
    public DbSet<AccountDetail> AccountDetails { get; set; }
    public DbSet<AccountMaster> AccountMasters { get; set; }
    public DbSet<SBUAccountMaster> SBUAccountMasters { get; set; }
    public DbSet<SBUAccountMaster> SBUAccountMaster { get; set; }
    public DbSet<LeadOrigin> LeadOrigins { get; set; }
    public DbSet<LeadType> LeadTypes { get; set; }
    public DbSet<FinanceVoucherType> FinanceVoucherTypes { get; set; }
    public DbSet<RequredServiceQualificationElement> RequredServiceQualificationElements{get; set;}
    public DbSet<RequiredServiceDocument> RequiredServiceDocuments { get; set; }
    public DbSet<ServiceRequiredServiceDocument> ServiceRequiredServiceDocument { get; set; }
    public DbSet<StandardSLAForOperatingEntities> StandardSLAForOperatingEntities { get; set; }
    public DbSet<MeansOfIdentification> MeansOfIdentification { get; set; }
    public DbSet<GroupType> GroupType { get; set; }
    public DbSet<Relationship> Relationships { get; set; }
    public DbSet<Target> Targets { get; set; }
    public DbSet<Bank> Banks { get; set; }
    public DbSet<ServiceTaskDeliverable> ServiceTaskDeliverables { get; set; }
    public DbSet<ServiceType> ServiceTypes {get; set; }
    public DbSet<DropReason> DropReasons {get; set; }
    public DbSet<Customer> Customers {get; set; }
    public DbSet<LeadContact> LeadContacts {get; set; }
    public DbSet<LeadDivisionContact> LeadDivisionContacts {get; set; }
    public DbSet<Lead> Leads {get; set; }
    public DbSet<LeadDivision> LeadDivisions {get; set; }
    public DbSet<LeadDivisionKeyPerson> LeadDivisionKeyPeople {get; set; }
    public DbSet<LeadKeyPerson> LeadKeyPeople {get; set; }
    public DbSet<Quote> Quotes {get; set; }
    public DbSet<QuoteService> QuoteServices {get; set; }
    public DbSet<QuoteServiceDocument> QuoteServiceDocuments {get; set; }
    public DbSet<CustomerDivision> CustomerDivisions {get; set; }
    public DbSet<SBUToContractServiceProportion> SBUToContractServiceProportions {get; set; }
    public DbSet<SBUToQuoteServiceProportion> SBUToQuoteServiceProportions {get; set; }
    public DbSet<ClosureDocument> ClosureDocuments {get; set; }
    public DbSet<DeleteLog> DeleteLogs {get; set; }
    public DbSet<ServiceRequredServiceQualificationElement> ServiceRequredServiceQualificationElement {get; set; }
    public DbSet<ReferenceNumber> ReferenceNumbers {get; set; }
    public DbSet<Region> Regions {get; set; }
    public DbSet<ControlAccount> ControlAccounts {get; set; }
    public DbSet<Zone> Zones {get; set; }   
    public DbSet<Contract> Contracts {get; set; }   
    public DbSet<ContractService> ContractServices {get; set; }
    public DbSet<NegotiationDocument> NegotiationDocuments { get; set; }
    public DbSet<Amortization> Amortizations { get; set; }
    public DbSet<Invoice> Invoices { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Defines many to many relationship between SBU and AccountMaster
            builder.Entity<SBUAccountMaster>()
                .HasKey(sc => new { sc.AccountMasterId, sc.StrategicBusinessUnitId });

            builder.Entity<ServiceRequiredServiceDocument>()
                .HasKey(sc => new {sc.RequiredServiceDocumentId, sc.ServicesId});

            builder.Entity<ServiceRequredServiceQualificationElement>()
                .HasKey(sc => new {sc.RequredServiceQualificationElementId, sc.ServicesId});

            builder.Entity<State>()
               .Property(p => p.UpdatedAt   )
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<State>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<LGA>()
               .Property(p => p.UpdatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Entity<LGA>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<Office>()
               .Property(p => p.UpdatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Entity<Office>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<Branch>()
               .Property(p => p.UpdatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Entity<Branch>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<Division>()
               .Property(p => p.UpdatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Entity<Division>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<OperatingEntity>()
               .Property(p => p.UpdatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Entity<OperatingEntity>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<ServiceCategory>()
               .Property(p => p.UpdatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Entity<ServiceCategory>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
                
            builder.Entity<ServiceGroup>()
               .Property(p => p.UpdatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Entity<ServiceGroup>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<Services>()
               .Property(p => p.UpdatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Entity<Services>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<StrategicBusinessUnit>()
               .Property(p => p.UpdatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Entity<StrategicBusinessUnit>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<UserProfile>()
               .Property(p => p.UpdatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Entity<UserProfile>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<ModificationHistory>()
               .Property(p => p.UpdatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Entity<ModificationHistory>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<Account>()
               .Property(p => p.UpdatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<Account>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<AccountClass>()
               .Property(p => p.UpdatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<AccountClass>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<AccountMaster>()
               .Property(p => p.UpdatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<AccountMaster>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
                
            builder.Entity<AccountDetail>()
               .Property(p => p.UpdatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<AccountDetail>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
                
            builder.Entity<LeadOrigin>()
               .Property(p => p.UpdatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<LeadOrigin>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<LeadType>()
               .Property(p => p.UpdatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<LeadType>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<FinanceVoucherType>()
               .Property(p => p.UpdatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<FinanceVoucherType>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<ServiceCategoryTask>()
               .Property(p => p.UpdatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<ServiceCategoryTask>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<RequiredServiceDocument>()
               .Property(p => p.UpdatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<RequiredServiceDocument>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<RequredServiceQualificationElement>()
               .Property(p => p.UpdatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<RequredServiceQualificationElement>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<MeansOfIdentification>()
               .Property(p => p.UpdatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<MeansOfIdentification>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<GroupType>()
               .Property(p => p.UpdatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<GroupType>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<Target>()
               .Property(p => p.UpdatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<Target>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<Relationship>()
               .Property(p => p.UpdatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<Relationship>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<Bank>()
               .Property(p => p.UpdatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<Bank>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<StandardSLAForOperatingEntities>()
               .Property(p => p.UpdatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<StandardSLAForOperatingEntities>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<ServiceTaskDeliverable>()
               .Property(p => p.UpdatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<ServiceTaskDeliverable>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<ServiceType>()
               .Property(p => p.UpdatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<ServiceType>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<ServiceRequiredServiceDocument>()
               .Property(p => p.UpdatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<ServiceRequiredServiceDocument>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<Customer>()
               .Property(p => p.UpdatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<Customer>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<LeadContact>()
               .Property(p => p.UpdatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<LeadContact>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<LeadDivisionContact>()
               .Property(p => p.UpdatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<LeadDivisionContact>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<DropReason>()
               .Property(p => p.UpdatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<DropReason>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<Lead>()
               .Property(p => p.UpdatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<Lead>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<LeadDivision>()
               .Property(p => p.UpdatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<LeadDivision>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<LeadDivisionKeyPerson>()
               .Property(p => p.UpdatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<LeadDivisionKeyPerson>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<LeadKeyPerson>()
               .Property(p => p.UpdatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<LeadKeyPerson>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<QuoteService>()
               .Property(p => p.UpdatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<QuoteService>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<Quote>()
               .Property(p => p.UpdatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<Quote>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<QuoteServiceDocument>()
               .Property(p => p.UpdatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<QuoteServiceDocument>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<CustomerDivision>()
               .Property(p => p.UpdatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<CustomerDivision>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<SBUToContractServiceProportion>()
               .Property(p => p.UpdatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<SBUToContractServiceProportion>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<SBUToQuoteServiceProportion>()
               .Property(p => p.UpdatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<SBUToQuoteServiceProportion>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<ClosureDocument>()
               .Property(p => p.UpdatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<ClosureDocument>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<DeleteLog>()
               .Property(p => p.UpdatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<DeleteLog>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<ServiceRequredServiceQualificationElement>()
               .Property(p => p.UpdatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<ServiceRequredServiceQualificationElement>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<Zone>()
               .Property(p => p.UpdatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<Zone>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<Region>()
               .Property(p => p.UpdatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<Region>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<ControlAccount>()
               .Property(p => p.UpdatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<ControlAccount>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<Contract>()
               .Property(p => p.UpdatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<Contract>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<ContractService>()
               .Property(p => p.UpdatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<ContractService>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<Amortization>()
               .Property(p => p.UpdatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<Amortization>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<Invoice>()
               .Property(p => p.UpdatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<Invoice>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

        }
    }
}
