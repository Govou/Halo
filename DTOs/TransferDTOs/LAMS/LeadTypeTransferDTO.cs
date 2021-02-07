using System.Collections.Generic;
using HaloBiz.Model.LAMS;

namespace HaloBiz.DTOs.TransferDTOs.LAMS
{
    public class LeadTypeTransferDTO
    {
        public long Id { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
        public IEnumerable<LeadOriginWithoutTypeTransferDTO> LeadOrigins { get; set; }
    }
    public class LeadTypeWithoutOriginDTO
    {
        public long Id { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }

    }
}