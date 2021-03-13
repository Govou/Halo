using HaloBiz.Model.LAMS;

namespace HaloBiz.DTOs.TransferDTOs.LAMS
{
    public class LeadContactTransferDTO : ContactTransferDTO
    {
        public ContactType Type { get; set; }
    }
}