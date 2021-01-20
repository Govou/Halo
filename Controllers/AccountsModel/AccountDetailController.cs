﻿using HaloBiz.DTOs.ApiDTOs;
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
    public class AccountDetailController : ControllerBase
    {
        private readonly IAccountDetailService _AccountDetailService;

        public AccountDetailController(IAccountDetailService accountDetailService)
        {
            this._AccountDetailService = accountDetailService;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetAccountDetails()
        {
            var response = await _AccountDetailService.GetAllAccountDetails();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var AccountDetail = ((ApiOkResponse)response).Result;
            return Ok((IEnumerable<AccountDetailTransferDTO>)AccountDetail);
        }
        [HttpGet("name/{name}")]
        public async Task<ActionResult> GetByName(string name)
        {
            var response = await _AccountDetailService.GetAccountDetailByName(name);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var AccountDetail = ((ApiOkResponse)response).Result;
            return Ok((AccountDetailTransferDTO)AccountDetail);
        }
        [HttpGet("alias/{alias}")]
        public async Task<ActionResult> GetByAlias(long alias)
        {
            var response = await _AccountDetailService.GetAccountDetailByAlias(alias);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Account = ((ApiOkResponse)response).Result;
            return Ok((AccountDetailTransferDTO)Account);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var response = await _AccountDetailService.GetAccountDetailById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var AccountDetail = ((ApiOkResponse)response).Result;
            return Ok((AccountDetailTransferDTO)AccountDetail);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNewAccountDetail(AccountDetailReceivingDTO AccountDetailReceiving)
        {
            var response = await _AccountDetailService.AddAccountDetail(HttpContext, AccountDetailReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var AccountDetail = ((ApiOkResponse)response).Result;
            return Ok((AccountDetailTransferDTO)AccountDetail);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, AccountDetailReceivingDTO AccountDetailReceiving)
        {
            var response = await _AccountDetailService.UpdateAccountDetail(id, AccountDetailReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var AccountDetail = ((ApiOkResponse)response).Result;
            return Ok((AccountDetailTransferDTO)AccountDetail);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(long id)
        {
            var response = await _AccountDetailService.DeleteAccountDetail(id);
            return StatusCode(response.StatusCode);
        }
    }
}
