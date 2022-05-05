using Halobiz.Common.DTOs.ApiDTOs;
using Halobiz.Common.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlinePortalBackend.MyServices.SecureMobilitySales;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Controllers.SecureMobilitySales
{
    [Route("api/[controller]")]
    [ApiController]
    public class SMSAccountController : ControllerBase
    {
        private readonly ISMSAccountService _accountService;
        public SMSAccountController(ISMSAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("CreateIndividualAccount")]
        public Task<ApiCommonResponse> CreateIndividualAccount(SMSIndividualAccountDTO request)
        {
           return _accountService.CreateIndividualAccount(request);
        }

        [HttpPost("CreateBusinessAccount")]
        public Task<ApiCommonResponse> CreateBusinessAccount(SMSBusinessAccountDTO request)
        {
            return _accountService.CreateBusinessAccount(request);
        }
    }
}
