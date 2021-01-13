using System.ComponentModel.DataAnnotations;

namespace HaloBiz.DTOs.TransferDTOs.LAMS
{
    public class FinancialVoucherTypeTransferDTO
    {
        [Key]
        public long Id { get; set; }
        public string VoucherType { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        [Required]
        public long CreatedById { get; set; }
        public virtual UserProfile CreatedBy { get; set; }
    }
}