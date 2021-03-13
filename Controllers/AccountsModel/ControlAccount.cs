using HaloBiz.DTOs.ApiDTOs;
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
        public async Task<ActionResult> GetControlAccounts()
        {
            var response = await _controlAccountService.GetAllControlAccounts();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var controlAccount = ((ApiOkResponse)response).Result;
            return Ok(controlAccount);
        }

        [HttpGet("alias/{alias}")]
        public async Task<ActionResult> GetByAlias(string alias)
        {
            var response = await _controlAccountService.GetControlAccountByAlias(alias);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var controlAccount = ((ApiOkResponse)response).Result;
            return Ok(controlAccount);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var response = await _controlAccountService.GetControlAccountById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var controlAccount = ((ApiOkResponse)response).Result;
            return Ok(controlAccount);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNewAccount(ControlAccountReceivingDTO controlAccountReceiving)
        {
            var response = await _controlAccountService.AddControlAccount(HttpContext, controlAccountReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var controlAccount = ((ApiOkResponse)response).Result;
            return Ok(controlAccount);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, ControlAccountReceivingDTO controlAccountReceiving)
        {
            var response = await _controlAccountService.UpdateControlAccount(id, controlAccountReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var controlAccount = ((ApiOkResponse)response).Result;
            return Ok(controlAccount);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var response = await _controlAccountService.DeleteControlAccount(id);
            return StatusCode(response.StatusCode);
        }
    }
}