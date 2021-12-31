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
            var response = await _AccountDetailService.GetAllAccountDetails();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var AccountDetail = ((ApiOkResponse)response).Result;
            return Ok((IEnumerable<AccountDetailTransferDTO>)AccountDetail);
        }

     

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            var response = await _AccountDetailService.GetAccountDetailById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var AccountDetail = ((ApiOkResponse)response).Result;
            return Ok((AccountDetailTransferDTO)AccountDetail);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewAccountDetail(AccountDetailReceivingDTO AccountDetailReceiving)
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
        public async Task<ApiCommonResponse> DeleteById(long id)
        {
            var response = await _AccountDetailService.DeleteAccountDetail(id);
            return StatusCode(response.StatusCode);
        }
    }
}
