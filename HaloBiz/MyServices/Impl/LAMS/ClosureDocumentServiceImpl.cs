using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
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

        public async Task<ApiCommonResponse> AddClosureDocument(HttpContext context, ClosureDocumentReceivingDTO closureDocumentReceivingDTO)
        {
            var closureDocument = _mapper.Map<ClosureDocument>(closureDocumentReceivingDTO);
            closureDocument.CreatedById = context.GetLoggedInUserId();
            var savedClosureDocument = await _closureDocumentRepo.SaveClosureDocument(closureDocument);
            if (savedClosureDocument == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var closureDocumentTransferDTO = _mapper.Map<ClosureDocumentTransferDTO>(savedClosureDocument);
            return CommonResponse.Send(ResponseCodes.SUCCESS,closureDocumentTransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllClosureDocument()
        {
            var closureDocuments = await _closureDocumentRepo.FindAllClosureDocument();
            if (closureDocuments == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var closureDocumentTransferDTO = _mapper.Map<IEnumerable<ClosureDocumentTransferDTO>>(closureDocuments);
            return CommonResponse.Send(ResponseCodes.SUCCESS,closureDocumentTransferDTO);
        }

        public async Task<ApiCommonResponse> GetClosureDocumentById(long id)
        {
            var closureDocument = await _closureDocumentRepo.FindClosureDocumentById(id);
            if (closureDocument == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var closureDocumentTransferDTOs = _mapper.Map<ClosureDocumentTransferDTO>(closureDocument);
            return CommonResponse.Send(ResponseCodes.SUCCESS,closureDocumentTransferDTOs);
        }

        public async Task<ApiCommonResponse> GetClosureDocumentByCaption(string caption)
        {
            var closureDocument = await _closureDocumentRepo.FindClosureDocumentByCaption(caption);
            if (closureDocument == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var closureDocumentTransferDTOs = _mapper.Map<ClosureDocumentTransferDTO>(closureDocument);
            return CommonResponse.Send(ResponseCodes.SUCCESS,closureDocumentTransferDTOs);
        }

        public async Task<ApiCommonResponse> UpdateClosureDocument(HttpContext context, long id, ClosureDocumentReceivingDTO closureDocumentReceivingDTO)
        {
            var closureDocumentToUpdate = await _closureDocumentRepo.FindClosureDocumentById(id);
            if (closureDocumentToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
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
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "ClosureDocument",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedClosureDocument.Id
            };

            await _historyRepo.SaveHistory(history);

            var closureDocumentTransferDTOs = _mapper.Map<ClosureDocumentTransferDTO>(updatedClosureDocument);
            return CommonResponse.Send(ResponseCodes.SUCCESS,closureDocumentTransferDTOs);

        }

        public async Task<ApiCommonResponse> DeleteClosureDocument(long id)
        {
            var closureDocumentToDelete = await _closureDocumentRepo.FindClosureDocumentById(id);
            if (closureDocumentToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _closureDocumentRepo.DeleteClosureDocument(closureDocumentToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }
    }
}