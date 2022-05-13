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

        [HttpPost("AddNewReceipt_v2")]
        public async Task<ApiCommonResponse> AddNewReceipt_v2(ReceiptReceivingDTO receiptReceiving)
        {
            return await _receiptService.AddNewReceipt_v2(receiptReceiving);
        }

        [HttpPost("PostPaymentDetails")]
        public async Task<ApiCommonResponse> PostPaymentDetails(PaymentDetailsDTO paymentDetails)
        {
            return await _receiptService.PostPaymentDetails(paymentDetails);
        }

        [HttpGet("GetInvoices")]
        public async Task<ApiCommonResponse> GetInvoices(int userId)
        {
            return await _invoiceService.GetInvoices(userId);
        }

        [HttpGet("GetInvoice")]
        public async Task<ApiCommonResponse> GetInvoice(string invoiceNumber, DateTime invoiceDate)
        {
            return await _invoiceService.GetInvoice(invoiceNumber, invoiceDate);
        }

        [HttpGet("CheckIfInvoiceHasBeenPaid")]
        public async Task<ApiCommonResponse> CheckIfInvoiceHasBeenPaid(string invoiceNumber, string sessionId, int userId)
        {
            return await _invoiceService.CheckIfInvoiceHasBeenPaid(invoiceNumber, sessionId, userId);
        }

    }
}
