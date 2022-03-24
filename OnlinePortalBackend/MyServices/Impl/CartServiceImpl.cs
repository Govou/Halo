using HalobizMigrations.Data;
using HalobizMigrations.Models;
using HalobizMigrations.Models.Halobiz;
using HalobizMigrations.Models.OnlinePortal;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OnlinePortalBackend.Adapters;
using OnlinePortalBackend.DTOs.ApiDTOs;
using OnlinePortalBackend.DTOs.ReceivingDTOs;
using OnlinePortalBackend.DTOs.TransferDTOs;
using OnlinePortalBackend.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OnlinePortalBackend.MyServices.Impl
{
    public class CartServiceImpl : ICartService
    {
        private readonly HalobizContext _context;
        private readonly ILogger<CartServiceImpl> _logger;
        private readonly IHttpClientFactory _httpClient;
        private readonly IPaymentAdapter _paymentAdapter;
        private readonly IReceiptService _receiptService;
        private readonly string _haloBizServiceUrl;

        public CartServiceImpl(
            HalobizContext context,
            IPaymentAdapter paymentAdapter,
            ILogger<CartServiceImpl> logger,
            IHttpClientFactory httpClient,
            IConfiguration config,
            IReceiptService receiptService)
        {
            _context = context;
            _logger = logger;
            _httpClient = httpClient;
            _paymentAdapter = paymentAdapter;
            _receiptService = receiptService;

            _haloBizServiceUrl = config["HaloBizUrl"] ?? config.GetSection("ServicesUrl:HaloBizUrl").Value;
        }

        public async Task<ApiResponse> PostCartItemsForSuspect(
            HttpContext context, 
            CartItemsReceivingDTO cartItemsReceivingDTO,
            long prospectId)
        {
            var prospect = await _context.Prospects.Where(x => x.Id == prospectId && !x.IsDeleted).FirstOrDefaultAsync();

            if(prospect == null)
            {
                return new ApiResponse(404, "Prospect not found.");
            }

            var emailLeadOrigin = await _context.LeadOrigins.SingleOrDefaultAsync(x => x.Caption.ToLower() == "email");
            var rfqLeadType = await _context.LeadTypes.SingleOrDefaultAsync(x => x.Caption.ToLower() == "rfq");
            var coporateGroupType = await _context.GroupTypes.SingleOrDefaultAsync(x => x.Caption.ToLower() == "corporate");

            try
            {
                using var client = _httpClient.CreateClient();
                client.BaseAddress = new Uri(_haloBizServiceUrl);

                string groupInvoiceNumber = string.Empty;
                var quoteServices = new List<QuoteService>();
                var invoices = new List<ProformaInvoice>();

                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    var loggedInUserId = context.GetLoggedInUserId();

                    LeadDivision savedLeadDivision = null;

                    if(prospect.LeadId == null)
                    {
                        //var theGroupType = await _context.GroupTypes.SingleOrDefaultAsync(x => x.Caption.ToLower() == prospect.ProspectType.ToString().ToLower());

                        var lead = new Lead
                        {
                            GroupName = prospect.CompanyName,
                            CreatedById = loggedInUserId,
                            GroupTypeId = coporateGroupType.Id,
                            LeadOriginId = emailLeadOrigin.Id,
                            LeadTypeId = rfqLeadType.Id,
                            Rcnumber = prospect.RCNumber
                        };

                        var leadEntry = await _context.Leads.AddAsync(lead);
                        await _context.SaveChangesAsync();

                        var leadDivision = new LeadDivision
                        {
                            LeadId = leadEntry.Entity.Id,
                            DivisionName = prospect.CompanyName,
                            Address = prospect.Address,
                            BranchId = prospect.BranchId ?? 1,
                            CreatedById = loggedInUserId,
                            Email = prospect.Email,
                            Industry = prospect.Industry,
                            LeadOriginId = emailLeadOrigin.Id,
                            LeadTypeId = rfqLeadType.Id,
                            Lgaid = prospect.Lgaid,
                            LogoUrl = prospect.LogoUrl,
                            OfficeId = prospect.OfficeId ?? 1,
                            PhoneNumber = prospect.MobileNumber,
                            StateId = prospect.StateId,
                            Street = prospect.Street,
                            Rcnumber = prospect.RCNumber
                        };

                        var leadDivisionEntry = await _context.LeadDivisions.AddAsync(leadDivision);
                        await _context.SaveChangesAsync();
                        savedLeadDivision = leadDivisionEntry.Entity;

                        prospect.LeadId = leadEntry.Entity.Id;
                        _context.Prospects.Update(prospect);
                    }
                    else
                    {
                        savedLeadDivision = await _context.LeadDivisions.Where(x => x.LeadId == prospect.LeadId && !x.IsDeleted)
                                                            .FirstOrDefaultAsync();
                    }

                    var quote = new Quote
                    {
                        LeadDivisionId = savedLeadDivision.Id,
                        CreatedById = loggedInUserId
                    };

                    var quoteEntry = await _context.Quotes.AddAsync(quote);
                    await _context.SaveChangesAsync();

                    var response1 = await client.GetAsync("GenerateGroupInvoiceNumber");
                    if (response1.IsSuccessStatusCode)
                    {
                        var res = await response1.Content.ReadAsStringAsync();
                        groupInvoiceNumber = res;
                    }
                    else
                    {
                        await transaction.RollbackAsync();
                        var res = await response1.Content.ReadAsStringAsync();
                        _logger.LogError($"The call to generate group invoice number failed on {DateTime.Now} failed.");
                        _logger.LogError($"Error => {res}");
                        return new ApiResponse(500, res);
                    }

                    foreach (var cartItem in cartItemsReceivingDTO.CartItems)
                    {
                        var service = await _context.Services.FindAsync(cartItem.Product.ServiceId);

                        //var contractStartDate = DateTime.Parse((string)cartItem.FormData["contractStartDate"]);
                        //var contractEndDate = DateTime.Parse((string)cartItem.FormData["contractEndDate"]);
                        //var fulfillmentStartDate = DateTime.Parse((string)cartItem.FormData["fulfillmentStartDate"]);
                        //var fulfillmentEndDate = DateTime.Parse((string)cartItem.FormData["fulfillmentEndDate"]);
                        //var activationDate = DateTime.Parse((string)cartItem.FormData["activationDate"]);
                        //var firstInvoiceSendDate = DateTime.Parse((string)cartItem.FormData["firstInvoiceSendDate"]);

                        var quoteService = new QuoteService
                        {
                            QuoteId = quoteEntry.Entity.Id,
                            Quantity = cartItem.Quantity,
                            ServiceId = cartItem.Product.ServiceId,
                            BillableAmount = cartItem.Total,
                            UnitPrice = service.UnitPrice,
                            Budget = cartItem.Total,
                            Discount = 0,
                            Vat = cartItem.Vat,
                            //ContractStartDate = contractStartDate,
                            //ContractEndDate = contractEndDate,
                            //FulfillmentStartDate = fulfillmentStartDate,
                            //FulfillmentEndDate = fulfillmentEndDate,
                            //ActivationDate = activationDate,
                            //FirstInvoiceSendDate = firstInvoiceSendDate,
                            InvoicingInterval = 2, // Monthly
                            PaymentCycle = 2,
                            //GroupInvoiceNumber = groupInvoiceNumber,
                            CreatedById = loggedInUserId
                        };
                        quoteServices.Add(quoteService);
                    }

                    await _context.QuoteServices.AddRangeAsync(quoteServices);                    
                    await _context.SaveChangesAsync();

                    foreach (var quoteService in quoteServices)
                    {
                        var invoice = new ProformaInvoice
                        {
                            Value = (double)quoteService.BillableAmount,
                            GroupInvoiceNumber = groupInvoiceNumber,
                            StartDate = DateTime.Now,
                            EndDate = DateTime.Now.AddMonths(1),
                            DateToBeSent = DateTime.Now.AddHours(1),
                            QuoteId = quoteEntry.Entity.Id,
                            QuoteServiceId = quoteService.Id,
                            LeadDivisionId = savedLeadDivision.Id,
                            UnitPrice = (double)quoteService.UnitPrice,
                            Discount = quoteService.Discount,
                            IsAccountPosted = false,
                            IsInvoiceSent = false,
                            Quantity = quoteService.Quantity
                        };

                        invoices.Add(invoice);
                    }

                    await _context.ProformaInvoices.AddRangeAsync(invoices);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(ex.Message);
                    _logger.LogError(ex.StackTrace);
                    return new ApiResponse(500, ex.Message);
                }                

                return new ApiOkResponse(true);
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);              
                return new ApiResponse(500, ex.Message);
            }
        }

        public async Task<ApiResponse> PostCartItemsForExistingCustomer(
            HttpContext context,
            CartItemsReceivingDTO cartItemsReceivingDTO,
            long customerDivisionId)
        {
            var customerDivision = await _context.CustomerDivisions.Where(x => x.Id == customerDivisionId && !x.IsDeleted)
                                                            .FirstOrDefaultAsync();

            if (customerDivision == null)
            {
                return new ApiResponse(404, "Customer division not found.");
            }

            try
            {
                using var client = _httpClient.CreateClient();
                client.BaseAddress = new Uri(_haloBizServiceUrl);

                string groupInvoiceNumber = string.Empty;
                var quoteServices = new List<QuoteService>();
                var invoices = new List<ProformaInvoice>();

                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    var loggedInUserId = context.GetLoggedInUserId();

                    LeadDivision savedLeadDivision = await _context.LeadDivisions.Where(x => x.DivisionName == customerDivision.DivisionName && !x.IsDeleted)
                                                                        .FirstOrDefaultAsync();
                    var quote = new Quote
                    {
                        LeadDivisionId = savedLeadDivision.Id,
                        CreatedById = loggedInUserId
                    };

                    var quoteEntry = await _context.Quotes.AddAsync(quote);
                    await _context.SaveChangesAsync();

                    var response1 = await client.GetAsync("GenerateGroupInvoiceNumber");
                    if (response1.IsSuccessStatusCode)
                    {
                        var res = await response1.Content.ReadAsStringAsync();
                        groupInvoiceNumber = res;
                    }
                    else
                    {
                        await transaction.RollbackAsync();
                        var res = await response1.Content.ReadAsStringAsync();
                        _logger.LogError($"The call to generate group invoice number failed on {DateTime.Now} failed.");
                        _logger.LogError($"Error => {res}");
                        return new ApiResponse(500, res);
                    }

                    foreach (var cartItem in cartItemsReceivingDTO.CartItems)
                    {
                        var service = await _context.Services.FindAsync(cartItem.Product.ServiceId);

                        //var contractStartDate = DateTime.Parse((string)cartItem.FormData["contractStartDate"]);
                        //var contractEndDate = DateTime.Parse((string)cartItem.FormData["contractEndDate"]);
                        //var fulfillmentStartDate = DateTime.Parse((string)cartItem.FormData["fulfillmentStartDate"]);
                        //var fulfillmentEndDate = DateTime.Parse((string)cartItem.FormData["fulfillmentEndDate"]);
                        //var activationDate = DateTime.Parse((string)cartItem.FormData["activationDate"]);
                        //var firstInvoiceSendDate = DateTime.Parse((string)cartItem.FormData["firstInvoiceSendDate"]);

                        var quoteService = new QuoteService
                        {
                            QuoteId = quoteEntry.Entity.Id,
                            Quantity = cartItem.Quantity,
                            ServiceId = cartItem.Product.ServiceId,
                            BillableAmount = cartItem.Total,
                            UnitPrice = service.UnitPrice,
                            Budget = cartItem.Total,
                            Discount = 0,
                            Vat = cartItem.Vat,
                            //ContractStartDate = contractStartDate,
                            //ContractEndDate = contractEndDate,
                            //FulfillmentStartDate = fulfillmentStartDate,
                            //FulfillmentEndDate = fulfillmentEndDate,
                            //ActivationDate = activationDate,
                            //FirstInvoiceSendDate = firstInvoiceSendDate,
                            InvoicingInterval = 2, // Monthly
                            PaymentCycle = 2,
                            //GroupInvoiceNumber = groupInvoiceNumber,
                            CreatedById = loggedInUserId
                        };
                        quoteServices.Add(quoteService);
                    }

                    await _context.QuoteServices.AddRangeAsync(quoteServices);
                    await _context.SaveChangesAsync();

                    foreach (var quoteService in quoteServices)
                    {
                        var invoice = new ProformaInvoice
                        {
                            Value = (double)quoteService.BillableAmount,
                            GroupInvoiceNumber = groupInvoiceNumber,
                            StartDate = DateTime.Now,
                            EndDate = DateTime.Now.AddMonths(1),
                            DateToBeSent = DateTime.Now.AddHours(1),
                            QuoteId = quoteEntry.Entity.Id,
                            QuoteServiceId = quoteService.Id,
                            LeadDivisionId = savedLeadDivision.Id,
                            UnitPrice = (double)quoteService.UnitPrice,
                            Discount = quoteService.Discount,
                            IsAccountPosted = false,
                            IsInvoiceSent = false,
                            Quantity = quoteService.Quantity
                        };

                        invoices.Add(invoice);
                    }

                    await _context.ProformaInvoices.AddRangeAsync(invoices);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(ex.Message);
                    _logger.LogError(ex.StackTrace);
                    return new ApiResponse(500, ex.Message);
                }

                return new ApiOkResponse(true);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                return new ApiResponse(500, ex.Message);
            }
        }

        public async Task<ApiResponse> CompletePayment(HttpContext context, CompletePaymentReceivingDTO completePaymentReceivingDTO)
        {
            var loggedInUserId = context.GetLoggedInUserId();

            try
            {
                var referenceCode = completePaymentReceivingDTO.ReferenceCode;

                if (await _context.Payments.AnyAsync(x => x.PaymentReference == referenceCode && x.PaymentValidated))
                    return new ApiResponse(400, $"Payment with reference {referenceCode} has already been processed.");

                #region Get Quote Services Info
                var prospectId = completePaymentReceivingDTO.ProspectId;

                var prospect = await _context.Prospects.FindAsync(prospectId);

                /*var leadDivision = await _context.LeadDivisions
                                                .Where(x => x.LeadId == prospect.LeadId && !x.IsDeleted)
                                                .FirstOrDefaultAsync();

                var quote = await _context.Quotes
                                         .Where(x => x.LeadDivisionId == leadDivision.Id && !x.IsConvertedToContract && !x.IsDeleted)
                                         .FirstOrDefaultAsync();*/

                var quoteId = completePaymentReceivingDTO.QuoteId;

                var quote = await _context.Quotes
                                         .Include(x => x.QuoteServices.Where(x => !x.IsDeleted))
                                         .Where(x => x.Id == quoteId)
                                         .FirstOrDefaultAsync();

                if (quote == null) return new ApiResponse(500, $"Quote {quoteId} does not exist.");

                var quoteServices = quote.QuoteServices;

                var singleQuoteService = quoteServices.FirstOrDefault();

               // string groupInvoiceNumber = singleQuoteService.GroupInvoiceNumber;

                if (quote.IsConvertedToContract)
                {
                    return new ApiResponse(404, "Invalid payment. Quote already converted to contract.");
                }

                var totalPrice = quoteServices.Sum(x => (double)x.BillableAmount);
                #endregion

                var paymentType = completePaymentReceivingDTO.PaymentType;

                var verificationResponse = await _paymentAdapter.VerifyPaymentAsync(paymentType, referenceCode);

                var payment = new Payment 
                {
                    Amount = verificationResponse.PaymentAmount,
                    CreatedById = loggedInUserId,
                    PaymentGateway = paymentType,
                    PaymentReference = referenceCode,
                    ProspectId = prospectId,
                    QuoteId = quoteId
                };

                string errorMessage = string.Empty;

                if (!verificationResponse.PaymentSuccessful) 
                {
                    errorMessage = string.Join("\n", verificationResponse.Errors);

                    payment.PaymentValidated = false;
                    payment.CustomerHasReceivedValue = false;
                    payment.IsReceipted = false;

                    await _context.Payments.AddAsync(payment);
                    await _context.SaveChangesAsync();
                    return new ApiResponse(400, errorMessage);
                }

                if (verificationResponse.PaymentAmount < (decimal)totalPrice)
                {
                    errorMessage = $"Amount paid {verificationResponse.PaymentAmount} is less than total quote services amount {totalPrice}.";

                    payment.PaymentValidated = false;
                    payment.CustomerHasReceivedValue = false;
                    payment.IsReceipted = false;

                    await _context.Payments.AddAsync(payment);
                    await _context.SaveChangesAsync();
                    return new ApiResponse(400, errorMessage);
                }

                payment.PaymentValidated = true;

                var conversionResult = await ConvertPropsectToClientV2(prospect, quoteId, loggedInUserId);
                if (!conversionResult.conversionSuccessful)
                {
                    await _context.Payments.AddAsync(payment);
                    await _context.SaveChangesAsync();
                    return new ApiResponse(500, "Could not process lead conversion");
                }

                prospect.CustomerId = conversionResult.customerId;
                _context.Prospects.Update(prospect);
                await _context.SaveChangesAsync();

                payment.CustomerHasReceivedValue = true;
             
                var receiptingSuccessful = await ReceiptInvoices(singleQuoteService, loggedInUserId);
                if (!receiptingSuccessful)
                {
                    await _context.Payments.AddAsync(payment);
                    await _context.SaveChangesAsync();
                    return new ApiResponse(500, "Could not process receipting");
                }

                payment.IsReceipted = true;
                await _context.Payments.AddAsync(payment);
                await _context.SaveChangesAsync();

                return new ApiOkResponse("Quote payment processed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                return new ApiResponse(500, ex.Message);
            }
        }

        public async Task<ApiResponse> AcceptPayment(HttpContext context, AcceptPaymentReceivingDTO acceptPaymentReceivingDTO)
        {
            var loggedInUserId = context.GetLoggedInUserId();

            try
            {
                var referenceCode = acceptPaymentReceivingDTO.ReferenceCode;

                if (await _context.Payments.AnyAsync(x => x.PaymentReference == referenceCode && x.PaymentValidated))
                    return new ApiResponse(400, $"Payment with reference {referenceCode} has already been processed.");

                #region Get Quote Services Info
                /*var prospectId = completePaymentReceivingDTO.ProspectId;

                var prospect = await _context.Prospects.FindAsync(prospectId);

                *//*var leadDivision = await _context.LeadDivisions
                                                .Where(x => x.LeadId == prospect.LeadId && !x.IsDeleted)
                                                .FirstOrDefaultAsync();

                var quote = await _context.Quotes
                                         .Where(x => x.LeadDivisionId == leadDivision.Id && !x.IsConvertedToContract && !x.IsDeleted)
                                         .FirstOrDefaultAsync();*//*

                var quoteId = completePaymentReceivingDTO.QuoteId;

                var quote = await _context.Quotes
                                         .Include(x => x.QuoteServices.Where(x => !x.IsDeleted))
                                         .Where(x => x.Id == quoteId)
                                         .FirstOrDefaultAsync();

                if (quote == null) return new ApiResponse(500, $"Quote {quoteId} does not exist.");

                var quoteServices = quote.QuoteServices;

                var singleQuoteService = quoteServices.FirstOrDefault();

                string groupInvoiceNumber = singleQuoteService.GroupInvoiceNumber;

                if (quote.IsConvertedToContract)
                {
                    return new ApiResponse(404, "Invalid payment. Quote already converted to contract.");
                }

                var totalPrice = quoteServices.Sum(x => (double)x.BillableAmount);*/
                #endregion

                #region Verify Payment
                var paymentType = acceptPaymentReceivingDTO.PaymentType;

                var verificationResponse = await _paymentAdapter.VerifyPaymentAsync(paymentType, referenceCode);

                var payment = new Payment
                {
                    Amount = verificationResponse.PaymentAmount,
                    CreatedById = loggedInUserId,
                    PaymentGateway = paymentType,
                    PaymentReference = referenceCode,
                    //ProspectId = prospectId,
                    //QuoteId = quoteId
                };

                string errorMessage = string.Empty;

                if (!verificationResponse.PaymentSuccessful)
                {
                    errorMessage = string.Join("\n", verificationResponse.Errors);

                    payment.PaymentValidated = false;
                    payment.CustomerHasReceivedValue = false;
                    payment.IsReceipted = false;

                    await _context.Payments.AddAsync(payment);
                    await _context.SaveChangesAsync();
                    return new ApiResponse(400, errorMessage);
                }

                if (verificationResponse.PaymentAmount < (decimal)0)
                {
                    errorMessage = $"Amount paid {verificationResponse.PaymentAmount} is less than total quote services amount {0}.";

                    payment.PaymentValidated = false;
                    payment.CustomerHasReceivedValue = false;
                    payment.IsReceipted = false;

                    await _context.Payments.AddAsync(payment);
                    await _context.SaveChangesAsync();
                    return new ApiResponse(400, errorMessage);
                }

                payment.PaymentValidated = true;
                #endregion

                /*var conversionResult = await ConvertPropsectToClientV2(prospect, quoteId, loggedInUserId);
                if (!conversionResult.conversionSuccessful)
                {
                    await _context.Payments.AddAsync(payment);
                    await _context.SaveChangesAsync();
                    return new ApiResponse(500, "Could not process lead conversion");
                }

                prospect.CustomerId = conversionResult.customerId;
                _context.Prospects.Update(prospect);
                await _context.SaveChangesAsync();

                payment.CustomerHasReceivedValue = true;

                var receiptingSuccessful = await ReceiptInvoices(singleQuoteService, loggedInUserId);
                if (!receiptingSuccessful)
                {
                    await _context.Payments.AddAsync(payment);
                    await _context.SaveChangesAsync();
                    return new ApiResponse(500, "Could not process receipting");
                }

                payment.IsReceipted = true;*/
                await _context.Payments.AddAsync(payment);
                await _context.SaveChangesAsync();

                return new ApiOkResponse("Quote payment processed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                return new ApiResponse(500, ex.Message);
            }
        }

        public async Task<ApiResponse> CompletePaymentForExistingCustomer(HttpContext context, CompletePaymentReceivingDTO completePaymentReceivingDTO)
        {
            var loggedInUserId = context.GetLoggedInUserId();

            try
            {
                var referenceCode = completePaymentReceivingDTO.ReferenceCode;

                if (await _context.Payments.AnyAsync(x => x.PaymentReference == referenceCode && x.PaymentValidated))
                    return new ApiResponse(400, $"Payment with reference {referenceCode} has already been processed.");

                #region Get Total Amount To Pay
                var quoteId = completePaymentReceivingDTO.QuoteId;

                var quote = await _context.Quotes
                                         .Include(x => x.QuoteServices.Where(x => !x.IsDeleted))
                                         .Where(x => x.Id == quoteId)
                                         .FirstOrDefaultAsync();

                if (quote == null) return new ApiResponse(500, $"Quote {quoteId} does not exist.");

                var quoteServices = quote.QuoteServices;

                var singleQuoteService = quoteServices.FirstOrDefault();

               // string groupInvoiceNumber = singleQuoteService.GroupInvoiceNumber;

                if (quote.IsConvertedToContract)
                {
                    return new ApiResponse(404, "Invalid payment. Quote already converted to contract.");
                }

                var totalPrice = quoteServices.Sum(x => (double)x.BillableAmount);
                #endregion

                var paymentType = completePaymentReceivingDTO.PaymentType;

                var verificationResponse = await _paymentAdapter.VerifyPaymentAsync(paymentType, referenceCode);

                var payment = new Payment
                {
                    Amount = verificationResponse.PaymentAmount,
                    CreatedById = loggedInUserId,
                    PaymentGateway = paymentType,
                    PaymentReference = referenceCode,
                    QuoteId = quoteId
                };

                string errorMessage = string.Empty;

                if (!verificationResponse.PaymentSuccessful)
                {
                    errorMessage = string.Join("\n", verificationResponse.Errors);

                    payment.PaymentValidated = false;
                    payment.CustomerHasReceivedValue = false;
                    payment.IsReceipted = false;

                    await _context.Payments.AddAsync(payment);
                    await _context.SaveChangesAsync();
                    return new ApiResponse(400, errorMessage);
                }

                if (verificationResponse.PaymentAmount < (decimal)totalPrice)
                {
                    errorMessage = $"Amount paid {verificationResponse.PaymentAmount} is less than total quote services amount {totalPrice}.";

                    payment.PaymentValidated = false;
                    payment.CustomerHasReceivedValue = false;
                    payment.IsReceipted = false;

                    await _context.Payments.AddAsync(payment);
                    await _context.SaveChangesAsync();
                    return new ApiResponse(400, errorMessage);
                }

                payment.PaymentValidated = true;
                payment.CustomerHasReceivedValue = true;

                var receiptingSuccessful = await ReceiptInvoices(singleQuoteService, loggedInUserId);
                if (!receiptingSuccessful)
                {
                    await _context.Payments.AddAsync(payment);
                    await _context.SaveChangesAsync();
                    return new ApiResponse(500, "Could not process receipting");
                }

                payment.IsReceipted = true;
                await _context.Payments.AddAsync(payment);
                await _context.SaveChangesAsync();

                return new ApiOkResponse("Quote payment processed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                return new ApiResponse(500, ex.Message);
            }
        }

        private async Task<(bool conversionSuccessful, long? customerId)> ConvertPropsectToClientV2(Prospect prospect, long quoteId, long loggedInUserId)
        {
            try
            {
                //var result = await _leadConversionService.ConvertLeadToClient((long)prospect.LeadId, quoteId, loggedInUserId);

                //if (result.conversionSuccessful)
                //{
                //    _logger.LogInformation($"{DateTime.Now}: Lead {prospect.LeadId} converted successfully.");
                //}
                //else
                //{
                //    _logger.LogError($"{DateTime.Now}: Lead {prospect.LeadId} conversion failed.");
                //}

                //return result;
                return (false, null);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogTrace(ex.StackTrace);
                return (false, null);
            }         
        }

        public async Task<bool> ReceiptInvoices(QuoteService singleQuoteService, long loggedInUserId)
        {
            try
            {
                var groupInNumber = singleQuoteService?.Quote.GroupInvoiceNumber;
                var invoices = await _context.Invoices
                    .Where(x => x.GroupInvoiceNumber == groupInNumber && x.StartDate == singleQuoteService.ContractStartDate)
                    .ToListAsync();

                foreach (var invoice in invoices)
                {
                    var receipt = new Receipt
                    {
                        InvoiceId = invoice.Id,
                        TransactionId = invoice.TransactionId,
                        ReceiptNumber = $"{invoice.InvoiceNumber.Replace("INV", "RCP")}/{1}",
                        InvoiceValueBalanceBeforeReceipting = invoice.Value,
                        CreatedById = loggedInUserId,
                        InvoiceValueBalanceAfterReceipting = 0,
                        ReceiptValue = invoice.Value
                    };

                    await _context.Receipts.AddAsync(receipt);
                    await _context.SaveChangesAsync();

                    invoice.IsReceiptedStatus = (int)InvoiceStatus.CompletelyReceipted;
                    _context.Invoices.Update(invoice);
                    await _context.SaveChangesAsync();

                    await _receiptService.PostAccounts(loggedInUserId, receipt, invoice, 1100000001);
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogTrace(ex.StackTrace);
                return false;
            }
        }

        public async Task<ApiResponse> GetInvoices(HttpContext context, long prospectId)
        {
            try
            {
                string groupInvoiceNumber = string.Empty;
                var quoteServices = new List<QuoteService>();

                var prospect = await _context.Prospects.FindAsync(prospectId);

                var leadDivision = await _context.LeadDivisions
                                                .Where(x => x.LeadId == prospect.LeadId && !x.IsDeleted)
                                                .FirstOrDefaultAsync();

                if(leadDivision == null)
                {
                    return new ApiResponse(404, "Cart items have not been posted for suspect.");
                }

                var quote = await _context.Quotes
                                         .Where(x => x.LeadDivisionId == leadDivision.Id && !x.IsDeleted)
                                         .FirstOrDefaultAsync();

                quoteServices = await _context.QuoteServices.Where(x => x.QuoteId == quote.Id && !x.IsDeleted)
                                        .ToListAsync();

               // groupInvoiceNumber = quoteServices.FirstOrDefault().GroupInvoiceNumber;

                var thelead = await _context.Leads.FindAsync(prospect.LeadId);

                // get and send the invoice back
                var theInvoices = await _context.Invoices
                    .Where(x => x.GroupInvoiceNumber == groupInvoiceNumber && x.StartDate == quoteServices.FirstOrDefault().ContractStartDate)
                    .ToListAsync();

                return new ApiOkResponse(theInvoices);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                return new ApiResponse(500, ex.Message);
            }
        }

        public async Task<ApiResponse> GetProformaInvoices(long prospectId, long quoteId)
        {
            try
            {
                var prospect = await _context.Prospects.FindAsync(prospectId);

                var leadDivision = await _context.LeadDivisions
                                                .Where(x => x.LeadId == prospect.LeadId && !x.IsDeleted)
                                                .FirstOrDefaultAsync();

                if (leadDivision == null)
                {
                    return new ApiResponse(404, "Cart items have not been posted for suspect.");
                }

                var theInvoices = await _context.ProformaInvoices
                    .Where(x => x.LeadDivisionId == leadDivision.Id && x.QuoteId == quoteId)
                    .ToListAsync();

                return new ApiOkResponse(theInvoices);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                return new ApiResponse(500, ex.Message);
            }
        }

        public async Task<ApiResponse> GetQuoteServices(HttpContext context, long prospectId)
        {
            try
            {
                var quoteServices = new List<QuoteService>();

                var prospect = await _context.Prospects.FindAsync(prospectId);

                var leadDivision = await _context.LeadDivisions
                                                .Where(x => x.LeadId == prospect.LeadId && !x.IsDeleted)
                                                .FirstOrDefaultAsync();

                if (leadDivision == null)
                {
                    return new ApiResponse(404, "Cart items have not been posted for suspect.");
                }

                var quote = await _context.Quotes
                                         .Where(x => x.LeadDivisionId == leadDivision.Id && !x.IsDeleted)
                                         .FirstOrDefaultAsync();

                quoteServices = await _context.QuoteServices.Where(x => x.QuoteId == quote.Id && !x.IsDeleted)
                                        .ToListAsync();

                return new ApiOkResponse(quoteServices);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                return new ApiResponse(500, ex.Message);
            }
        }

        public async Task<ApiResponse> GetQuoteServicesPaymentStatus(HttpContext context, long prospectId)
        {
            try
            {
                var prospect = await _context.Prospects.FindAsync(prospectId);

                var leadDivision = await _context.LeadDivisions
                                                .Where(x => x.LeadId == prospect.LeadId && !x.IsDeleted)
                                                .FirstOrDefaultAsync();

                if (leadDivision == null)
                {
                    return new ApiResponse(404, "Cart items have not been posted for suspect.");
                }

                var quotes = await _context.Quotes.AsNoTracking()
                                         .Include(x => x.QuoteServices.Where(x => !x.IsDeleted))
                                         .Where(x => x.LeadDivisionId == leadDivision.Id && !x.IsDeleted)
                                         .ToListAsync();

                var statuses = new List<QuotePaymentStatusTransferDTO>();

                foreach (var quote in quotes)
                {
                    var status = new QuotePaymentStatusTransferDTO { Quote = quote };
                    
                    if (quote.IsConvertedToContract)
                    {
                        status.PaymentStatus = PaymentStatus.Success;
                    }
                    else
                    {
                        status.PaymentStatus = PaymentStatus.Pending;
                    }

                    statuses.Add(status);
                }

                return new ApiOkResponse(statuses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                return new ApiResponse(500, ex.Message);
            }
        }
    }
}
