using HalobizMigrations.Models;
using Microsoft.AspNetCore.Http;
using OnlinePortalBackend.DTOs.ApiDTOs;
using OnlinePortalBackend.DTOs.ReceivingDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePortalBackend.MyServices
{
    public interface ICartService
    {
        Task<ApiResponse> PostCartItemsForSuspect(HttpContext context, CartItemsReceivingDTO cartItemsReceivingDTO, long prospectId);
        Task<ApiResponse> PostCartItemsForExistingCustomer(HttpContext context, CartItemsReceivingDTO cartItemsReceivingDTO, long customerDivisionId);
        Task<ApiResponse> CompletePayment(HttpContext context, CompletePaymentReceivingDTO completePaymentReceivingDTO);
        Task<ApiResponse> AcceptPayment(HttpContext context, AcceptPaymentReceivingDTO completePaymentReceivingDTO);
        Task<ApiResponse> CompletePaymentForExistingCustomer(HttpContext context, CompletePaymentReceivingDTO completePaymentReceivingDTO);
        Task<ApiResponse> GetInvoices(HttpContext context, long prospectId);
        Task<bool> ReceiptInvoices(QuoteService quoteService, long loggedInUserId);
        Task<ApiResponse> GetProformaInvoices(long prospectId, long quoteId);
        Task<ApiResponse> GetQuoteServices(HttpContext context, long prospectId);
        Task<ApiResponse> GetQuoteServicesPaymentStatus(HttpContext context, long prospectId);
        Task<ApiResponse> ConfirmPayment(string transactionRef);
    }
}
