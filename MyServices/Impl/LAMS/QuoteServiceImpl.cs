using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using HaloBiz.Adapters;
using HalobizMigrations.Data;
using HaloBiz.DTOs.ApiDTOs;
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

        public async Task<ApiResponse> AddQuote(HttpContext context, QuoteReceivingDTO quoteReceivingDTO)
        {
            Quote savedQuote = null;

            foreach (var quoteService in quoteReceivingDTO.QuoteServices)
            {
                if(quoteService.ContractStartDate == null || quoteService.ContractEndDate == null
                    || quoteService.InvoicingInterval == null || quoteService.PaymentCycle == null || quoteService.FirstInvoiceSendDate == null
                    || quoteService.FulfillmentStartDate == null || quoteService.FulfillmentEndDate == null)
                {
                    return new ApiResponse(400, "Missing Quote Service Parameters");
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
                        return new ApiResponse(500);
                    }

                    foreach (var item in quoteService)
                    {
                        item.QuoteId = savedQuote.Id;
                        item.CreatedById = createdById;
                    }

                    var savedSuccessfully = await _quoteServiceRepo.SaveQuoteServiceRange(quoteService);
                    if (!savedSuccessfully)
                    {
                        return new ApiResponse(500);
                    }

                    await transaction.CommitAsync();
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    await transaction.RollbackAsync();
                    return new ApiResponse(500);
                }                   
            }

            var quoteFromDatabase = await _quoteRepo.FindQuoteById(savedQuote.Id);
            
            //var serializedQuote = JsonConvert.SerializeObject(quoteFromDatabase, new JsonSerializerSettings { 
            //    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            //});

            //Action action = async () => {
            //    await _mailAdapter.SendQuoteNotification(serializedQuote);
            //};
            //action.RunAsTask();
            var quoteTransferDTO = _mapper.Map<QuoteTransferDTO>(quoteFromDatabase);
            return new ApiOkResponse(quoteTransferDTO);
        }

        public async Task<ApiResponse> GetAllQuote()
        {
            var quotes = await _quoteRepo.FindAllQuote();
            if (quotes == null)
            {
                return new ApiResponse(404);
            }
            var quoteTransferDTO = _mapper.Map<IEnumerable<QuoteTransferDTO>>(quotes);
            return new ApiOkResponse(quoteTransferDTO);
        }

        public async Task<ApiResponse> GetQuoteById(long id)
        {
            var quote = await _quoteRepo.FindQuoteById(id);
            if (quote == null)
            {
                return new ApiResponse(404);
            }
            var quoteTransferDTOs = _mapper.Map<QuoteTransferDTO>(quote);
            return new ApiOkResponse(quoteTransferDTOs);
        }

        public async Task<ApiResponse> GetQuoteByReferenceNumber(string reference)
        {
            var quote = await _quoteRepo.FindQuoteByReferenceNumber(reference);
            if (quote == null)
            {
                return new ApiResponse(404);
            }
            var quoteTransferDTOs = _mapper.Map<QuoteTransferDTO>(quote);
            return new ApiOkResponse(quoteTransferDTOs);
        }

        public async Task<ApiResponse> UpdateQuote(HttpContext context, long id, QuoteReceivingDTO quoteReceivingDTO)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (quoteReceivingDTO.QuoteServices.Count() < 1)
                {
                    return new ApiResponse(400, "There should be at least 1 quote service for an update.");
                }

                var createdById = context.GetLoggedInUserId();
                var quote = await _quoteRepo.FindQuoteById(id);
                if (quote == null)
                {
                    return new ApiResponse(404, "Quote was not found");
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
                        return new ApiResponse(500);
                    }
                }

               

                var savedSuccessful = await _quoteServiceRepo.SaveQuoteServiceRange(quoteServices);
                if (!savedSuccessful)
                {
                    return new ApiResponse(500);
                }

                var updatedQuote = await _quoteRepo.FindQuoteById(id);

                if (updatedQuote == null)
                {
                    return new ApiResponse(500);
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
                return new ApiOkResponse(quoteTransferDTOs);
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                _logger.LogError(e.Message);
                _logger.LogError(e.StackTrace);
                return new ApiResponse(500);
            }
        }

        public async Task<ApiResponse> DeleteQuote(long id)
        {
            var quoteToDelete = await _quoteRepo.FindQuoteById(id);
            if (quoteToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _quoteRepo.DeleteQuote(quoteToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }
    }
}
