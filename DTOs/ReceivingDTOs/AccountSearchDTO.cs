using System;

namespace halobiz_backend.DTOs.ReceivingDTOs
{
    public class AccountSearchDTO
    {
        public long AccountId { get; set; }
        public DateTime TransactionStart { get; set; }
        public DateTime TransactionEnd { get; set; }
        public long VoucherTypeId { get; set; }
    }
}