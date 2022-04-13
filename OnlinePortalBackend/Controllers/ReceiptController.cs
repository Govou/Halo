using Halobiz.Common.DTOs.ApiDTOs;
using Halobiz.Common.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlinePortalBackend.Adapters;
using OnlinePortalBackend.MyServices;
using OnlinePortalBackend.MyServices.Impl;
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
        private readonly IReceiptService _receiptService;
        private readonly IInvoiceService _invoiceService;
        public ReceiptController(IInvoiceService invoiceService, IReceiptService receiptService)
        {
            _invoiceService = invoiceService;
            _receiptService = receiptService;
        }
        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewReceipt(ReceiptReceivingDTO receiptReceiving)
        {
            return await _receiptService.AddNewReceipt(receiptReceiving);
        }

        [HttpGet("GetInvoices")]
        public async Task<ApiCommonResponse> GetInvoices(int userId)
        {
            return await _invoiceService.GetInvoices(userId);
        }

        [HttpGet("GetInvoice")]
        public async Task<ApiCommonResponse> GetInvoice(int invoiceId)
        {
            return await _invoiceService.GetInvoice(invoiceId);
        }

        //public async Task<ApiCommonResponse> CompletePaymentForReceipt(ReceiptReceivingDTO request)
        //{
            
        //}

    }
}
