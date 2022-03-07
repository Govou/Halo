using HalobizMigrations.Models;
using HalobizMigrations.Models.Armada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class SMORouteMapTransferDTO
    {
        
        public long Id { get; set; }
        public long? StateId { get; set; }
       
        public State State { get; set; }
        public long? SMORegionId { get; set; }
       
        public SMORegion SMORegion { get; set; }
        public long? SMORouteId { get; set; }
       
        public SMORoute SMORoute { get; set; }
        public string Comment { get; set; }
        public long CreatedById { get; set; }
      
        public UserProfile CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
