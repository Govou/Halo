﻿namespace OnlinePortalBackend.DTOs.ReceivingDTOs
{
    public class LoadWalletDTO
    {
        public int ProfileId { get; set; }
        public double Amount { get; set; }
        public string Platform { get; set; }
    }

    public class SpendWalletDTO
    {
        public int ProfileId { get; set; }
        public double Amount { get; set; }
    }

    public enum WalletTransactionType
    {
        Load = 1, 
        Spend = 2,
    }
}