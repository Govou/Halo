﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class RegServicesResourceTypesReceivingDTO
    {
    }

    public class CommanderTypeRegReceivingDTO
    {
    
      
        [Required]
        public long[] ServiceRegistrationId { get; set; }
    }
    public class AEscortTypeRegReceivingDTO
    {
       
     
        [Required]
        public long[] ServiceRegistrationId { get; set; }
    }
    public class PilotTypeRegReceivingDTO
    {
    
     
        [Required]
        public long[] ServiceRegistrationId { get; set; }
    }
    public class VehicleTypeRegReceivingDTO
    {
       
     
        [Required]
        public long[] ServiceRegistrationId { get; set; }
    }
}
