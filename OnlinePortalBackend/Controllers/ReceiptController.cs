using Halobiz.Common.DTOs.ApiDTOs;
using Halobiz.Common.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlinePortalBackend.Adapters;
using OnlinePortalBackend.MyServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiptController : ControllerBase
    {
        private readonly IReceiptAdapter _receiptAdapter;
        private readonly IInvoiceService _invoiceService;
        public ReceiptController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
              
        }
        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewReceipt(ReceiptReceivingDTO receiptReceiving)
        {
            return await _receiptAdapter.AddReceipt(receiptReceiving);
        }

        [HttpGet("GetInvoices")]
        public async Task<ApiCommonResponse> GetInvoicesByContractService(int userId, int? contractServiceId, int? contractId, int limit = 10)
        {
            return await _invoiceService.GetInvoices(userId, contractServiceId, contractId, limit = 10);
        }

       
    }
}
