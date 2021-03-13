using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HaloBiz.Helpers;

namespace HaloBiz.Model.LAMS
{
    public class Contract
    {
        [Key]
        public long Id { get; set; }
        [StringLength(50)]
        public string ReferenceNo { get; set; }
        public long CustomerDivisionId { get; set; }
        public CustomerDivision CustomerDivision { get; set; }
        public long QuoteId { get; set; }
        public virtual Quote Quote { get; set; }
        public IEnumerable<ContractService> ContractServices { get; set; }
        public VersionType Version { get; set; } = VersionType.Latest;
        public long CreatedById { get; set; }
        public virtual UserProfile CreatedBy { get; set; }
        public bool IsDeleted { get; set; } = false;
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }
    }
}
