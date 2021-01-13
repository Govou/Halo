using System.ComponentModel.DataAnnotations;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class FinancialVoucherTypeReceivingDTO
    {
        public string VoucherType { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        [Required]
        public long CreatedById { get; set; }
    }
}