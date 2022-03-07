using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaloBiz.Model.LAMS
{
    public class FinanceVoucherType
    {
        [Key]
        public long Id { get; set; }
        public string VoucherType { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        public string Alias { get; set; } //To map with TranType from Dtrack
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