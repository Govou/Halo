using Halobiz.Common.DTOs.ApiDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Controllers.SecureMobilitySales
{
    [Route("api/[controller]")]
    [ApiController]
    public class SMSWalletController : ControllerBase
    {
        private readonly IWalletService _walletService;
        public SMSWalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpPost("ActivateWallet")]
        public async Task<ApiCommonResponse> ActivateWallet(int profileId)
        {
            return await _authService.VerifyCode(model);
        }
    }
}
