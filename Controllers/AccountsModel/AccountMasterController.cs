using HaloBiz.DTOs.ApiDTOs;
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
    public class AccountMasterController : ControllerBase
    {
        private readonly IAccountMasterService _AccountMasterService;

        public AccountMasterController(IAccountMasterService AccountMasterService)
        {
            this._AccountMasterService = AccountMasterService;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetAccountMasters([FromQuery]AccountMasterTransactionDateQueryParams query)
        {
            var response = await _AccountMasterService.QueryAccountMasters( query);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var AccountMaster = ((ApiOkResponse)response).Result;
            return Ok((IEnumerable<AccountMasterTransferDTO>)AccountMaster);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var response = await _AccountMasterService.GetAccountMasterById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var AccountMaster = ((ApiOkResponse)response).Result;
            return Ok((AccountMasterTransferDTO)AccountMaster);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNewAccountMaster(AccountMasterReceivingDTO AccountMasterReceiving)
        {
            var response = await _AccountMasterService.AddAccountMaster(HttpContext, AccountMasterReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var AccountMaster = ((ApiOkResponse)response).Result;
            return Ok((AccountMasterTransferDTO)AccountMaster);
        }

        [HttpGet("GetAccountMasterByTransactionId/{transactionId}")]
        public async Task<ActionResult> GetAccountMasterByTransactionId(string transactionId)
        {
            var response = await _AccountMasterService.GetAllAccountMastersByTransactionId(transactionId);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var AccountMaster = ((ApiOkResponse)response).Result;
            return Ok(AccountMaster);
        }

        [HttpPost("GetAccountMasterByCustomerIdAndContractYears")]
        public async Task<ActionResult> GetAccountMasterByTransactionId(AccountMasterTransactionDateQueryParams searchDto)
        {
            var response = await _AccountMasterService.GetAllAccountMastersByCustomerIdAndContractYear(searchDto);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var accountMasters = ((ApiOkResponse)response).Result;
            return Ok(accountMasters);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, AccountMasterReceivingDTO AccountMasterReceiving)
        {
            var response = await _AccountMasterService.UpdateAccountMaster(id, AccountMasterReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var AccountMaster = ((ApiOkResponse)response).Result;
            return Ok((AccountMasterTransferDTO)AccountMaster);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(long id)
        {
            var response = await _AccountMasterService.DeleteAccountMaster(id);
            return StatusCode(response.StatusCode);
        }
    }
}
