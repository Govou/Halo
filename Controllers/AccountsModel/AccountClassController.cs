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
            var response = await _accountClassService.GetAllAccountClasses();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var accountClass = ((ApiOkResponse)response).Result;
            return Ok(accountClass);
        }
        [HttpGet("AccountClassBreakDown")]
        public async Task<ApiCommonResponse> GetAccountClassesBreakdown()
        {
            var response = await _accountClassService.GetBreakdownOfAccountClass();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var accountClass = ((ApiOkResponse)response).Result;
            return Ok(accountClass);
        }

        [HttpGet("caption/{caption}")]
        public async Task<ApiCommonResponse> GetByCaption(string caption)
        {
            var response = await _accountClassService.GetAccountClassByCaption(caption);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var accountClass = ((ApiOkResponse)response).Result;
            return Ok((AccountClassTransferDTO)accountClass);
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            var response = await _accountClassService.GetAccountClassById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var accountClass = ((ApiOkResponse)response).Result;
            return Ok((AccountClassTransferDTO)accountClass);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewaccountClass(AccountClassReceivingDTO accountClassReceiving)
        {
            var response = await _accountClassService.AddAccountClass(HttpContext, accountClassReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var accountClass = ((ApiOkResponse)response).Result;
            return Ok((AccountClassTransferDTO)accountClass);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, AccountClassReceivingDTO accountClassReceiving)
        {
            var response = await _accountClassService.UpdateAccountClass(id, accountClassReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var accountClass = ((ApiOkResponse)response).Result;
            return Ok((AccountClassTransferDTO)accountClass);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(long id)
        {
            var response = await _accountClassService.DeleteAccountClass(id);
            return StatusCode(response.StatusCode);
        }
    }
}
