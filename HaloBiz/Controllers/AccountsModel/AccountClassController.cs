using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.MyServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Controllers.AccountsModel
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AccountClassController : ControllerBase
    {
        private readonly IAccountClassService _accountClassService;

        public AccountClassController(IAccountClassService accountClassService)
        {
            this._accountClassService = accountClassService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetAccountClasses()
        {
            return await _accountClassService.GetAllAccountClasses();
        }
        [HttpGet("AccountClassBreakDown")]
        public async Task<ApiCommonResponse> GetAccountClassesBreakdown()
        {
            return await _accountClassService.GetBreakdownOfAccountClass();
        }

        [HttpGet("caption/{caption}")]
        public async Task<ApiCommonResponse> GetByCaption(string caption)
        {
            return await _accountClassService.GetAccountClassByCaption(caption);
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _accountClassService.GetAccountClassById(id);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewaccountClass(AccountClassReceivingDTO accountClassReceiving)
        {
            return await _accountClassService.AddAccountClass(HttpContext, accountClassReceiving);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, AccountClassReceivingDTO accountClassReceiving)
        {
            return await _accountClassService.UpdateAccountClass(id, accountClassReceiving);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(long id)
        {
            return await _accountClassService.DeleteAccountClass(id);
        }
    }
}
