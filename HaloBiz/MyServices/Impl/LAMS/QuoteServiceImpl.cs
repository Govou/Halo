using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using HaloBiz.Adapters;
using HalobizMigrations.Data;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.Helpers;
using HalobizMigrations.Models;

using HaloBiz.MyServices.LAMS;
using HaloBiz.Repository;
using HaloBiz.Repository.LAMS;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Halobiz.Common.DTOs.ReceivingDTOs;
using Halobiz.Common.Helpers;

namespace HaloBiz.MyServices.Impl.LAMS
{
    public class QuoteServiceImpl : IQuoteService
    {
        private readonly ILogger<QuoteServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IQuoteRepository _quoteRepo;
        private readonly IQuoteServiceRepository _quoteServiceRepo;
        private readonly IMailAdapter _mailAdapter;
        private readonly IMapper _mapper;
        private readonly HalobizContext _context;

        public QuoteServiceImpl(IModificationHistoryRepository historyRepo, 
            IQuoteRepository quoteRepo, 
            IQuoteServiceRepository quoteServiceRepo, 
            ILogger<QuoteServiceImpl> logger, 
            IMailAdapter mailAdapter,
            IMapper mapper,
            HalobizContext context)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._quoteRepo = quoteRepo;
            this._quoteServiceRepo = quoteServiceRepo;
            this._mailAdapter = mailAdapter;
            this._logger = logger;
            this._context = context;
        }

        public async Task<ApiCommonResponse> AddQuote(HttpContext context, QuoteReceivingDTO quoteReceivingDTO)
        {
            Quote savedQuote = null;

            foreach (var quoteService in quoteReceivingDTO.QuoteServices)
            {
                if(quoteService.ContractStartDate == null || quoteService.ContractEndDate == null
                    || quoteService.InvoicingInterval == null || quoteService.PaymentCycle == null || quoteService.FirstInvoiceSendDate == null
                    || quoteService.FulfillmentStartDate == null || quoteService.FulfillmentEndDate == null)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Missing Quote Service Parameters");
                }
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var createdById = context.GetLoggedInUserId();

                    var quote = _mapper.Map<Quote>(quoteReceivingDTO);
                    var quoteService = quote.QuoteServices;

                    quote.QuoteServices = null;
                    quote.CreatedById = createdById;
                    savedQuote = await _quoteRepo.SaveQuote(quote);

                    if (savedQuote == null)
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                    }

                    foreach (var item in quoteService)
                    {
                        item.QuoteId = savedQuote.Id;
                        item.CreatedById = createdById;

                        if (item.InvoicingInterval == (int) TimeCycle.MonthlyProrata)
                        {
                            if (item.ContractEndDate.Value.AddDays(1).Day != 1)
                            {
                                return CommonResponse.Send(ResponseCodes.FAILURE, null, $"Contract end date must be last day of month for tag {item.UniqueTag}");
                            }
                        }
                    }

                    var savedSuccessfully = await _quoteServiceRepo.SaveQuoteServiceRange(quoteService);
                    if (!savedSuccessfully)
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                    }

                    await transaction.CommitAsync();
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    await transaction.RollbackAsync();
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                }                   
            }

            var quoteFromDatabase = await _quoteRepo.FindQuoteById(savedQuote.Id);
            var quoteTransferDTO = _mapper.Map<QuoteTransferDTO>(quoteFromDatabase);
            return CommonResponse.Send(ResponseCodes.SUCCESS,quoteTransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllQuote()
        {
            var quotes = await _quoteRepo.FindAllQuote();
            if (quotes == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var quoteTransferDTO = _mapper.Map<IEnumerable<QuoteTransferDTO>>(quotes);
            return CommonResponse.Send(ResponseCodes.SUCCESS,quoteTransferDTO);
        }

        public async Task<ApiCommonResponse> GetQuoteById(long id)
        {
            var quote = await _quoteRepo.FindQuoteById(id);
            if (quote == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var quoteTransferDTOs = _mapper.Map<QuoteTransferDTO>(quote);
            return CommonResponse.Send(ResponseCodes.SUCCESS,quoteTransferDTOs);
        } 

        public async Task<ApiCommonResponse> FindByLeadDivisionId(long id)
        {
            var quote = await _quoteRepo.FindByLeadDivisionId(id);
            if (quote == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var quoteTransferDTOs = _mapper.Map<QuoteTransferDTO>(quote);
            return CommonResponse.Send(ResponseCodes.SUCCESS,quoteTransferDTOs);
        }

        public async Task<ApiCommonResponse> GetQuoteByReferenceNumber(string reference)
        {
            var quote = await _quoteRepo.FindQuoteByReferenceNumber(reference);
            if (quote == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var quoteTransferDTOs = _mapper.Map<QuoteTransferDTO>(quote);
            return CommonResponse.Send(ResponseCodes.SUCCESS,quoteTransferDTOs);
        }

        public async Task<ApiCommonResponse> UpdateQuote(HttpContext context, long id, QuoteReceivingDTO quoteReceivingDTO)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (quoteReceivingDTO.QuoteServices.Count() < 1)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE,null, "There should be at least 1 quote service for an update.");
                }

                var createdById = context.GetLoggedInUserId();
                var quote = await _quoteRepo.FindQuoteById(id);
                if (quote == null)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE,null, "Quote was not found");
                }

                var summary = $"Initial details before change, \n {quote} \n";

                var quoteServices = _mapper.Map<IEnumerable<QuoteService>>(quoteReceivingDTO.QuoteServices);

                foreach (var item in quoteServices)
                {
                    item.QuoteId = id;
                    item.CreatedById = createdById;
                }               

                if (quote.QuoteServices.Any())
                {
                    var deleteSuccessful = await _quoteServiceRepo.DeleteQuoteServiceRange(quote.QuoteServices);
                    if (!deleteSuccessful)
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                    }
                }

               

                var savedSuccessful = await _quoteServiceRepo.SaveQuoteServiceRange(quoteServices);
                if (!savedSuccessful)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                }

                var updatedQuote = await _quoteRepo.FindQuoteById(id);

                if (updatedQuote == null)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                }

                summary += $"Details after change, \n {updatedQuote} \n";          

                ModificationHistory history = new ModificationHistory()
                {
                    ModelChanged = "Quote",
                    ChangeSummary = summary,
                    ChangedById = context.GetLoggedInUserId(),
                    ModifiedModelId = updatedQuote.Id
                };

                await _historyRepo.SaveHistory(history);
                var quoteTransferDTOs = _mapper.Map<QuoteTransferDTO>(updatedQuote);

                await transaction.CommitAsync();
                return CommonResponse.Send(ResponseCodes.SUCCESS,quoteTransferDTOs);
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                _logger.LogError(e.Message);
                _logger.LogError(e.StackTrace);
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
        }

        public async Task<ApiCommonResponse> DeleteQuote(long id)
        {
            var quoteToDelete = await _quoteRepo.FindQuoteById(id);
            if (quoteToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _quoteRepo.DeleteQuote(quoteToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }
    }
}
