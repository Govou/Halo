using Halobiz.Common.DTOs.ReceivingDTOs;
using HaloBiz.Helpers;
using HalobizMigrations.Models;


namespace HaloBiz.DTOs.TransferDTOs.LAMS
{
    public class SbutoQuoteServiceProportionTransferDTO
    {
        public long Id { get; set; }
        public double Proportion { get; set; }
        public ProportionStatusType Status { get; set; }
        public StrategicBusinessUnit StrategicBusinessUnit { get; set; }
        public UserProfile UserInvolved { get; set; }
        public long QuoteServiceId { get; set; }
    }
}