using HalobizMigrations.Models;
using HalobizMigrations.Models.Armada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class ServiceRegistrationReceivingDTO
    {
        public string[] ApplicableArmedEscortTypes { get; set; }
        public string[] ApplicableVehicleTypes { get; set; }
        public string[] ApplicablePilotTypes { get; set; }
        public string[] ApplicableCommanderTypes { get; set; }
        public long ArmedEscortQuantityRequired { get; set; }
        public bool RequiresArmedEscort { get; set; }
        public bool RequiresPilot { get; set; }
        public long VehicleQuantityRequired { get; set; }
        public bool RequiresVehicle { get; set; }
        public long CommanderQuantityRequired { get; set; }
        public bool RequiresCommander { get; set; }
        public long ServiceId { get; set; }

        public long PilotQuantityRequired { get; set; }
    }
}
