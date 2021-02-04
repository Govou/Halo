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
        
        public decimal CooperateEstimatedAnnualIncome { get; set; }
        public decimal CooperateEstimatedAnnualProfit { get; set; }
        public long CooperateBalanceSheetSize { get; set; }
        public long CooperateStaffStrength { get; set; }
        public long CooperateCashBookSize { get; set; }

        public decimal IndividualEstimatedAnnualIncome { get; set; }
        public decimal IndividualDisposableIncome { get; set; }
        public long IndividualResidenceSize { get; set; }

        [Required]
        public long GroupTypeId { get; set; }
        public virtual GroupType GroupType { get; set; }

        [Required]
        public long LeadId { get; set; }
        public virtual Lead Lead { get; set; }


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
