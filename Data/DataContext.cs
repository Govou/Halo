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
    public DbSet<RequiredServiceField> RequiredServiceFields{get; set;}
    public DbSet<ServiceRequiredServiceField> ServiceRequiredServiceField { get; set; }
    public DbSet<RequiredServiceDocument> RequiredServiceDocuments { get; set; }
    public DbSet<ServiceRequiredServiceDocument> ServiceRequiredServiceDocument { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
        {
            //Defines many to many relationship between SBU and AccountMaster
            builder.Entity<SBUAccountMaster>()
                .HasKey(sc => new { sc.AccountMasterId, sc.StrategicBusinessUnitId });

            builder.Entity<ServiceRequiredServiceField>()
                .HasKey(sc => new {sc.RequiredServiceFieldId, sc.ServicesId});

            builder.Entity<ServiceRequiredServiceDocument>()
                .HasKey(sc => new {sc.RequiredServiceDocumentId, sc.ServicesId});

            builder.Entity<State>()
               .Property(p => p.UpdatedAt)
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

            builder.Entity<RequiredServiceField>()
               .Property(p => p.UpdatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<RequiredServiceField>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

        }
    }
}