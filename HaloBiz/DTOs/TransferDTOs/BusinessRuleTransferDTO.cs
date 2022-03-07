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
        public ServiceRegistration ServiceRegistration { get; set; }
        public bool IsQuantityRequired { get; set; }
        public long MaxQuantity { get; set; }
        public bool IsPairingRequired { get; set; }
        public long CreatedById { get; set; }
        public UserProfile CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
