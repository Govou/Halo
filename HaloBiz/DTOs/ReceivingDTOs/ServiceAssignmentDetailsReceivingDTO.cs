using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class ServiceAssignmentDetailsReceivingDTO
    {
    }
    public class ArmedEscortServiceAssignmentDetailsReceivingDTO
    {
        //public long SMORouteId { get; set; }
        //public long SMORegionId { get; set; }
        public long ArmedEscortResourceId { get; set; }
        public long ServiceAssignmentId { get; set; }

        //public bool IsTemporarilyHeld { get; set; }
        //public bool IsHeldForAction { get; set; }

    }
    public class CommanderServiceAssignmentDetailsReceivingDTO
    {
        public long ServiceAssignmentId { get; set; }
        public long CommanderResourceId { get; set; }
        public long TiedVehicleResourceId { get; set; }
        //public long SMORouteId { get; set; }
        //public long SMORegionId { get; set; }
       
    }
    public class PilotServiceAssignmentDetailsReceivingDTO
    {
        public long ServiceAssignmentId { get; set; }
        public long PilotResourceId { get; set; }
        public long TiedVehicleResourceId { get; set; }
        //public long SMORouteId { get; set; }
        //public long SMORegionId { get; set; }
       
    }
    public class VehicleServiceAssignmentDetailsReceivingDTO
    {
        public long ServiceAssignmentId { get; set; }
        public long VehicleResourceId { get; set; }
        //public long SMORouteId { get; set; }
        //public long SMORegionId { get; set; }
      
    } 
    public class PassengerReceivingDTO
    {
       
        public long? PassengerTypeId { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string InstagramHandle { get; set; }
        public string TwitterHandle { get; set; }
        public string Othername { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public long? ServiceAssignmentId { get; set; }
        public string FacebooHandle { get; set; }
    }

    public class ArmedEscortReplacementReceivingDTO
    {
        public long MasterServiceAssignmentId { get; set; }

        public long OldResourceId { get; set; }

        public long NewResourceId { get; set; }

        public string ReasonForReplacement { get; set; }
    }

    public class CommanderReplacementReceivingDTO
    {
        public long MasterServiceAssignmentId { get; set; }

        public long OldResourceId { get; set; }

        public long NewResourceId { get; set; }
        public string ReasonForReplacement { get; set; }
    }

    public class PilotReplacementReceivingDTO
    {
        public long MasterServiceAssignmentId { get; set; }
       
        public long OldResourceId { get; set; }

        public long NewResourceId { get; set; }

        public string ReasonForReplacement { get; set; }
    }

    public class VehicleReplacementReceivingDTO
    {
        public long MasterServiceAssignmentId { get; set; }

        public long OldResourceId { get; set; }

        public long NewResourceId { get; set; }
    
        public string ReasonForReplacement { get; set; }
    }
}
