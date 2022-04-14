using Halobiz.Common.Auths;
using Halobiz.Common.DTOs.ApiDTOs;
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
    [ModuleName(HalobizModules.Finance,3)]

    public class AccountDetailController : ControllerBase
    {
        private readonly IAccountDetailService _AccountDetailService;

        public AccountDetailController(IAccountDetailService accountDetailService)
        {
            this._AccountDetailService = accountDetailService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetAccountDetails()
        {
            return await _AccountDetailService.GetAllAccountDetails();
        }

     

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _AccountDetailService.GetAccountDetailById(id);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewAccountDetail(AccountDetailReceivingDTO AccountDetailReceiving)
        {
            return await _AccountDetailService.AddAccountDetail(HttpContext, AccountDetailReceiving);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, AccountDetailReceivingDTO AccountDetailReceiving)
        {
            return await _AccountDetailService.UpdateAccountDetail(id, AccountDetailReceiving);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(long id)
        {
            return await _AccountDetailService.DeleteAccountDetail(id);
        }
    }
}
