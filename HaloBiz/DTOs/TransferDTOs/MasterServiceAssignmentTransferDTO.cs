using HalobizMigrations.Models;
using HalobizMigrations.Models.Armada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class MasterServiceAssignmentTransferDTO
    {
        public long Id { get; set; }
      
        public bool IsReturnJourney { get; set; }
        public long? PrimaryTripAssignmentId { get; set; }
        public TripType TripType { get; set; }
        public long? TripTypeId { get; set; }
        //public SMORoute SMORoute { get; set; }
        public SMORoutesForServiceAssignmentTransferDTO SMORoute { get; set; }
        public long? SMORouteId { get; set; }
        //public SMORegion SMORegion { get; set; }
        public SMORegionsForServiceAssignmentTransferDTO SMORegion { get; set; }
        public long? SMORegionId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ReadinessTime { get; set; }
        public string DropoffLocation { get; set; }
        public string PickoffLocation { get; set; }
        public DateTime DropoffDate { get; set; }
        public DateTime PickupDate { get; set; }
        public ContractService ContractService { get; set; }
        public int SAExecutionStatus { get; set; }
        public long? ContractServiceId { get; set; }
        public double PickupLocationLongitude { get; set; }
        public double PickupLocationLatitude { get; set; }
        public double DropoffLocationLongitude { get; set; }
        public double DropoffLocationLatitude { get; set; }
        public double DistanceInKM { get; set; }
        //public ServiceRegistration ServiceRegistration { get; set; }
        public serviceRegistrationForServiceAssignmentTransferDTO ServiceRegistration { get; set; }
        public long? ServiceRegistrationId { get; set; }
        public CustomerDivision CustomerDivision { get; set; }
        public long? CustomerDivisionId { get; set; }
        public SourceType SourceType { get; set; }
        public long? SourceTypeId { get; set; }
        public UserProfile CreatedBy { get; set; }
        public long CreatedById { get; set; }
        public bool IsDeleted { get; set; }
        public string AssignmentStatus { get; set; }
        public bool ReadyStatus { get; set; }
        public DateTime PickoffTime { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool HasPassenger { get; set; }

        public ICollection<SecondaryServiceAssignment> SecondaryServiceAssignments { get; set; }
    }

    public class SecondaryServiceAssignmentTransferDTO
    {
        public long Id { get; set; }
        public long? ServiceAssignmentId { get; set; }
        public MasterServiceAssignment ServiceAssignment { get; set; }
        public long? SecondaryServiceRegistrationId { get; set; }
        public ServiceRegistration SecondaryServiceRegistration { get; set; }
        public long? SecondaryContractServiceId { get; set; }
        public ContractService SecondaryContractService { get; set; }
        public bool IsDeleted { get; set; }
        public long CreatedById { get; set; }
        public UserProfile CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class ServiceTransferForOnlineDTO
    {
     
        public long Id { get; set; }
     
        public string Name { get; set; }
        public string Alias { get; set; }
      
        public string Description { get; set; }
        public double UnitPrice { get; set; }
        public long ServiceCategoryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long DivisionId { get; set; }
        public long OperatingEntityId { get; set; }
        public long ServiceGroupId { get; set; }
        public string ImageUrl { get; set; }
    }

    public class serviceRegistrationForServiceAssignmentTransferDTO
    {
       
        public long CreatedById { get; set; }
        //public ICollection<ArmedEscortType> ApplicableArmedEscortTypes { get; set; }
        //public ICollection<VehicleType> ApplicableVehicleTypes { get; set; }
        //public ICollection<PilotType> ApplicablePilotTypes { get; set; }
        //public ICollection<CommanderType> ApplicableCommanderTypes { get; set; }
        public string Description { get; set; }
        public long ArmedEscortQuantityRequired { get; set; }
        public bool RequiresArmedEscort { get; set; }
        public long PilotQuantityRequired { get; set; }
        public bool RequiresPilot { get; set; }
        public long VehicleQuantityRequired { get; set; }
        public bool RequiresVehicle { get; set; }
        public long CommanderQuantityRequired { get; set; }
        public bool RequiresCommander { get; set; }
        public Service Service { get; set; }
        public long? ServiceId { get; set; }
        public long Id { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class serviceForServiceAssignmentTransferDTO
    {
       
        public long? DirectServiceId { get; set; }
        public long CreatedById { get; set; }
        //public virtual ControlAccount ControlAccount { get; set; }
        
        public virtual Account Account { get; set; }
       
        //public virtual UserProfile CreatedBy { get; set; }
      
        public virtual Division Division { get; set; }
       
        //public virtual OperatingEntity OperatingEntity { get; set; }
       
        public virtual ServiceCategory ServiceCategory { get; set; }
        public long? ControlAccountId { get; set; }
      
        public virtual ServiceGroup ServiceGroup { get; set; }
       
        //public virtual Target Target { get; set; }
       
        //public virtual ICollection<Approval> Approvals { get; set; }
       
        //public virtual ICollection<ContractServiceForEndorsement> ContractServiceForEndorsements { get; set; }
        
        //public virtual ICollection<ContractService> ContractServices { get; set; }
       
        //public virtual ICollection<QuoteService> QuoteServices { get; set; }
       
        //public virtual ICollection<ServiceRequiredServiceDocument> ServiceRequiredServiceDocuments { get; set; }
       
        //public virtual ICollection<ServiceRequredServiceQualificationElement> ServiceRequredServiceQualificationElements { get; set; }
       
        public virtual ServiceType ServiceType { get; set; }
        public long? AccountId { get; set; }
        public long? ServiceTypeId { get; set; }
        public long? TargetId { get; set; }
       
        public long Id { get; set; }
       
        public string Name { get; set; }
        public string Alias { get; set; }
     
        public string Description { get; set; }
        public double UnitPrice { get; set; }
        public long ServiceCategoryId { get; set; }
       
        public long DivisionId { get; set; }
        public long OperatingEntityId { get; set; }
        public long ServiceGroupId { get; set; }
        public string ImageUrl { get; set; }
       
        public bool? IsDeleted { get; set; }
       
        public bool? IsPublished { get; set; }
       
        public bool? IsRequestedForPublish { get; set; }
       
        public bool? PublishedApprovedStatus { get; set; }
      
        public bool? IsVatable { get; set; }
        public bool? CanBeSoldOnline { get; set; }
       
        public string ServiceCode { get; set; }
        
        //public virtual ServiceRelationship AdminRelationship { get; set; }
        
        //public virtual ServiceRelationship DirectRelationship { get; set; }
    }


    public class MasterServiceAssignmentWithRegisterTransferDTO
    {
        public long Id { get; set; }

        public bool ReadyStatus { get; set; }
        public bool IsScheduled { get; set; }
        public bool InhouseAssignment { get; set; }
        public bool IsPaidFor { get; set; }
        public string AssignmentStatus { get; set; }
      
        public long? SMORouteId { get; set; }
        public string RouteName { get; set; }
        public string PickoffLocation { get; set; }
        public string DropoffLocation { get; set; }
        public long? ContractServiceId { get; set; }
        public ServiceRegistration ServiceRegistration { get; set; }
        public ServiceRegistration PriceServiceRegistration { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal CostPrice { get; set; }
        public decimal MarkupPrice { get; set; }
        public ServiceCategory ServiceCategory { get; set; }
        public Service Service { get; set; }
        //public ServiceTransferForOnlineDTO Service { get; set; }
        public double PickupLocationLongitude { get; set; }
        public double PickupLocationLatitude { get; set; }
        public double DropoffLocationLongitude { get; set; }
        public double DropoffLocationLatitude { get; set; }
        public DateTime DropoffDate { get; set; }
        public DateTime PickupDate { get; set; }
        public DateTime PickoffTime { get; set; }
        public bool HasPassenger { get; set; }

    }

    public class SMORegionsForServiceAssignmentTransferDTO{
          public long Id { get; set; }
    public string RegionName { get; set; }
    public string RegionDescription { get; set; }
    public long CreatedById { get; set; }
    //public UserProfile CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    //public List<SMORoute> SMORoutes { get; set; }
}

    public class SMORoutesForServiceAssignmentTransferDTO
    {
     
        public long Id { get; set; }
        public string RouteName { get; set; }
        public string RouteDescription { get; set; }
        public long? SMORegionId { get; set; }
        public SMORegion SMORegion { get; set; }
        public int RRecoveryTime { get; set; }
        public bool IsReturnRouteRequired { get; set; }
        
        public long CreatedById { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
