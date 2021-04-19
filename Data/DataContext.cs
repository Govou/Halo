using HaloBiz.Model;
using HaloBiz.Model.AccountsModel;
using HaloBiz.Model.LAMS;
using HaloBiz.Model.ManyToManyRelationship;
using HaloBiz.Model.RoleManagement;
using halobiz_backend.Model;
using halobiz_backend.Model.AccountsModel;
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
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleClaim> RoleClaims { get; set; }
        public DbSet<OtherLeadCaptureInfo> OtherLeadCaptureInfos { get; set; }
        public DbSet<TaskFulfillment> TaskFulfillments { get; set; }
        public DbSet<DeliverableFulfillment> DeliverableFulfillments { get; set; }
        public DbSet<Industry> Industries { get; set; }
        public DbSet<Designation> Designations { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<TaskEscalationTiming> TaskEscalationTiming { get; set; }
        public DbSet<Claim> Claims { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<ApproverLevel> ApproverLevels { get; set; }
        public DbSet<ApprovalLimit> ApprovalLimits { get; set; }
        public DbSet<ProcessesRequiringApproval> ProcessesRequiringApprovals { get; set; }
        public DbSet<Approval> Approvals { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<EndorsementType> EndorsementTypes { get; set; }
        public DbSet<ClientBeneficiary> ClientBeneficiaries { get; set; }
        public DbSet<GroupInvoiceDetails> GroupInvoiceDetails { get; set; }
        public DbSet<GroupInvoiceTracker> GroupInvoiceTracker { get; set; }
        public DbSet<SBUProportion> SBUProportions { get; set; }
        public DbSet<ModeOfTransport> ModeOfTransports { get; set; }
        public DbSet<SupplierCategory> SupplierCategories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<SupplierService> SupplierServices { get; set; }
        public DbSet<EndorsementTypeTracker> EndorsementTypeTrackers { get; set; }
        public DbSet<ContractServiceForEndorsement> ContractServiceForEndorsements{ get; set; }
        public DbSet<EngagementType> EngagementTypes { get; set; }
        public DbSet<LeadEngagement> LeadEngagements { get; set; }
        public DbSet<ClientEngagement> ClientEngagements{ get; set; }
        public DbSet<ClientContactQualification> ClientContactQualifications{ get; set; }
        public DbSet<LeadDivisionContactLeadEngagement> LeadDivisionContactLeadEngagements { get; set; }
        public DbSet<LeadDivisionKeyPersonLeadEngagement> LeadDivisionKeyPersonLeadEngagements { get; set; }
        public DbSet<LeadEngagementUserProfile> LeadEngagementUserProfiles { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Defines many to many relationship between SBU and AccountMaster
            builder.Entity<SBUAccountMaster>()
                .HasKey(sc => new { sc.AccountMasterId, sc.StrategicBusinessUnitId });

            builder.Entity<ServiceRequiredServiceDocument>()
                .HasKey(sc => new {sc.RequiredServiceDocumentId, sc.ServicesId});

            builder.Entity<ServiceRequredServiceQualificationElement>()
                .HasKey(sc => new {sc.RequredServiceQualificationElementId, sc.ServicesId});

            builder.Entity<LeadDivisionContactLeadEngagement>()
                .HasKey(sc => new { sc.ContactsEngagedWithId, sc.LeadEngagementsId });

            builder.Entity<LeadDivisionKeyPersonLeadEngagement>()
                .HasKey(sc => new { sc.KeyPersonsEngagedWithId, sc.LeadEngagementsId });

            builder.Entity<LeadEngagementUserProfile>()
                .HasKey(sc => new { sc.UserLeadEngagementsId, sc.UsersEngagedWithId });
        }
    }
}