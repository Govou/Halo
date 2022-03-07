using HalobizMigrations.Models;
using HalobizMigrations.Models.Armada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class SMORoutesResourceTiesTransferDTO
    {
    }

    public class ArmedEscortSMORoutesResourceTieTransferDTO
    {
      
        public long Id { get; set; }
        public long? ResourceId { get; set; }
        public ArmedEscortProfile Resource { get; set; }
        public long? SMORegionId { get; set; }
        public SMORegion SMORegion { get; set; }
        public long? SMORouteId { get; set; }
        public SMORoute SMORoute { get; set; }
        public long CreatedById { get; set; }
        public UserProfile CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
    public class CommanderSMORoutesResourceTieTransferDTO
    {
        public long Id { get; set; }
        public long? ResourceId { get; set; }
        public CommanderProfile Resource { get; set; }
        public long? SMORegionId { get; set; }
        public SMORegion SMORegion { get; set; }
        public long? SMORouteId { get; set; }
        public SMORoute SMORoute { get; set; }
        public long CreatedById { get; set; }
        public UserProfile CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
    public class PilotSMORoutesResourceTieTransferDTO
    {
       
        public long Id { get; set; }
        public long? ResourceId { get; set; }
        public PilotProfile Resource { get; set; }
        public long? SMORegionId { get; set; }
        public SMORegion SMORegion { get; set; }
        public long? SMORouteId { get; set; }
        public SMORoute SMORoute { get; set; }
        public long CreatedById { get; set; }
        public UserProfile CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
    public class VehicleSMORoutesResourceTieTransferDTO
    {
      
        public long Id { get; set; }
        public long? ResourceId { get; set; }
        public Vehicle Resource { get; set; }
        public long? SMORegionId { get; set; }
        public SMORegion SMORegion { get; set; }
        public long? SMORouteId { get; set; }
        public SMORoute SMORoute { get; set; }
        public long CreatedById { get; set; }
        public UserProfile CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
