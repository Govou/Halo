using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Helpers;
using HaloBiz.Model;
using HaloBiz.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace HaloBiz.MyServices.Impl
{
    public class EngagementReasonServiceImpl : IEngagementReasonService
    {
        private readonly ILogger<EngagementReasonServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IEngagementReasonRepository _engagementReasonRepo;
        private readonly IMapper _mapper;

        public EngagementReasonServiceImpl(IModificationHistoryRepository historyRepo, IEngagementReasonRepository engagementReasonRepo, ILogger<EngagementReasonServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._engagementReasonRepo = engagementReasonRepo;
            this._logger = logger;
        }
        public async  Task<ApiResponse> AddEngagementReason(HttpContext context, EngagementReasonReceivingDTO engagementReasonReceivingDTO)
        {
            var engagementReason = _mapper.Map<EngagementReason>(engagementReasonReceivingDTO);
            engagementReason.CreatedById = context.GetLoggedInUserId();
            var savedEngagementReason = await _engagementReasonRepo.SaveEngagementReason(engagementReason);
            if (savedEngagementReason == null)
            {
                return new ApiResponse(500);
            }
            var engagementReasonTransferDTO = _mapper.Map<EngagementReasonTransferDTO>(engagementReason);
            return new ApiOkResponse(engagementReasonTransferDTO);
        }

        public async Task<ApiResponse> DeleteEngagementReason(long id)
        {
            var engagementReasonToDelete = await _engagementReasonRepo.FindEngagementReasonById(id);
            if(engagementReasonToDelete == null)
            {
                return new ApiResponse(404);
            }
            if (!await _engagementReasonRepo.DeleteEngagementReason(engagementReasonToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> GetAllEngagementReason()
        {
            var engagementReason = await _engagementReasonRepo.GetEngagementReasons();
            if (engagementReason == null)
            {
                return new ApiResponse(404);
            }
            var engagementReasonTransferDTO = _mapper.Map<IEnumerable<EngagementReasonTransferDTO>>(engagementReason);
            return new ApiOkResponse(engagementReasonTransferDTO);
        }

        public  async Task<ApiResponse> UpdateEngagementReason(HttpContext context, long id, EngagementReasonReceivingDTO engagementReasonReceivingDTO)
        {
            var engagementReasonToUpdate = await _engagementReasonRepo.FindEngagementReasonById(id);
            if (engagementReasonToUpdate == null)
            {
                return new ApiResponse(404);
            }
            
            var summary = $"Initial details before change, \n {engagementReasonToUpdate.ToString()} \n" ;

            engagementReasonToUpdate.Caption = engagementReasonReceivingDTO.Caption;
            engagementReasonToUpdate.Description = engagementReasonReceivingDTO.Description;
            var updatedEngagementReason = await _engagementReasonRepo.UpdateEngagementReason(engagementReasonToUpdate);

            summary += $"Details after change, \n {updatedEngagementReason.ToString()} \n";

            if (updatedEngagementReason == null)
            {
                return new ApiResponse(500);
            }
            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "EngagementReason",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedEngagementReason.Id
            };

            await _historyRepo.SaveHistory(history);

            var engagementReasonTransferDTOs = _mapper.Map<EngagementReasonTransferDTO>(updatedEngagementReason);
            return new ApiOkResponse(engagementReasonTransferDTOs);
        }
    }
}