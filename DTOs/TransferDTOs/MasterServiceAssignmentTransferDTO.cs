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
        public SMORoute SMORoute { get; set; }
        public long? SMORouteId { get; set; }
        public SMORegion SMORegion { get; set; }
        public long? SMORegionId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ReadinessTime { get; set; }
        public string DropoffLocation { get; set; }
        public string PickoffLocation { get; set; }
        public DateTime DropoffDate { get; set; }
        public DateTime PickupDate { get; set; }
        public ContractService ContractService { get; set; }
      
        public long? ContractServiceId { get; set; }
       
        public ServiceRegistration ServiceRegistration { get; set; }
        public long? ServiceRegistrationId { get; set; }
        public CustomerDivision CustomerDivision { get; set; }
        public long? CustomerDivisionId { get; set; }
        public SourceType SourceType { get; set; }
        public long? SourceTypeId { get; set; }
        public UserProfile CreatedBy { get; set; }
        public long CreatedById { get; set; }
        public bool IsDeleted { get; set; }

        public DateTime PickoffTime { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
