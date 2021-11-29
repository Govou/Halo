using HalobizMigrations.Models;
using HalobizMigrations.Models.Armada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class BRPairableTransferDTO
    {
        
        public long Id { get; set; }
        public long? BusinessRuleId { get; set; }
        public BusinessRule BusinessRule { get; set; }
        public long? ServiceRegistrationId { get; set; }
        public ServiceRegistration ServiceRegistration { get; set; }
        public long CreatedById { get; set; }
        public UserProfile CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
