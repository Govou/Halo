using System.Collections.Generic;


namespace HaloBiz.DTOs.TransferDTOs.LAMS
{
    public class EndorsementTypeTransferDTO
    {
        public long Id { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
        public long CreatedById { get; set; }
    }
}