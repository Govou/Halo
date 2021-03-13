using HaloBiz.DTOs.ApiDTOs;
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
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            this._accountService = accountService;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetAccountes()
        {
            var response = await _accountService.GetAllAccounts();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Account = ((ApiOkResponse)response).Result;
            return Ok((IEnumerable<AccountTransferDTO>)Account);
        }

        [HttpGet("CashBookAccounts")]
        public async Task<ActionResult> GetCashBookAccounts()
        {
            var response = await _accountService.GetCashBookAccounts();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Account = ((ApiOkResponse)response).Result;
            return Ok((IEnumerable<AccountTransferDTO>)Account);
        }


        [HttpPost("SeachAccount")]
        public async Task<ActionResult> SeachAccount(AccountSearchDTO accountSearchDTO)
        {
            var response = await _accountService.SearchForAccountDetails( accountSearchDTO);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Account = ((ApiOkResponse)response).Result;
            return Ok(Account);
        }

        [HttpGet("TradeIncome")]
        public async Task<ActionResult> GetTradeIncomeAccountes()
        {
            var response = await _accountService.GetAllTradeIncomeTaxAccounts();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Account = ((ApiOkResponse)response).Result;
            return Ok(Account);
        }
        [HttpGet("alias/{alias}")]
        public async Task<ActionResult> GetByAlias(string alias)
        {
            var response = await _accountService.GetAccountByAlias(alias);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Account = ((ApiOkResponse)response).Result;
            return Ok((AccountTransferDTO)Account);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var response = await _accountService.GetAccountById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Account = ((ApiOkResponse)response).Result;
            return Ok((AccountTransferDTO)Account);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNewAccount(AccountReceivingDTO accountReceiving)
        {
            var response = await _accountService.AddAccount(HttpContext, accountReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Account = ((ApiOkResponse)response).Result;
            return Ok((AccountTransferDTO)Account);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, AccountReceivingDTO accountReceiving)
        {
            var response = await _accountService.UpdateAccount(id, accountReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Account = ((ApiOkResponse)response).Result;
            return Ok((AccountTransferDTO)Account);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var response = await _accountService.DeleteAccount(id);
            return StatusCode(response.StatusCode);
        }
    }
}
