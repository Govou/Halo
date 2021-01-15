using System.ComponentModel.DataAnnotations;
using HaloBiz.Model;

namespace HaloBiz.DTOs.TransferDTOs.LAMS
{
    public class FinancialVoucherTypeTransferDTO
    {
        public long Id { get; set; }
        public string VoucherType { get; set; }
        public string Description { get; set; }
    }
}