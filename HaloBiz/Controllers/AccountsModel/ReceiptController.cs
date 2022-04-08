using System.Threading.Tasks;
using Halobiz.Common.Auths;
using Halobiz.Common.DTOs.ApiDTOs;
using Halobiz.Common.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.MyServices;
using Microsoft.AspNetCore.Mvc;

namespace HaloBiz.Controllers.AccountsModel
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ModuleName(HalobizModules.Finance)]

    public class ReceiptController : ControllerBase
    {
        private readonly IReceiptService _receiptService;

        public ReceiptController(IReceiptService receiptService)
        {
            this._receiptService = receiptService;
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewReceipt(ReceiptReceivingDTO receiptReceiving)
        {
            return await _receiptService.AddReceipt(HttpContext, receiptReceiving);
        }

        [HttpGet("ReceiptBreakDown/{invoiceId}/{totalReceiptAmount}")]
        public async Task<ApiCommonResponse> GetReceiptBreakDown(long invoiceId, double totalReceiptAmount)
        {
            return await _receiptService.GetReceiptBreakDown(invoiceId, totalReceiptAmount);
        }
    }
}