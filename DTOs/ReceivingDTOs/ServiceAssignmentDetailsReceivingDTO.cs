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
      
        public long ArmedEscortResourceId { get; set; }
      
    }
    public class CommanderServiceAssignmentDetailsReceivingDTO
    {
        public long CommanderResourceId { get; set; }
        public long TiedVehicleResourceId { get; set; }
    }
    public class PilotServiceAssignmentDetailsReceivingDTO
    {
        public long PilotResourceId { get; set; }
        public long TiedVehicleResourceId { get; set; }
    }
    public class VehicleServiceAssignmentDetailsReceivingDTO
    {
        public long VehicleResourceId { get; set; }
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
}
