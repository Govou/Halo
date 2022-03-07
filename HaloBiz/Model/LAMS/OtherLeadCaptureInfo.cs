using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Model.LAMS
{
    public class OtherLeadCaptureInfo
    {
        [Key]
        public long Id { get; set; }
        public double CooperateEstimatedAnnualIncome { get; set; }
        public double CooperateEstimatedAnnualProfit { get; set; }
        public long CooperateBalanceSheetSize { get; set; }
        public long CooperateStaffStrength { get; set; }
        public long CooperateCashBookSize { get; set; }

        public double IndividualEstimatedAnnualIncome { get; set; }
        public double IndividualDisposableIncome { get; set; }
        public long IndividualResidenceSize { get; set; }

        [Required]
        public long GroupTypeId { get; set; }
        public virtual GroupType GroupType { get; set; }

        [Required]
        public long LeadDivisionId { get; set; }
        public LeadDivision LeadDivision { get; set; }
        public long? CustomerDivisionId { get; set; }
        public virtual CustomerDivision CustomerDivision { get; set; }


        [Required]
        public long CreatedById { get; set; }
        public virtual UserProfile CreatedBy { get; set; }
        public bool IsDeleted { get; set; } = false;
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }
    }
}
