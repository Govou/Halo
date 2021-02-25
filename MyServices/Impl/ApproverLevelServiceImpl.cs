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
    public class ApproverLevelServiceImpl : IApproverLevelService
    {
        private readonly ILogger<ApproverLevelServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IApproverLevelRepository _approverLevelRepo;
        private readonly IMapper _mapper;

        public ApproverLevelServiceImpl(IModificationHistoryRepository historyRepo, IApproverLevelRepository approverLevelRepo, ILogger<ApproverLevelServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._approverLevelRepo = approverLevelRepo;
            this._logger = logger;
        }
        public async  Task<ApiResponse> AddApproverLevel(HttpContext context, ApproverLevelReceivingDTO approverLevelReceivingDTO)
        {
            var approverLevel = _mapper.Map<ApproverLevel>(approverLevelReceivingDTO);
            approverLevel.CreatedById = context.GetLoggedInUserId();
            var savedApproverLevel = await _approverLevelRepo.SaveApproverLevel(approverLevel);
            if (savedApproverLevel == null)
            {
                return new ApiResponse(500);
            }
            var approverLevelTransferDTO = _mapper.Map<ApproverLevelTransferDTO>(approverLevel);
            return new ApiOkResponse(approverLevelTransferDTO);
        }

        public async Task<ApiResponse> DeleteApproverLevel(long id)
        {
            var approverLevelToDelete = await _approverLevelRepo.FindApproverLevelById(id);
            if(approverLevelToDelete == null)
            {
                return new ApiResponse(404);
            }
            if (!await _approverLevelRepo.DeleteApproverLevel(approverLevelToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> GetAllApproverLevel()
        {
            var approverLevel = await _approverLevelRepo.GetApproverLevels();
            if (approverLevel == null)
            {
                return new ApiResponse(404);
            }
            var approverLevelTransferDTO = _mapper.Map<IEnumerable<ApproverLevelTransferDTO>>(approverLevel);
            return new ApiOkResponse(approverLevelTransferDTO);
        }

        public  async Task<ApiResponse> UpdateApproverLevel(HttpContext context, long id, ApproverLevelReceivingDTO approverLevelReceivingDTO)
        {
            var approverLevelToUpdate = await _approverLevelRepo.FindApproverLevelById(id);
            if (approverLevelToUpdate == null)
            {
                return new ApiResponse(404);
            }
            
            var summary = $"Initial details before change, \n {approverLevelToUpdate.ToString()} \n" ;

            approverLevelToUpdate.Caption = approverLevelReceivingDTO.Caption;
            approverLevelToUpdate.Alias = approverLevelReceivingDTO.Alias;
            approverLevelToUpdate.Description = approverLevelReceivingDTO.Description;
            var updatedApproverLevel = await _approverLevelRepo.UpdateApproverLevel(approverLevelToUpdate);

            summary += $"Details after change, \n {updatedApproverLevel.ToString()} \n";

            if (updatedApproverLevel == null)
            {
                return new ApiResponse(500);
            }
            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "ApproverLevel",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedApproverLevel.Id
            };

            await _historyRepo.SaveHistory(history);

            var approverLevelTransferDTOs = _mapper.Map<ApproverLevelTransferDTO>(updatedApproverLevel);
            return new ApiOkResponse(approverLevelTransferDTOs);
        }
    }
}