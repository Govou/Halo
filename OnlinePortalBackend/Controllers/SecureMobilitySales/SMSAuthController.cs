using Halobiz.Common.DTOs.ApiDTOs;
using Halobiz.Common.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlinePortalBackend.MyServices;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Controllers.SecureMobilitySales
{
    [Route("api/[controller]")]
    [ApiController]
    public class SMSAuthController : ControllerBase
    {
        private readonly IOnlineAccounts _authService;
        public SMSAuthController(IOnlineAccounts authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpGet("SendCode")]
        public async Task<ApiCommonResponse> SendCode(string email)
        {
            return await _authService.SendConfirmCodeToClient(email);
        }


        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ApiCommonResponse> Login(LoginDTO login)
        {
            return await _authService.Login(login);
        }

        [AllowAnonymous]
        [HttpPost("VerifyCode")]
        public async Task<ApiCommonResponse> VerifyCode(CodeVerifyModel model)
        {
            return await _authService.VerifyCode(model);
        }
    }
}
