using Halobiz.Common.Auths;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.MyServices;
using halobiz_backend.DTOs.QueryParamsDTOs;
using halobiz_backend.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Controllers.AccountsModel
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ModuleName(HalobizModules.Finance,4)]

    public class AccountMasterController : ControllerBase
    {
        private readonly IAccountMasterService _AccountMasterService;

        public AccountMasterController(IAccountMasterService AccountMasterService)
        {
            this._AccountMasterService = AccountMasterService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetAccountMasters([FromQuery]AccountMasterTransactionDateQueryParams query)
        {
            var result = await _AccountMasterService.QueryAccountMasters(query);
            return result;
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _AccountMasterService.GetAccountMasterById(id);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewAccountMaster(AccountMasterReceivingDTO AccountMasterReceiving)
        {
            return await _AccountMasterService.AddAccountMaster(HttpContext, AccountMasterReceiving);
        }

        [HttpGet("GetAccountMasterByTransactionId/{transactionId}")]
        public async Task<ApiCommonResponse> GetAccountMasterByTransactionId(string transactionId)
        {
            return await _AccountMasterService.GetAllAccountMastersByTransactionId(transactionId);
        }

        [HttpPost("GetAccountMasterByCustomerIdAndContractYears")]
        public async Task<ApiCommonResponse> GetAccountMasterByTransactionId(AccountMasterTransactionDateQueryParams searchDto)
        {
            return await _AccountMasterService.GetAllAccountMastersByCustomerIdAndContractYear(searchDto);
        }

        [HttpPost("PostPeriodicAccounts")]
        public async Task<ApiCommonResponse> PostPeriodicAccount()
        {
            return await _AccountMasterService.PostPeriodicAccountMaster(HttpContext);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, AccountMasterReceivingDTO AccountMasterReceiving)
        {
            return await _AccountMasterService.UpdateAccountMaster(id, AccountMasterReceiving);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(long id)
        {
            return await _AccountMasterService.DeleteAccountMaster(id);
        }
    }
}
