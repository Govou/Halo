using System.Collections.Generic;
using HaloBiz.Model;
using HaloBiz.Model.LAMS;

namespace HaloBiz.DTOs.TransferDTOs.LAMS
{
    public class OtherLeadCaptureInfoTransferDTO
    {
        public long Id { get; set; }

        public decimal CooperateEstimatedAnnualIncome { get; set; }
        public decimal CooperateEstimatedAnnualProfit { get; set; }
        public long CooperateBalanceSheetSize { get; set; }
        public long CooperateStaffStrength { get; set; }
        public long CooperateCashBookSize { get; set; }

        public decimal IndividualEstimatedAnnualIncome { get; set; }
        public decimal IndividualDisposableIncome { get; set; }
        public long IndividualResidenceSize { get; set; }

        public long GroupTypeId { get; set; }
        public virtual GroupType GroupType { get; set; }

        public long LeadId { get; set; }
        public virtual Lead Lead { get; set; }

        public long CreatedById { get; set; }
        public virtual UserProfile CreatedBy { get; set; }
    }
}