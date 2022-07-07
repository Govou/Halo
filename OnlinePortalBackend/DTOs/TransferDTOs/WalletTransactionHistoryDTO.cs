using System;
using System.Collections.Generic;

namespace OnlinePortalBackend.DTOs.TransferDTOs
{
    public class WalletTransactionHistoryDTO
    {
        public int ProfileId { get; set; }
        public decimal Balance { get; set; }
        public decimal TotalTopup { get; set; }
        public decimal TotalSpend { get; set; }
        public IEnumerable<WalletTransactionActivity> TransactionHistory { get; set; }
    }

    public class WalletTransactionActivity
    {
        public string Platform { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
    }

    public class WalletTransactionStatistics
    {
        public int TotalTransactions { get; set; }
        public int TopUpCount { get; set; }
        public int TopUpPercent { get; set; }
        public int SpendCount { get; set; }
        public int SpendPercent { get; set; }
    }
}