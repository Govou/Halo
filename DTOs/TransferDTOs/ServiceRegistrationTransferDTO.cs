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
        public ICollection<ArmedEscortType> ApplicableArmedEscortTypes { get; set; }
        public ICollection<VehicleType> ApplicableVehicleTypes { get; set; }
        public ICollection<PilotType> ApplicablePilotTypes { get; set; }
        public ICollection<CommanderType> ApplicableCommanderTypes { get; set; }
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
       
        public long Id { get; set; }
        public long PilotQuantityRequired { get; set; }
        public bool IsDeleted { get; set; }
    }
}
