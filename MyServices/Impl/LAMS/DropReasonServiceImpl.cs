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
    public class DropReasonServiceImpl : IDropReasonService
    {
        private readonly ILogger<DropReasonServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IDropReasonRepository _DropReasonRepo;
        private readonly IMapper _mapper;

        public DropReasonServiceImpl(IModificationHistoryRepository historyRepo, IDropReasonRepository DropReasonRepo, ILogger<DropReasonServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._DropReasonRepo = DropReasonRepo;
            this._logger = logger;
        }

        public async Task<ApiResponse> AddDropReason(HttpContext context, DropReasonReceivingDTO DropReasonReceivingDTO)
        {
            var DropReason = _mapper.Map<DropReason>(DropReasonReceivingDTO);
            DropReason.CreatedById = context.GetLoggedInUserId();
            var savedDropReason = await _DropReasonRepo.SaveDropReason(DropReason);
            if (savedDropReason == null)
            {
                return new ApiResponse(500);
            }
            var DropReasonTransferDTO = _mapper.Map<DropReasonTransferDTO>(DropReason);
            return new ApiOkResponse(DropReasonTransferDTO);
        }

        public async Task<ApiResponse> GetAllDropReason()
        {
            var DropReasons = await _DropReasonRepo.FindAllDropReason();
            if (DropReasons == null)
            {
                return new ApiResponse(404);
            }
            var DropReasonTransferDTO = _mapper.Map<IEnumerable<DropReasonTransferDTO>>(DropReasons);
            return new ApiOkResponse(DropReasonTransferDTO);
        }

        public async Task<ApiResponse> GetDropReasonById(long id)
        {
            var DropReason = await _DropReasonRepo.FindDropReasonById(id);
            if (DropReason == null)
            {
                return new ApiResponse(404);
            }
            var DropReasonTransferDTOs = _mapper.Map<DropReasonTransferDTO>(DropReason);
            return new ApiOkResponse(DropReasonTransferDTOs);
        }

        public async Task<ApiResponse> GetDropReasonByTitle(string title)
        {
            var DropReason = await _DropReasonRepo.FindDropReasonByTitle(title);
            if (DropReason == null)
            {
                return new ApiResponse(404);
            }
            var DropReasonTransferDTOs = _mapper.Map<DropReasonTransferDTO>(DropReason);
            return new ApiOkResponse(DropReasonTransferDTOs);
        }

        public async Task<ApiResponse> UpdateDropReason(HttpContext context, long id, DropReasonReceivingDTO DropReasonReceivingDTO)
        {
            var DropReasonToUpdate = await _DropReasonRepo.FindDropReasonById(id);
            if (DropReasonToUpdate == null)
            {
                return new ApiResponse(404);
            }
            
            var summary = $"Initial details before change, \n {DropReasonToUpdate.ToString()} \n" ;

            DropReasonToUpdate.Title = DropReasonReceivingDTO.Title;
            var updatedDropReason = await _DropReasonRepo.UpdateDropReason(DropReasonToUpdate);

            summary += $"Details after change, \n {updatedDropReason.ToString()} \n";

            if (updatedDropReason == null)
            {
                return new ApiResponse(500);
            }
            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "DropReason",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedDropReason.Id
            };

            await _historyRepo.SaveHistory(history);

            var DropReasonTransferDTOs = _mapper.Map<DropReasonTransferDTO>(updatedDropReason);
            return new ApiOkResponse(DropReasonTransferDTOs);

        }

        public async Task<ApiResponse> DeleteDropReason(long id)
        {
            var DropReasonToDelete = await _DropReasonRepo.FindDropReasonById(id);
            if (DropReasonToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _DropReasonRepo.DeleteDropReason(DropReasonToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }
    }
}