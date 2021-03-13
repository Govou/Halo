using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class RequredServiceQualificationElementTransferDTO : BaseSetupTransferDTO
    {
        public long ServiceCategoryId { get; set; }
        public string Type { get; set; }
    }
}
