using HalobizMigrations.Models;
using System.ComponentModel.DataAnnotations;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class OtherLeadCaptureInfoReceivingDTO
    {
        public double CooperateEstimatedAnnualIncome { get; set; }
        public double CooperateEstimatedAnnualProfit { get; set; }
        public long CooperateBalanceSheetSize { get; set; }
        public long CooperateStaffStrength { get; set; }
        public long CooperateCashBookSize { get; set; }
        public double IndividualEstimatedAnnualIncome { get; set; }
        public double IndividualDisposableIncome { get; set; }
        public long IndividualResidenceSize { get; set; }
        public long GroupTypeId { get; set; }
        public long LeadDivisionId { get; set; }
    }
}