using HaloBiz.Model.LAMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.TransferDTOs.LAMS
{
    public class LeadDivisionContactTransferDTO : ContactTransferDTO
    {
        public ContactType Type { get; set; }
    }
}
