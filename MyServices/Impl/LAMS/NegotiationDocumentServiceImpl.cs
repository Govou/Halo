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
    public class NegotiationDocumentServiceImpl : INegotiationDocumentService
    {
        private readonly ILogger<NegotiationDocumentServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly INegotiationDocumentRepository _negotiationDocumentRepo;
        private readonly IMapper _mapper;

        public NegotiationDocumentServiceImpl(IModificationHistoryRepository historyRepo, INegotiationDocumentRepository negotiationDocumentRepo, ILogger<NegotiationDocumentServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._negotiationDocumentRepo = negotiationDocumentRepo;
            this._logger = logger;
        }

        public async Task<ApiCommonResponse> AddNegotiationDocument(HttpContext context, NegotiationDocumentReceivingDTO negotiationDocumentReceivingDTO)
        {
            var negotiationDocument = _mapper.Map<NegotiationDocument>(negotiationDocumentReceivingDTO);
            negotiationDocument.CreatedById = context.GetLoggedInUserId();
            var savedNegotiationDocument = await _negotiationDocumentRepo.SaveNegotiationDocument(negotiationDocument);
            if (savedNegotiationDocument == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var negotiationDocumentTransferDTO = _mapper.Map<NegotiationDocumentTransferDTO>(savedNegotiationDocument);
            return new ApiOkResponse(negotiationDocumentTransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllNegotiationDocument()
        {
            var negotiationDocuments = await _negotiationDocumentRepo.FindAllNegotiationDocument();
            if (negotiationDocuments == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var negotiationDocumentTransferDTO = _mapper.Map<IEnumerable<NegotiationDocumentTransferDTO>>(negotiationDocuments);
            return new ApiOkResponse(negotiationDocumentTransferDTO);
        }

        public async Task<ApiCommonResponse> GetNegotiationDocumentById(long id)
        {
            var negotiationDocument = await _negotiationDocumentRepo.FindNegotiationDocumentById(id);
            if (negotiationDocument == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var negotiationDocumentTransferDTOs = _mapper.Map<NegotiationDocumentTransferDTO>(negotiationDocument);
            return new ApiOkResponse(negotiationDocumentTransferDTOs);
        }

        public async Task<ApiCommonResponse> GetNegotiationDocumentByCaption(string caption)
        {
            var negotiationDocument = await _negotiationDocumentRepo.FindNegotiationDocumentByCaption(caption);
            if (negotiationDocument == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var negotiationDocumentTransferDTOs = _mapper.Map<NegotiationDocumentTransferDTO>(negotiationDocument);
            return new ApiOkResponse(negotiationDocumentTransferDTOs);
        }

        public async Task<ApiCommonResponse> UpdateNegotiationDocument(HttpContext context, long id, NegotiationDocumentReceivingDTO negotiationDocumentReceivingDTO)
        {
            var negotiationDocumentToUpdate = await _negotiationDocumentRepo.FindNegotiationDocumentById(id);
            if (negotiationDocumentToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            
            var summary = $"Initial details before change, \n {negotiationDocumentToUpdate.ToString()} \n" ;

            negotiationDocumentToUpdate.Caption = negotiationDocumentReceivingDTO.Caption;
            negotiationDocumentToUpdate.Description = negotiationDocumentReceivingDTO.Description;
            negotiationDocumentToUpdate.QuoteServiceId = negotiationDocumentReceivingDTO.QuoteServiceId;
            negotiationDocumentToUpdate.DocumentUrl = negotiationDocumentReceivingDTO.DocumentUrl;
            var updatedNegotiationDocument = await _negotiationDocumentRepo.UpdateNegotiationDocument(negotiationDocumentToUpdate);

            summary += $"Details after change, \n {updatedNegotiationDocument.ToString()} \n";

            if (updatedNegotiationDocument == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "NegotiationDocument",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedNegotiationDocument.Id
            };

            await _historyRepo.SaveHistory(history);

            var negotiationDocumentTransferDTOs = _mapper.Map<NegotiationDocumentTransferDTO>(updatedNegotiationDocument);
            return new ApiOkResponse(negotiationDocumentTransferDTOs);

        }

        public async Task<ApiCommonResponse> DeleteNegotiationDocument(long id)
        {
            var negotiationDocumentToDelete = await _negotiationDocumentRepo.FindNegotiationDocumentById(id);
            if (negotiationDocumentToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _negotiationDocumentRepo.DeleteNegotiationDocument(negotiationDocumentToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }
    }
}