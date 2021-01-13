using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.MyServices.LAMS;
using Microsoft.AspNetCore.Mvc;

namespace HaloBiz.Controllers.LAMS
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FinanceVoucherTypeController : ControllerBase
    {
        private readonly IFinancialVoucherTypeService _finacialVoucherTypeService;

        public FinanceVoucherTypeController(IFinancialVoucherTypeService finacialVoucherTypeService)
        {
            this._finacialVoucherTypeService = finacialVoucherTypeService;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetFinancialVoucherTypes()
        {
            var response = await _finacialVoucherTypeService.GetAllFinancialVoucherTypes();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var voucherType = ((ApiOkResponse)response).Result;
            return Ok(voucherType);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var response = await _finacialVoucherTypeService.GetFinancialVoucherTypeById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var voucherType = ((ApiOkResponse)response).Result;
            return Ok(voucherType);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNewFinancialVoucherType(FinancialVoucherTypeReceivingDTO voucherTypeReceiving)
        {
            var response = await _finacialVoucherTypeService.AddFinancialVoucherType(HttpContext, voucherTypeReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var voucherType = ((ApiOkResponse)response).Result;
            return Ok(voucherType);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, FinancialVoucherTypeReceivingDTO voucherTypeReceiving)
        {
            var response = await _finacialVoucherTypeService.UpdateFinancialVoucherType(HttpContext, id, voucherTypeReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var voucherType = ((ApiOkResponse)response).Result;
            return Ok(voucherType);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var response = await _finacialVoucherTypeService.DeleteFinancialVoucherType(id);
            return StatusCode(response.StatusCode);
        }
    }
}