using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.Helpers;
using HaloBiz.MyServices;
using Microsoft.AspNetCore.Mvc;
    
namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BankController : Controller
    {
        private readonly IBankService _bankService;

        public BankController(IBankService bankService)
        {
            this._bankService = bankService;
        }

        //[ClaimRequirement(ClaimConstants.ClaimType, ClaimConstants.CAN_GET_BANKS)]
        [HttpGet("")]
        public async Task<ActionResult> GetBank()
        {
            var response = await _bankService.GetAllBank();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var bank = ((ApiOkResponse)response).Result;
            return Ok(bank);
        }

        //[ClaimRequirement(ClaimConstants.ClaimType, ClaimConstants.CAN_GET_BANKS)]
        [HttpGet("name/{name}")]
        public async Task<ActionResult> GetByCaption(string name)
        {
            var response = await _bankService.GetBankByName(name);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var bank = ((ApiOkResponse)response).Result;
            return Ok(bank);
        }

        //[ClaimRequirement(ClaimConstants.ClaimType, ClaimConstants.CAN_GET_BANKS)]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var response = await _bankService.GetBankById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var bank = ((ApiOkResponse)response).Result;
            return Ok(bank);
        }

        //[ClaimRequirement(ClaimConstants.ClaimType, ClaimConstants.CAN_UPDATE_BANK)]
        [HttpPost("")]
        public async Task<ActionResult> AddNewBank(BankReceivingDTO bankReceiving)
        {
            var response = await _bankService.AddBank(HttpContext, bankReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var bank = ((ApiOkResponse)response).Result;
            return Ok(bank);
        }

        //[ClaimRequirement(ClaimConstants.ClaimType, ClaimConstants.CAN_UPDATE_BANK)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, BankReceivingDTO bankReceiving)
        {
            var response = await _bankService.UpdateBank(HttpContext, id, bankReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var bank = ((ApiOkResponse)response).Result;
            return Ok(bank);
        }

        //[ClaimRequirement(ClaimConstants.ClaimType, ClaimConstants.CAN_DELETE_BANK)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var response = await _bankService.DeleteBank(id);
            return StatusCode(response.StatusCode);
        }
    }
}
