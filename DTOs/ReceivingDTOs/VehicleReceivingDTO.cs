using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class VehicleReceivingDTO
    {
        public long? SupplierServiceId { get; set; }

        public long? AttachedBranchId { get; set; }

        public long? AttachedOfficeId { get; set; }

        public long? VehicleTypeId { get; set; }
    }

    public class VehicleTypeReceivingDTO
    {
        //public long Id { get; set; }
        [Required]
        public string TypeName { get; set; }
        [Required]
        public string TypeDesc { get; set; }
       
    }
}
