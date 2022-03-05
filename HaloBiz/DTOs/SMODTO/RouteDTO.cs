using HalobizMigrations.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.SMODTO
{
    public class RouteDTO
    {

        //[Key]
        public long Id { get; set; }
        public string RouteName { get; set; }
        public string RouteDescription { get; set; }
        public long? SMORegionId { get; set; }
        
        //public SMORegion SMORegion { get; set; }
        public DateTime RecoveryTime { get; set; }
        public bool IsReturnRouteRequired { get; set; }
        public DateTime CreatedAt { get; set; }
        public long CreatedById { get; set; }
        public UserProfile CreatedBy { get; set; }
     
    }
}
