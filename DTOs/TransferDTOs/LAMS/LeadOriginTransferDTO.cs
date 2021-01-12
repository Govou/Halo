using System.ComponentModel.DataAnnotations;

namespace HaloBiz.DTOs.TransferDTOs.LAMS
{
    public class LeadOriginTransferDTO
    {
        [Key]
        public long Id { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
    }
}