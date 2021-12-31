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

        public async Task<ApiCommonResponse> AddQuoteServiceDocument(HttpContext context, QuoteServiceDocumentReceivingDTO qdd)
        {
            if (qdd.IsGroupUpload && qdd.QuoteId == 0)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "You need to specify quoteId since IsGroupUpload is true");
            }
            else if (!qdd.IsGroupUpload && qdd.QuoteServiceId == 0)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "You need to specify QuoteServiceId");

            }
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
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            var quoteServiceDocumentTransferDTO = _mapper.Map<QuoteServiceDocumentTransferDTO>(savedQuoteServiceDocument);            
            return CommonResponse.Send(ResponseCodes.SUCCESS,quoteServiceDocumentTransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllQuoteServiceDocument()
        {
            var quoteServiceDocuments = await _quoteServiceDocumentRepo.FindAllQuoteServiceDocument();
            if (quoteServiceDocuments == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var quoteServiceDocumentTransferDTO = _mapper.Map<IEnumerable<QuoteServiceDocumentTransferDTO>>(quoteServiceDocuments);
            return CommonResponse.Send(ResponseCodes.SUCCESS,quoteServiceDocumentTransferDTO);
        }
        public async Task<ApiCommonResponse> GetAllQuoteServiceDocumentForAQuoteService(long quoteServiceId)
        {
            var quoteServiceDocuments = await _quoteServiceDocumentRepo.FindAllQuoteServiceDocumentForAQuoteService(quoteServiceId);
            if (quoteServiceDocuments == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var quoteServiceDocumentTransferDTO = _mapper.Map<IEnumerable<QuoteServiceDocumentTransferDTO>>(quoteServiceDocuments);
            return CommonResponse.Send(ResponseCodes.SUCCESS,quoteServiceDocumentTransferDTO);
        }

        public async Task<ApiCommonResponse> GetQuoteServiceDocumentById(long id)
        {
            var quoteServiceDocument = await _quoteServiceDocumentRepo.FindQuoteServiceDocumentById(id);
            if (quoteServiceDocument == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var quoteServiceDocumentTransferDTOs = _mapper.Map<QuoteServiceDocumentTransferDTO>(quoteServiceDocument);
            return CommonResponse.Send(ResponseCodes.SUCCESS,quoteServiceDocumentTransferDTOs);
        }

        public async Task<ApiCommonResponse> GetQuoteServiceDocumentByCaption(string caption)
        {
            var quoteServiceDocument = await _quoteServiceDocumentRepo.FindQuoteServiceDocumentByCaption(caption);
            if (quoteServiceDocument == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var quoteServiceDocumentTransferDTOs = _mapper.Map<QuoteServiceDocumentTransferDTO>(quoteServiceDocument);
            return CommonResponse.Send(ResponseCodes.SUCCESS,quoteServiceDocumentTransferDTOs);
        }

        public async Task<ApiCommonResponse> UpdateQuoteServiceDocument(HttpContext context, long id, QuoteServiceDocumentReceivingDTO quoteServiceDocumentReceivingDTO)
        {
            var quoteServiceDocumentToUpdate = await _quoteServiceDocumentRepo.FindQuoteServiceDocumentById(id);
            if (quoteServiceDocumentToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
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
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "QuoteServiceDocument",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedQuoteServiceDocument.Id
            };

            await _historyRepo.SaveHistory(history);

            var quoteServiceDocumentTransferDTOs = _mapper.Map<QuoteServiceDocumentTransferDTO>(updatedQuoteServiceDocument);
            return CommonResponse.Send(ResponseCodes.SUCCESS,quoteServiceDocumentTransferDTOs);

        }

        public async Task<ApiCommonResponse> DeleteQuoteServiceDocument(long id)
        {
            var quoteServiceDocumentToDelete = await _quoteServiceDocumentRepo.FindQuoteServiceDocumentById(id);
            if (quoteServiceDocumentToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _quoteServiceDocumentRepo.DeleteQuoteServiceDocument(quoteServiceDocumentToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }
    }
}