using System.ComponentModel.DataAnnotations;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class FinancialVoucherTypeReceivingDTO
    {
        public string VoucherType { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        public string Alias { get; set; } //To map with TranType from Dtrack
    }
}