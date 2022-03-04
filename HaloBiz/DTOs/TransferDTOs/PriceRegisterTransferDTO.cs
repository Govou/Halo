using HalobizMigrations.Models;
using HalobizMigrations.Models.Armada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class PriceRegisterTransferDTO
    {
        public DateTime CreatedAt { get; set; }
      
        public UserProfile CreatedBy { get; set; }
        public long CreatedById { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal MarkupPrice { get; set; }
        public double MarkupPercentage { get; set; }
        public DateTime UpdatedAt { get; set; }
        public decimal CostPrice { get; set; }
        public long? ServiceRegistrationId { get; set; }
  
        public SMORoute SMORoute { get; set; }
        public long? SMORouteId { get; set; }
        
        public SMORegion SMORegion { get; set; }
        public long? SMORegionId { get; set; }
   
        public long Id { get; set; }
      
        public ServiceRegistration ServiceRegistration { get; set; }
        public bool IsDeleted { get; set; }
    }
}
