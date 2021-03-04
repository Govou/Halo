using System.Collections.Generic;
using HaloBiz.Model;
using HaloBiz.Model.LAMS;

namespace HaloBiz.DTOs.TransferDTOs.LAMS
{
    public class OtherLeadCaptureInfoTransferDTO
    {
        public long Id { get; set; }

        public double CooperateEstimatedAnnualIncome { get; set; }
        public double CooperateEstimatedAnnualProfit { get; set; }
        public long CooperateBalanceSheetSize { get; set; }
        public long CooperateStaffStrength { get; set; }
        public long CooperateCashBookSize { get; set; }

        public double IndividualEstimatedAnnualIncome { get; set; }
        public double IndividualDisposableIncome { get; set; }
        public long IndividualResidenceSize { get; set; }

        public long GroupTypeId { get; set; }
        public virtual GroupType GroupType { get; set; }
        public long LeadDivisionId { get; set; }
        public  LeadDivision LeadDivision { get; set; }
        public long CustomerDivisionId { get; set; }
        public CustomerDivision CustomerDivision { get; set; }
        public long CreatedById { get; set; }
        public virtual UserProfile CreatedBy { get; set; }
    }
}