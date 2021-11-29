using HalobizMigrations.Models;
using HalobizMigrations.Models.Armada;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class ServiceRegistrationReceivingDTO
    {
        //public ArmedEscortType[] ApplicableArmedEscortTypes { get; set; }
        //public VehicleType[] ApplicableVehicleTypes { get; set; }
        //public PilotType[] ApplicablePilotTypes { get; set; }
        //public CommanderType[] ApplicableCommanderTypes { get; set; }
     
        public long ArmedEscortQuantityRequired { get; set; }
        [Required]
        public bool RequiresArmedEscort { get; set; }
        [Required]
        public bool RequiresPilot { get; set; }
        //[Required]
        public long VehicleQuantityRequired { get; set; }
        [Required]
        public bool RequiresVehicle { get; set; }
        public long CommanderQuantityRequired { get; set; }
        [Required]
        public bool RequiresCommander { get; set; }
        [Required]
        public long ServiceId { get; set; }

        public long PilotQuantityRequired { get; set; }
    }
}
