

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HaloBiz.Helpers;

namespace HaloBiz.Model.LAMS
{
    public class Quote
    {
        [Key]
        public long Id { get; set; }
        [StringLength(50)]
        public string ReferenceNo { get; set; }
        public long LeadDivisionId { get; set; }
        public LeadDivision LeadDivision { get; set; }
        public bool IsConvertedToContract { get; set; } = false;
        public bool IsApproved { get; set; } = false;
        public IEnumerable<QuoteService> QuoteServices { get; set; }
        public VersionType Version { get; set; } = VersionType.Latest;
        public long CreatedById { get; set; }
        public virtual UserProfile CreatedBy { get; set; }
        public IEnumerable<Contract> Contracts { get; set; }
        public bool IsDeleted { get; set; } = false;
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }

    }
}
