using Halobiz.Common.DTOs.ApiDTOs;
using HalobizMigrations.Models;
using HalobizMigrations.Models.OnlinePortal;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace OnlinePortalBackend.MyServices.SecureMobilitySales
{
    public interface ISMSWalletService
    {
        Task<ApiCommonResponse> ActivateWallet();
        Task<ApiCommonResponse> LoadWallet();
        Task<ApiCommonResponse> SpendWallet();
    }

    public class WalletMaster
    {
        public long Id { get; set; }
        public OnlineProfile OnlineProfile { get; set; }

        [ForeignKey("OnlineProfile")]
        public int OnlineProfileId { get; set; }

        public Account Account { get; set; }

        [ForeignKey("Account")]
        public int WalletliabilityAccount { get; set; }

        public string WalletPin { get; set; }
        public string SecurityQuestion { get; set; }
        public string SecurityAnswer { get; set; }
        public string WalletBalance { get; set; }
        public int CreatedById { get; set; }

        [ForeignKey("CreatedById")]
        public UserProfile CreatedBy { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public DateTime DateTimeLastUpdated { get; set; }
    }

    public class WalletDetails
    {
        public long Id { get; set; }
        public WalletMaster WalletMaster { get; set; }

        [ForeignKey("WalletMaster")]
        public long WalletMasterId { get; set; }

        public string Platform { get; set; }
        public int TransactionType { get; set; }
        public string TransactionValue { get; set; }
        public string MovingBalance { get; set; }
        public int CreatedById { get; set; }

        [ForeignKey("CreatedById")]
        public UserProfile CreatedBy { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public DateTime DateTimeLastUpdated { get; set; }
    }
}
