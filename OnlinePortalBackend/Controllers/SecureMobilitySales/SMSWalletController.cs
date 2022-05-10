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
        public async Task<ApiCommonResponse> LoadWallet(ActivateWalletDTO request)
        {
            return await _walletService.ActivateWallet(request);
        }

        [HttpPost("SpendWallet")]
        public async Task<ApiCommonResponse> SpendWallet(ActivateWalletDTO request)
        {
            return await _walletService.ActivateWallet(request);
        }
    }
}
