using HalobizMigrations.Models;
using HalobizMigrations.Models.Armada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class ServiceRegistrationTransferDTO
    {
        public DateTime CreatedAt { get; set; }
       
        public UserProfile CreatedBy { get; set; }
        public long CreatedById { get; set; }
        public List<ArmedEscortType> ApplicableArmedEscortTypes { get; set; }
        public List<VehicleType> ApplicableVehicleTypes { get; set; }
        public List<PilotType> ApplicablePilotTypes { get; set; }
        public List<CommanderType> ApplicableCommanderTypes { get; set; }
        public long ArmedEscortQuantityRequired { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool RequiresArmedEscort { get; set; }
        public bool RequiresPilot { get; set; }
        public long VehicleQuantityRequired { get; set; }
        public bool RequiresVehicle { get; set; }
        public long CommanderQuantityRequired { get; set; }
        public bool RequiresCommander { get; set; }
        public Service Service { get; set; }
        public long ServiceId { get; set; }
        public string Description { get; set; }
        public long Id { get; set; }
        public long PilotQuantityRequired { get; set; }
        public bool IsDeleted { get; set; }
    }
}
