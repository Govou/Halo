using HalobizMigrations.Models;
using HalobizMigrations.Models.Armada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class SMORouteTransferDTO
    {
        public long Id { get; set; }
        public string RouteName { get; set; }
        public string RouteDescription { get; set; }
        public long? SMORegionId { get; set; }

        public SMORegion SMORegion { get; set; }
        public int RRecoveryTime { get; set; }
        public bool IsReturnRouteRequired { get; set; }
        public DateTime CreatedAt { get; set; }
        public long CreatedById { get; set; }
        public UserProfile CreatedBy { get; set; }

        public List<VehicleSMORoutesResourceTie> VehiclesOnRoute { get; set; }
        public List<PilotSMORoutesResourceTie> PilotsOnRoute { get; set; }
        public List<ArmedEscortSMORoutesResourceTie> ArmedEscortsOnRoute { get; set; }
     
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class SMOReturnRouteTransferDTO
    {
        public long Id { get; set; }
        public long? SMORouteId { get; set; }
        public SMORoute SMORoute { get; set; }
        public long? ReturnRouteId { get; set; }
        public SMORoute ReturnRoute { get; set; }
        public int RRecoveryTime { get; set; }
        public long CreatedById { get; set; }
        public UserProfile CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class SMORegionTransferDTO
    {
        public long Id { get; set; }
        public string RegionName { get; set; }
        public string RegionDescription { get; set; }
        public long CreatedById { get; set; }
        public UserProfile CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<SMORoute> SMORoutes { get; set; }
    }
}
