using HaloBiz.Model.LAMS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Model
{
    public class Approval
    {
        [Key]
        public long Id { get; set; }
        
        [Required, MinLength(3), MaxLength(255)]
        public string Caption { get; set; }

        public DateTime? DateTimeApproved { get; set; }

        [Required]
        public bool IsApproved { get; set; }

        [Required]
        public long ResponsibleId { get; set; }
        public virtual UserProfile Responsible { get; set; }
        
        [Required]
        public long Sequence { get; set; }

        [Required]
        public string Level { get; set; }

        public long? QuoteServiceId { get; set; }
        public virtual QuoteService QuoteService { get; set; }

        public long? QuoteId { get; set; }
        public virtual Quote Quote { get; set; }

        public long? ContractId { get; set; }
        public virtual Contract Contract { get; set; }

        public long? ContractServiceId { get; set; }
        public virtual ContractService ContractService { get; set; }

        public long? ContractServiceForEndorsementId { get; set; }
        public virtual ContractServiceForEndorsement ContractServiceForEndorsement { get; set; }

        public long? ServicesId { get; set; }
        public virtual Services Services { get; set; }

        public bool IsDeleted { get; set; } = false;
        public long CreatedById { get; set; }
        public virtual UserProfile CreatedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }
    }
}
