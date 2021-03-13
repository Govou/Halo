using HaloBiz.Model.LAMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ReceivingDTOs.LAMS
{
    public class LeadDivisionContactReceivingDTO : ContactRecievingDTO
    {
        public ContactType Type { get; set; }
    }
}
