using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlinePortalBackend.DTOs.ApiDTOs;
using OnlinePortalBackend.DTOs.ReceivingDTOs;
using OnlinePortalBackend.DTOs.TransferDTOs;
using OnlinePortalBackend.MyServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("PostCartItemsForProspect/{prospectId}")]
        public async Task<ActionResult> PostCartItemsForProspect(CartItemsReceivingDTO controlRoomAlertReceiving, long prospectId)
        {
            var response = await _cartService.PostCartItemsForSuspect(HttpContext, controlRoomAlertReceiving, prospectId);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var theResponse = ((ApiOkResponse)response).Result;
            return Ok(theResponse);
        }

        [HttpPost("PostCartItemsForExistingCustomer/{customerDivisionId}")]
        public async Task<ActionResult> PostCartItemsForExistingCustomer(CartItemsReceivingDTO controlRoomAlertReceiving, long customerDivisionId)
        {
            var response = await _cartService.PostCartItemsForExistingCustomer(HttpContext, controlRoomAlertReceiving, customerDivisionId);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var theResponse = ((ApiOkResponse)response).Result;
            return Ok(theResponse);
        }

        [HttpPost("CompletePayment")]
        public async Task<ActionResult> CompletePayment(CompletePaymentReceivingDTO completePaymentReceivingDTO)
        {
            var response = await _cartService.CompletePayment(HttpContext, completePaymentReceivingDTO);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var theResponse = ((ApiOkResponse)response).Result;
            return Ok(theResponse);
        }

        [HttpPost("CompletePaymentForExistingCustomer")]
        public async Task<ActionResult> CompletePaymentForExistingCustomer(CompletePaymentReceivingDTO completePaymentReceivingDTO)
        {
            var response = await _cartService.CompletePaymentForExistingCustomer(HttpContext, completePaymentReceivingDTO);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var theResponse = ((ApiOkResponse)response).Result;
            return Ok(theResponse);
        }

        [HttpPost("AcceptPayment")]
        public async Task<ActionResult> AcceptPayment(AcceptPaymentReceivingDTO acceptPaymentReceivingDTO)
        {
            var response = await _cartService.AcceptPayment(HttpContext, acceptPaymentReceivingDTO);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var theResponse = ((ApiOkResponse)response).Result;
            return Ok(theResponse);
        }

        [HttpGet("GetProformaInvoices/{prospectId}/{quoteId}")]
        public async Task<ActionResult> GetProformaInvoices(long prospectId, long quoteId)
        {
            var response = await _cartService.GetProformaInvoices(prospectId, quoteId);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var theResponse = ((ApiOkResponse)response).Result;
            return Ok(theResponse);
        }

        [HttpGet("GetInvoices/{prospectId}")]
        public async Task<ActionResult> GetInvoices(long prospectId)
        {
            var response = await _cartService.GetInvoices(HttpContext, prospectId);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var theResponse = ((ApiOkResponse)response).Result;
            return Ok(theResponse);
        }

        [HttpGet("GetQuoteServices/{prospectId}")]
        public async Task<ActionResult> GetQuoteServices(long prospectId)
        {
            var response = await _cartService.GetQuoteServices(HttpContext, prospectId);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var theResponse = ((ApiOkResponse)response).Result;
            return Ok(theResponse);
        }

        [HttpGet("GetQuotesPaymentStatus/{prospectId}")]
        public async Task<ActionResult> GetQuoteServicesPaymentStatus(long prospectId)
        {
            var response = await _cartService.GetQuoteServicesPaymentStatus(HttpContext, prospectId);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var theResponse = ((ApiOkResponse)response).Result;
            return Ok(theResponse);
        }

        [HttpGet("GetPaymentConfirmation/{paymentRef}")]
        public async Task<ActionResult> GetPaymentConfirmation(string paymentRef)
        {
            var response = await _cartService.ConfirmPayment(paymentRef);
           
            var theResponse = ((ApiOkResponse)response).Result;
            return Ok(theResponse);
        }

    }
}
