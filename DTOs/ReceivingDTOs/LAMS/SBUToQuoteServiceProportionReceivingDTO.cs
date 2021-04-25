using HaloBiz.Helpers;
using HalobizMigrations.Models;

namespace HaloBiz.DTOs.ReceivingDTOs.LAMS
{
    public class SbutoQuoteServiceProportionReceivingDTO
    {
        public double Proportion { get; set; }
        public ProportionStatusType Status { get; set; }
        public long UserInvolvedId { get; set; }
        public long QuoteServiceId { get; set; }
        public long StrategicBusinessUnitId { get; set; }
    }
}