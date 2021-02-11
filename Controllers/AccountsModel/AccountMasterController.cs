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
    public class AccountMasterController : ControllerBase
    {
        private readonly IAccountMasterService _AccountMasterService;

        public AccountMasterController(IAccountMasterService AccountMasterService)
        {
            this._AccountMasterService = AccountMasterService;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetAccountMasters()
        {
            var response = await _AccountMasterService.GetAllAccountMasters();
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
