using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.MyServices;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HaloBiz.Controllers.AccountsModel
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ControlAccount : ControllerBase
    {
        private readonly IControlAccountService _controlAccountService;
        public ControlAccount(IControlAccountService controlAccountService)
        {
            this._controlAccountService = controlAccountService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetControlAccounts()
        {
            return await _controlAccountService.GetAllControlAccounts();
        }

        [HttpGet("GetIncomeControlAccounts")]
        public async Task<ApiCommonResponse> GetIncomeControlAccounts()
        {
            return await _controlAccountService.GetAllIncomeControlAccounts();
        }

        [HttpGet("alias/{alias}")]
        public async Task<ApiCommonResponse> GetByAlias(string alias)
        {
            return await _controlAccountService.GetControlAccountByAlias(alias);
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _controlAccountService.GetControlAccountById(id);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewAccount(ControlAccountReceivingDTO controlAccountReceiving)
        {
            return await _controlAccountService.AddControlAccount(HttpContext, controlAccountReceiving);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, ControlAccountReceivingDTO controlAccountReceiving)
        {
            return await _controlAccountService.UpdateControlAccount(id, controlAccountReceiving);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _controlAccountService.DeleteControlAccount(id);
        }
    }
}