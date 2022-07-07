using Halobiz.Common.DTOs.ApiDTOs;
using Halobiz.Common.DTOs.ReceivingDTOs;
using HalobizMigrations.Models;
using HalobizMigrations.Models.OnlinePortal;
using OnlinePortalBackend.DTOs.ReceivingDTOs;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace OnlinePortalBackend.MyServices.SecureMobilitySales
{
    public interface ISMSWalletService
    {
        Task<ApiCommonResponse> ActivateWallet(ActivateWalletDTO request);
        Task<ApiCommonResponse> LoadWallet(LoadWalletDTO request);
        Task<ApiCommonResponse> WalletLogin(WalletLoginDTO request);
        Task<ApiCommonResponse> SpendWallet(SpendWalletDTO request);
        Task<ApiCommonResponse> GetWalletBalance(int profileId);
        Task<ApiCommonResponse> GetWalletActivationStatus(int profileId);
        Task<ApiCommonResponse> GetWalletTransactionHistory(int profileId);
        Task<ApiCommonResponse> GetWalletTransactionStatistics(int profileId);
    }
}
