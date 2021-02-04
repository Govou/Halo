using HaloBiz.Model;
using HaloBiz.Model.LAMS;
using System.ComponentModel.DataAnnotations;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class OtherLeadCaptureInfoReceivingDTO
    {
        public decimal CooperateEstimatedAnnualIncome { get; set; }
        public decimal CooperateEstimatedAnnualProfit { get; set; }
        public long CooperateBalanceSheetSize { get; set; }
        public long CooperateStaffStrength { get; set; }
        public long CooperateCashBookSize { get; set; }

        public decimal IndividualEstimatedAnnualIncome { get; set; }
        public decimal IndividualDisposableIncome { get; set; }
        public long IndividualResidenceSize { get; set; }
        public long GroupTypeId { get; set; }
        public long LeadId { get; set; }
    }
}