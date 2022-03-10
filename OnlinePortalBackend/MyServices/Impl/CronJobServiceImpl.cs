using HalobizMigrations.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnlinePortalBackend.DTOs.ApiDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OnlinePortalBackend.MyServices.Impl
{
    public class CronJobServiceImpl : ICronJobService
    {
        private readonly HalobizContext _context;
       // private readonly ILeadConversionService _leadConversionService;
        private readonly ICartService _cartService;
        private readonly ILogger<CronJobServiceImpl> _logger;

        public CronJobServiceImpl(HalobizContext context,
            ILogger<CronJobServiceImpl> logger,
          //  ILeadConversionService leadConversionService,
            ICartService cartService)
        {
            _context = context;
           // _leadConversionService = leadConversionService;
            _cartService = cartService;
            _logger = logger;
        }

        public async Task<ApiResponse> RetryPaymentProcessing()
        {
            var payments = await _context.Payments
                                    .Where(x => x.PaymentValidated && (!x.CustomerHasReceivedValue || !x.IsReceipted)
                                                && !x.IsDeleted)
                                    .ToListAsync();

            foreach (var payment in payments)
            {
                var prospect = await _context.Prospects.FirstOrDefaultAsync(x => x.Id == payment.ProspectId);

                if(prospect == null)
                {
                    _logger.LogInformation($"Payment with reference {payment.PaymentReference} has no prospect");
                    continue;
                }

                var quote = await _context.Quotes
                                         .Include(x => x.QuoteServices.Where(x => !x.IsDeleted))
                                         .Where(x => x.Id == payment.QuoteId)
                                         .FirstOrDefaultAsync();

                var quoteServices = quote.QuoteServices;

                var singleQuoteService = quoteServices.FirstOrDefault();

                if (!payment.CustomerHasReceivedValue)
                {
                    //var conversionResult = await _leadConversionService.ConvertLeadToClient((long)prospect.LeadId, (long)payment.QuoteId, payment.CreatedById);
                    //if (!conversionResult.conversionSuccessful)
                    //{
                    //    _logger.LogInformation($"Payment with reference {payment.PaymentReference}, Lead Id => {prospect.LeadId}, Quote Id => {payment.QuoteId} could not be converted.");
                    //    continue;
                    //}

                    //prospect.CustomerId = conversionResult.customerId;
                    _context.Prospects.Update(prospect);
                    await _context.SaveChangesAsync();
                }         

                payment.CustomerHasReceivedValue = true;

                var receiptingSuccessful = await _cartService.ReceiptInvoices(singleQuoteService, payment.CreatedById);
                if (!receiptingSuccessful)
                {
                    _context.Payments.Update(payment);
                    await _context.SaveChangesAsync();
                    return new ApiResponse(500, "Could not process receipting");
                }

                payment.IsReceipted = true;

                _context.Payments.Update(payment);
                await _context.SaveChangesAsync();
            }

            return new ApiOkResponse(true);
        }
    }
}
