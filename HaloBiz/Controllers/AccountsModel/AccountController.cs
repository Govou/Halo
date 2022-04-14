using Halobiz.Common.Auths;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.MyServices;
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
    [ModuleName(HalobizModules.Finance,2)]

    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            this._accountService = accountService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetAccountes()
        {
            return await _accountService.GetAllAccounts();
        }

        [HttpGet("CashBookAccounts")]
        public async Task<ApiCommonResponse> GetCashBookAccounts()
        {
            return await _accountService.GetCashBookAccounts();
        }


        [HttpPost("SeachAccount")]
        public async Task<ApiCommonResponse> SeachAccount(AccountSearchDTO accountSearchDTO)
        {
            return await _accountService.SearchForAccountDetails( accountSearchDTO);
        }

        [HttpGet("TradeIncome")]
        public async Task<ApiCommonResponse> GetTradeIncomeAccountes()
        {
            return await _accountService.GetAllTradeIncomeTaxAccounts();
        }
        [HttpGet("alias/{alias}")]
        public async Task<ApiCommonResponse> GetByAlias(string alias)
        {
            return await _accountService.GetAccountByAlias(alias);
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _accountService.GetAccountById(id);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewAccount(AccountReceivingDTO accountReceiving)
        {
            return await _accountService.AddAccount(HttpContext, accountReceiving);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, AccountReceivingDTO accountReceiving)
        {
            return await _accountService.UpdateAccount(id, accountReceiving);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _accountService.DeleteAccount(id);
        }
    }
}
