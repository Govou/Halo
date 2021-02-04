using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using HaloBiz.Data;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.Helpers;
using HaloBiz.Model;
using HaloBiz.Model.LAMS;
using HaloBiz.MyServices.LAMS;
using HaloBiz.Repository;
using HaloBiz.Repository.LAMS;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace HaloBiz.MyServices.Impl.LAMS
{
    public class QuoteServiceImpl : IQuoteService
    {
        private readonly ILogger<QuoteServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IQuoteRepository _quoteRepo;
        private readonly IQuoteServiceRepository _quoteServiceRepo;
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public QuoteServiceImpl(IModificationHistoryRepository historyRepo, 
            IQuoteRepository quoteRepo, 
            IQuoteServiceRepository quoteServiceRepo, 
            ILogger<QuoteServiceImpl> logger, 
            IMapper mapper,
            DataContext context)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._quoteRepo = quoteRepo;
            this._quoteServiceRepo = quoteServiceRepo;
            this._logger = logger;
            this._context = context;
        }

        public async Task<ApiResponse> AddQuote(HttpContext context, QuoteReceivingDTO quoteReceivingDTO)
        {
            Quote savedQuote = null;

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var createdById = context.GetLoggedInUserId();

                    var quote = _mapper.Map<Quote>(quoteReceivingDTO);
                    var quoteServices = quote.QuoteServices;

                    quote.QuoteServices = null;
                    quote.CreatedById = createdById;
                    savedQuote = await _quoteRepo.SaveQuote(quote);

                    if (savedQuote == null)
                    {
                        return new ApiResponse(500);
                    }

                    foreach (var item in quoteServices)
                    {
                        item.QuoteId = savedQuote.Id;
                        item.CreatedById = createdById;
                    }

                    var savedSuccessfully = await _quoteServiceRepo.SaveQuoteServiceRange(quoteServices);
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
            var quoteToUpdate = await _quoteRepo.FindQuoteById(id);
            if (quoteToUpdate == null)
            {
                return new ApiResponse(404);
            }
            
            var summary = $"Initial details before change, \n {quoteToUpdate.ToString()} \n" ;

            quoteToUpdate.ReferenceNo = quoteReceivingDTO.ReferenceNo;
            quoteToUpdate.LeadDivisionId = quoteReceivingDTO.LeadDivisionId;
            quoteToUpdate.IsConvertedToContract = quoteReceivingDTO.IsConvertedToContract;
            quoteToUpdate.Version = quoteReceivingDTO.Version;

            var updatedQuote = await _quoteRepo.UpdateQuote(quoteToUpdate);

            summary += $"Details after change, \n {updatedQuote.ToString()} \n";

            if (updatedQuote == null)
            {
                return new ApiResponse(500);
            }
            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "Quote",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedQuote.Id
            };

            await _historyRepo.SaveHistory(history);

            var quoteTransferDTOs = _mapper.Map<QuoteTransferDTO>(updatedQuote);
            return new ApiOkResponse(quoteTransferDTOs);

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