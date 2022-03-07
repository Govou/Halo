using System.ComponentModel.DataAnnotations;
using HalobizMigrations.Models;

namespace HaloBiz.DTOs.TransferDTOs.LAMS
{
    public class FinancialVoucherTypeTransferDTO
    {
        public long Id { get; set; }
        public string VoucherType { get; set; }
        public string Alias { get; set; } //To map with TranType from Dtrack
        public string Description { get; set; }
    }
}