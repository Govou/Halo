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
}
