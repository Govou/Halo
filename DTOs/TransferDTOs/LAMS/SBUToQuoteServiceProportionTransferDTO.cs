using HaloBiz.Helpers;
using HaloBiz.Model;
using HaloBiz.Model.LAMS;

namespace HaloBiz.DTOs.TransferDTOs.LAMS
{
    public class SBUToQuoteServiceProportionTransferDTO
    {
        public long Id { get; set; }
        public double Proportion { get; set; }
        public ProportionStatusType Status { get; set; }
        public StrategicBusinessUnit StrategicBusinessUnit { get; set; }
        public UserProfile UserInvolved { get; set; }
        public long QuoteServiceId { get; set; }
    }
}