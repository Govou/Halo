using System.ComponentModel.DataAnnotations;

namespace HaloBiz.DTOs.TransferDTOs.LAMS
{
    public class LeadOriginTransferDTO
    {
        public long Id { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
        public long LeadTypeId { get; set; }
        public LeadTypeWithoutOriginDTO LeadType { get; set; }
    }

    
     public class LeadOriginWithoutTypeTransferDTO
    {
        public long Id { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
        public long LeadTypeId { get; set; }

    }
}