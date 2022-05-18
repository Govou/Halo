using Halobiz.Common.DTOs.ApiDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlinePortalBackend.DTOs.ReceivingDTOs;
using OnlinePortalBackend.MyServices.SecureMobilitySales;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Controllers.SecureMobilitySales
{
    [Route("api/[controller]")]
    [ApiController]
    public class SMSWalletController : ControllerBase
    {
        private readonly ISMSWalletService _walletService;
        public SMSWalletController(ISMSWalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpPost("ActivateWallet")]
        public async Task<ApiCommonResponse> ActivateWallet(ActivateWalletDTO request)
        {
            return await _walletService.ActivateWallet(request);
        }

        [HttpPost("LoadWallet")]
        public async Task<ApiCommonResponse> LoadWallet(LoadWalletDTO request)
        {
            return await _walletService.LoadWallet(request);
        }

        [HttpPost("SpendWallet")]
        public async Task<ApiCommonResponse> SpendWallet(SpendWalletDTO request)
        {
            return await _walletService.SpendWallet(request);
        }

        [HttpGet("GetWalletBalance")]
        public async Task<ApiCommonResponse> GetWalletBalance(int profileId)
        {
            return await _walletService.GetWalletBalance(profileId);
        }

        [HttpGet("GetWalletActivationStatus")]
        public async Task<ApiCommonResponse> GetWalletActivationStatus(int profileId)
        {
            return await _walletService.GetWalletActivationStatus(profileId);
        }
    }
}
