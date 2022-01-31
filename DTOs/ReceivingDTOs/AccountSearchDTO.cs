using System;
using System.Collections.Generic;

namespace halobiz_backend.DTOs.ReceivingDTOs
{
    public class AccountSearchDTO
    {
        public long AccountId { get; set; }
        public DateTime TransactionStart { get; set; }
        public DateTime TransactionEnd { get; set; }
        public List<Int64> VoucherTypeIds { get; set; }
    }
}