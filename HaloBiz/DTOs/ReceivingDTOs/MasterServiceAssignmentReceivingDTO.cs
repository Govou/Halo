using HalobizMigrations.Models.Armada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class MasterServiceAssignmentReceivingDTO
    {


        public long ContractServiceId { get; set; }
        public long? CustomerDivisionId { get; set; }
        public long ServiceRegistrationId { get; set; }
        public long SMORouteId { get; set; }
        public long? SMORegionId { get; set; }
        //public DateTime ReadinessTime { get; set; }
        public string DropoffLocation { get; set; }
        public string PickoffLocation { get; set; }
        //public DateTime DropoffDate { get; set; }
        public DateTime PickupDate { get; set; }
        public bool IsReturnJourney { get; set; }
       
        //public long? SourceTypeId { get; set; }
        public DateTime PickoffTime { get; set; }
        public double PickupLocationLongitude { get; set; }
        public double PickupLocationLatitude { get; set; }
        public double DropoffLocationLongitude { get; set; }
        public double DropoffLocationLatitude { get; set; }
        public string PickUpLocationGeometry { get; set; }
        public string DropOffLocationGeometry { get; set; }
        public double DistanceInKM { get; set; }
        public long?[] SecondaryServiceRegistrationId { get; set; }
        //public SecondaryServiceAssignmentReceivingDTO SecondaryServiceAssignmentReceivingDTO { get; set; }
    }

    public class MasterServiceAssignmentForAutoReceivingDTO
    {
        public long ContractServiceId { get; set; }
        public long? CustomerDivisionId { get; set; }
        public long ServiceRegistrationId { get; set; }
        public long SMORouteId { get; set; }
        public long? SMORegionId { get; set; }
        //public DateTime ReadinessTime { get; set; }
        public string DropoffLocation { get; set; }
        public string PickoffLocation { get; set; }
        //public DateTime DropoffDate { get; set; }
        public DateTime PickupDate { get; set; }
        public bool IsReturnJourney { get; set; }

        //public long? SourceTypeId { get; set; }
        public DateTime PickoffTime { get; set; }
        public double PickupLocationLongitude { get; set; }
        public double PickupLocationLatitude { get; set; }
        public double DropoffLocationLongitude { get; set; }
        public double DropoffLocationLatitude { get; set; }
        public string PickUpLocationGeometry { get; set; }
        public string DropOffLocationGeometry { get; set; }
        public double DistanceInKM { get; set; }
        public long?[] SecondaryServiceRegistrationId { get; set; }
      
    }

    public class SecondaryServiceAssignmentReceivingDTO
    {
        public long? ServiceAssignmentId { get; set; }
        public long?[] SecondaryServiceRegistrationId { get; set; }
        public long? SecondaryContractServiceId { get; set; }
    }
    public class MasterServiceAssignmentMailVMDTO
    {


        public long id { get; set; }
        public long? ServiceRegistrationId { get; set; }

        //public ServiceRegistration ServiceRegistration { get; set; }
        public string DropoffLocation { get; set; }
        public string PickoffLocation { get; set; }
        public string[] Recepients { get; set; }
        public DateTime PickupDate { get; set; }
        public string Subject { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByMobile { get; set; }
        public string CreatedByPic { get; set; }
        public DateTime PickoffTime { get; set; }
        public ServiceMailDTO ServiceMailDTO { get; set; }

        public IEnumerable<CommandersMailDTO> Commanders { get; set; }
        public IEnumerable<ArmedEscortsMailDTO> armedEscorts { get; set; }
        public IEnumerable<PilotsMailDTO> pilots { get; set; }
        public IEnumerable<VehiclesMailDTO> vehicles { get; set; }
        public IEnumerable<PassengersMailDTO> passengers { get; set; }

    }

    public class CommandersMailDTO
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Mobile { get; set; }
        public string ImageUrl { get; set; }
        public long? ResourceId { get; set; }
    }
    public class ArmedEscortsMailDTO
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Mobile { get; set; }
        public string ImageUrl { get; set; }
        public long ForceId { get; set; }
    }
    public class PilotsMailDTO
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Mobile { get; set; }
        public string ImageUrl { get; set; }
        public long? ResourceId { get; set; }
        
    }
    public class VehiclesMailDTO
    {
        public string serviceName { get; set; }
        public string IdentificationNumber { get; set; }
        
        //public string Mobile { get; set; }
        //public string ImageUrl { get; set; }
    }
    public class PassengersMailDTO
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string PassengerType { get; set; }
    }
    public class ServiceMailDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
