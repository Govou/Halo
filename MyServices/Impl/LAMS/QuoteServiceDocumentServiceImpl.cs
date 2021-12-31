using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
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

namespace HaloBiz.MyServices.Impl.LAMS
{
    public class QuoteServiceDocumentServiceImpl : IQuoteServiceDocumentService
    {
        private readonly ILogger<QuoteServiceDocumentServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IQuoteServiceDocumentRepository _quoteServiceDocumentRepo;
        private readonly IQuoteRepository _quoteRepository;
        private readonly IMapper _mapper;

        public QuoteServiceDocumentServiceImpl(IModificationHistoryRepository historyRepo,
            IQuoteRepository quoteRepository,
            IQuoteServiceDocumentRepository quoteServiceDocumentRepo, ILogger<QuoteServiceDocumentServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._quoteServiceDocumentRepo = quoteServiceDocumentRepo;
            this._logger = logger;
            _quoteRepository = quoteRepository;
        }

        public async Task<ApiResponse> AddQuoteServiceDocument(HttpContext context, QuoteServiceDocumentReceivingDTO qdd)
        {
           // var quoteServiceDocument = _mapper.Map<QuoteServiceDocument>(qdd);
           
            var CreatedById = context.GetLoggedInUserId();

            //check if this is a group upload 
            List<QuoteServiceDocument> serviceDocuments = new List<QuoteServiceDocument>();
            if (qdd.IsGroupUpload)
            {
                //get all the services of the quote
                var quote = await _quoteRepository.FindQuoteById(qdd.QuoteId);

                foreach (var quoteService in quote?.QuoteServices)
                {
                    serviceDocuments.Add(new QuoteServiceDocument
                    {
                        Caption = qdd.Caption,
                        Description = qdd.Description,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        DocumentUrl = qdd.DocumentUrl,
                        Type = qdd.Type,
                        QuoteServiceId = quoteService.Id,
                        CreatedById = CreatedById
                    });
                }
            }
            else
            {
                serviceDocuments.Add(new QuoteServiceDocument
                {
                    Caption = qdd.Caption,
                    Description = qdd.Description,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    DocumentUrl = qdd.DocumentUrl,
                    Type = qdd.Type,
                    QuoteServiceId = qdd.QuoteServiceId,
                    CreatedById = CreatedById
                });
            }

            var savedQuoteServiceDocument = await _quoteServiceDocumentRepo.SaveQuoteServiceDocument(serviceDocuments);
           
            if (savedQuoteServiceDocument == null)
            {
                return new ApiResponse(500);
            }

            var quoteServiceDocumentTransferDTO = _mapper.Map<QuoteServiceDocumentTransferDTO>(savedQuoteServiceDocument);
            return new ApiOkResponse(quoteServiceDocumentTransferDTO);
        }

        public async Task<ApiResponse> GetAllQuoteServiceDocument()
        {
            var quoteServiceDocuments = await _quoteServiceDocumentRepo.FindAllQuoteServiceDocument();
            if (quoteServiceDocuments == null)
            {
                return new ApiResponse(404);
            }
            var quoteServiceDocumentTransferDTO = _mapper.Map<IEnumerable<QuoteServiceDocumentTransferDTO>>(quoteServiceDocuments);
            return new ApiOkResponse(quoteServiceDocumentTransferDTO);
        }
        public async Task<ApiResponse> GetAllQuoteServiceDocumentForAQuoteService(long quoteServiceId)
        {
            var quoteServiceDocuments = await _quoteServiceDocumentRepo.FindAllQuoteServiceDocumentForAQuoteService(quoteServiceId);
            if (quoteServiceDocuments == null)
            {
                return new ApiResponse(404);
            }
            var quoteServiceDocumentTransferDTO = _mapper.Map<IEnumerable<QuoteServiceDocumentTransferDTO>>(quoteServiceDocuments);
            return new ApiOkResponse(quoteServiceDocumentTransferDTO);
        }

        public async Task<ApiResponse> GetQuoteServiceDocumentById(long id)
        {
            var quoteServiceDocument = await _quoteServiceDocumentRepo.FindQuoteServiceDocumentById(id);
            if (quoteServiceDocument == null)
            {
                return new ApiResponse(404);
            }
            var quoteServiceDocumentTransferDTOs = _mapper.Map<QuoteServiceDocumentTransferDTO>(quoteServiceDocument);
            return new ApiOkResponse(quoteServiceDocumentTransferDTOs);
        }

        public async Task<ApiResponse> GetQuoteServiceDocumentByCaption(string caption)
        {
            var quoteServiceDocument = await _quoteServiceDocumentRepo.FindQuoteServiceDocumentByCaption(caption);
            if (quoteServiceDocument == null)
            {
                return new ApiResponse(404);
            }
            var quoteServiceDocumentTransferDTOs = _mapper.Map<QuoteServiceDocumentTransferDTO>(quoteServiceDocument);
            return new ApiOkResponse(quoteServiceDocumentTransferDTOs);
        }

        public async Task<ApiResponse> UpdateQuoteServiceDocument(HttpContext context, long id, QuoteServiceDocumentReceivingDTO quoteServiceDocumentReceivingDTO)
        {
            var quoteServiceDocumentToUpdate = await _quoteServiceDocumentRepo.FindQuoteServiceDocumentById(id);
            if (quoteServiceDocumentToUpdate == null)
            {
                return new ApiResponse(404);
            }
            
            var summary = $"Initial details before change, \n {quoteServiceDocumentToUpdate.ToString()} \n" ;

            quoteServiceDocumentToUpdate.Caption = quoteServiceDocumentReceivingDTO.Caption;
            quoteServiceDocumentToUpdate.Description = quoteServiceDocumentReceivingDTO.Description;
            quoteServiceDocumentToUpdate.QuoteServiceId = quoteServiceDocumentReceivingDTO.QuoteServiceId;
            quoteServiceDocumentToUpdate.DocumentUrl = quoteServiceDocumentReceivingDTO.DocumentUrl;
            var updatedQuoteServiceDocument = await _quoteServiceDocumentRepo.UpdateQuoteServiceDocument(quoteServiceDocumentToUpdate);

            summary += $"Details after change, \n {updatedQuoteServiceDocument.ToString()} \n";

            if (updatedQuoteServiceDocument == null)
            {
                return new ApiResponse(500);
            }
            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "QuoteServiceDocument",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedQuoteServiceDocument.Id
            };

            await _historyRepo.SaveHistory(history);

            var quoteServiceDocumentTransferDTOs = _mapper.Map<QuoteServiceDocumentTransferDTO>(updatedQuoteServiceDocument);
            return new ApiOkResponse(quoteServiceDocumentTransferDTOs);

        }

        public async Task<ApiResponse> DeleteQuoteServiceDocument(long id)
        {
            var quoteServiceDocumentToDelete = await _quoteServiceDocumentRepo.FindQuoteServiceDocumentById(id);
            if (quoteServiceDocumentToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _quoteServiceDocumentRepo.DeleteQuoteServiceDocument(quoteServiceDocumentToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }
    }
}