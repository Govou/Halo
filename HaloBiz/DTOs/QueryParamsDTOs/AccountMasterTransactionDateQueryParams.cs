using System;
using System.Collections.Generic;

namespace halobiz_backend.DTOs.QueryParamsDTOs
{
    public class AccountMasterTransactionDateQueryParams
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string TransactionId { get; set; }
        public List<int> Years { get; set; }
        public long? ClientId { get; set; }
        public List<long> VoucherTypeIds { get; set; }
    }
}