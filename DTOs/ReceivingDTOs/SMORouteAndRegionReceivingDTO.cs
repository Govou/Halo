using HalobizMigrations.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class SMORouteAndRegionReceivingDTO
    {
        
    }

    public class SMORouteReceivingDTO
    {
        [Required]
        public string RouteName { get; set; }
        [Required]
        public string RouteDescription { get; set; }
        [Required]
        public long? SMORegionId { get; set; }
        [Required]
        public int RRecoveryTime { get; set; }
        [Required]
        public bool IsReturnRouteRequired { get; set; }
        //public DateTime ReturnRouteRecoveryTime { get; set; }
    }

    public class SMORegionReceivingDTO
    {
        public string RegionName { get; set; }
        public string RegionDescription { get; set; }
        //public long CreatedById { get; set; }
        //public UserProfile CreatedBy { get; set; }
        //public DateTime CreatedAt { get; set; }
        //public DateTime UpdatedAt { get; set; }
    }

    public class SMOReturnRouteReceivingDTO
    {
        [Required]
        public long? SMORouteId { get; set; }
        [Required]
        public long? ReturnRouteId { get; set; }
        [Required]
        public int RRecoveryTime { get; set; }
     
    }
}
