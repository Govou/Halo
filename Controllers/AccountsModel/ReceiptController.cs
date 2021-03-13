using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.MyServices;
using Microsoft.AspNetCore.Mvc;

namespace HaloBiz.Controllers.AccountsModel
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ReceiptController : ControllerBase
    {
        private readonly IReceiptService _receiptService;

        public ReceiptController(IReceiptService receiptService)
        {
            this._receiptService = receiptService;
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNewReceipt(ReceiptReceivingDTO receiptReceiving)
        {
            var response = await _receiptService.AddReceipt(HttpContext, receiptReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var receipt = ((ApiOkResponse)response).Result;
            return Ok(receipt);
        }
    }
}