using HalobizMigrations.Models;
using HalobizMigrations.Models.Armada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class BusinessRuleTransferDTO
    {
        
        public long Id { get; set; }
        public long? ServiceRegistrationId { get; set; }
        //public ServiceRegistration ServiceRegistration { get; set; }
        public BusinessRuleServiceRegTransferDTO ServiceRegistration { get; set; }
        public bool IsQuantityRequired { get; set; }
        public long MaxQuantity { get; set; }
        public bool IsPairingRequired { get; set; }
        public long CreatedById { get; set; }
        public UserProfile CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }


    public class BusinessRuleServiceRegTransferDTO
    {
    
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
      
    }
}
