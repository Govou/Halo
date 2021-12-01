using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class RegServicesResourceTypesReceivingDTO
    {
        public class CommanderTypeRegReceivingDTO
        {

            public long? ServiceRegistrationId { get; set; }
            public long[] CommanderTypeId { get; set; }

            // public long[] ServiceRegistrationId { get; set; }
        }
        public class AEscortTypeRegReceivingDTO
        {

            public long? ServiceRegistrationId { get; set; }
            public long[] ArmedEscortTypeId { get; set; }
            //public long[] ServiceRegistrationId { get; set; }
        }
        public class PilotTypeRegReceivingDTO
        {
            public long? ServiceRegistrationId { get; set; }
            public long[] PilotTypeId { get; set; }
            //public long[] ServiceRegistrationId { get; set; }
        }
        public class VehicleTypeRegReceivingDTO
        {

            public long? ServiceRegistrationId { get; set; }
            public long[] VehicleTypeId { get; set; }
            //[Required]
            // public long[] ServiceRegistrationId { get; set; }
        }
    }

 

    public class AllResourceTypesPerServiceRegReceivingDTO
    {
        public long VehicleQuantity { get; set; }
        public long? VehicleTypeId { get; set; }

        public long PilotQuantity { get; set; }
        public long? PilotTypeId { get; set; }

        public long CommanderQuantity { get; set; }
        public long? CommanderTypeId { get; set; }
        public long ArmedEscortQuantity { get; set; }
        public long? ArmedEscortTypeId { get; set; }

        [Required]
        public long[] ArmedEscortEServiceRegistrationId { get; set; }
        [Required]
        public long[] CommanderServiceRegistrationId { get; set; }
        [Required]
        public long[] PilotServiceRegistrationId { get; set; }
        [Required]
        public long[] VehicleServiceRegistrationId { get; set; }
    }
}
