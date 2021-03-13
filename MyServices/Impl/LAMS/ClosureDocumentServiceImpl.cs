using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
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
    public class ClosureDocumentServiceImpl : IClosureDocumentService
    {
        private readonly ILogger<ClosureDocumentServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IClosureDocumentRepository _closureDocumentRepo;
        private readonly IMapper _mapper;

        public ClosureDocumentServiceImpl(IModificationHistoryRepository historyRepo, IClosureDocumentRepository closureDocumentRepo, ILogger<ClosureDocumentServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._closureDocumentRepo = closureDocumentRepo;
            this._logger = logger;
        }

        public async Task<ApiResponse> AddClosureDocument(HttpContext context, ClosureDocumentReceivingDTO closureDocumentReceivingDTO)
        {
            var closureDocument = _mapper.Map<ClosureDocument>(closureDocumentReceivingDTO);
            closureDocument.CreatedById = context.GetLoggedInUserId();
            var savedClosureDocument = await _closureDocumentRepo.SaveClosureDocument(closureDocument);
            if (savedClosureDocument == null)
            {
                return new ApiResponse(500);
            }
            var closureDocumentTransferDTO = _mapper.Map<ClosureDocumentTransferDTO>(savedClosureDocument);
            return new ApiOkResponse(closureDocumentTransferDTO);
        }

        public async Task<ApiResponse> GetAllClosureDocument()
        {
            var closureDocuments = await _closureDocumentRepo.FindAllClosureDocument();
            if (closureDocuments == null)
            {
                return new ApiResponse(404);
            }
            var closureDocumentTransferDTO = _mapper.Map<IEnumerable<ClosureDocumentTransferDTO>>(closureDocuments);
            return new ApiOkResponse(closureDocumentTransferDTO);
        }

        public async Task<ApiResponse> GetClosureDocumentById(long id)
        {
            var closureDocument = await _closureDocumentRepo.FindClosureDocumentById(id);
            if (closureDocument == null)
            {
                return new ApiResponse(404);
            }
            var closureDocumentTransferDTOs = _mapper.Map<ClosureDocumentTransferDTO>(closureDocument);
            return new ApiOkResponse(closureDocumentTransferDTOs);
        }

        public async Task<ApiResponse> GetClosureDocumentByCaption(string caption)
        {
            var closureDocument = await _closureDocumentRepo.FindClosureDocumentByCaption(caption);
            if (closureDocument == null)
            {
                return new ApiResponse(404);
            }
            var closureDocumentTransferDTOs = _mapper.Map<ClosureDocumentTransferDTO>(closureDocument);
            return new ApiOkResponse(closureDocumentTransferDTOs);
        }

        public async Task<ApiResponse> UpdateClosureDocument(HttpContext context, long id, ClosureDocumentReceivingDTO closureDocumentReceivingDTO)
        {
            var closureDocumentToUpdate = await _closureDocumentRepo.FindClosureDocumentById(id);
            if (closureDocumentToUpdate == null)
            {
                return new ApiResponse(404);
            }
            
            var summary = $"Initial details before change, \n {closureDocumentToUpdate.ToString()} \n" ;

            closureDocumentToUpdate.Caption = closureDocumentReceivingDTO.Caption;
            closureDocumentToUpdate.Description = closureDocumentReceivingDTO.Description;
            closureDocumentToUpdate.ContractServiceId = closureDocumentReceivingDTO.ContractServiceId;
            closureDocumentToUpdate.DocumentUrl = closureDocumentReceivingDTO.DocumentUrl;
            var updatedClosureDocument = await _closureDocumentRepo.UpdateClosureDocument(closureDocumentToUpdate);

            summary += $"Details after change, \n {updatedClosureDocument.ToString()} \n";

            if (updatedClosureDocument == null)
            {
                return new ApiResponse(500);
            }
            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "ClosureDocument",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedClosureDocument.Id
            };

            await _historyRepo.SaveHistory(history);

            var closureDocumentTransferDTOs = _mapper.Map<ClosureDocumentTransferDTO>(updatedClosureDocument);
            return new ApiOkResponse(closureDocumentTransferDTOs);

        }

        public async Task<ApiResponse> DeleteClosureDocument(long id)
        {
            var closureDocumentToDelete = await _closureDocumentRepo.FindClosureDocumentById(id);
            if (closureDocumentToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _closureDocumentRepo.DeleteClosureDocument(closureDocumentToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }
    }
}