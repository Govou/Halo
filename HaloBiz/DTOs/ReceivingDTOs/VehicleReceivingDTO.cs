using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class VehicleReceivingDTO
    {
        public long SupplierServiceId { get; set; }

        public long? AttachedBranchId { get; set; }

        public long? AttachedOfficeId { get; set; }

        public long? VehicleTypeId { get; set; }
        public string FrontViewImage { get; set; }
        public string LeftViewImage { get; set; }
        public string RightViewImage { get; set; }
        public string RearViewImage { get; set; }
        public string TopViewImage { get; set; }
        public string InteriorViewImage { get; set; }
    }

    public class VehicleTypeReceivingDTO
    {
        //public long Id { get; set; }
        [Required]
        public string TypeName { get; set; }
        [Required]
        public string TypeDesc { get; set; }
        //public long ServiceRegistrationId { get; set; }

    }
}
