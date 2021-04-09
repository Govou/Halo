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
    public class EngagementTypeServiceImpl : IEngagementTypeService
    {
        private readonly ILogger<EngagementTypeServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IEngagementTypeRepository _engagementTypeRepo;
        private readonly IMapper _mapper;

        public EngagementTypeServiceImpl(IModificationHistoryRepository historyRepo, IEngagementTypeRepository engagementTypeRepo, ILogger<EngagementTypeServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._engagementTypeRepo = engagementTypeRepo;
            this._logger = logger;
        }
        public async  Task<ApiResponse> AddEngagementType(HttpContext context, EngagementTypeReceivingDTO engagementTypeReceivingDTO)
        {
            var engagementType = _mapper.Map<EngagementType>(engagementTypeReceivingDTO);
            engagementType.CreatedById = context.GetLoggedInUserId();
            var savedEngagementType = await _engagementTypeRepo.SaveEngagementType(engagementType);
            if (savedEngagementType == null)
            {
                return new ApiResponse(500);
            }
            var engagementTypeTransferDTO = _mapper.Map<EngagementTypeTransferDTO>(engagementType);
            return new ApiOkResponse(engagementTypeTransferDTO);
        }

        public async Task<ApiResponse> DeleteEngagementType(long id)
        {
            var engagementTypeToDelete = await _engagementTypeRepo.FindEngagementTypeById(id);
            if(engagementTypeToDelete == null)
            {
                return new ApiResponse(404);
            }
            if (!await _engagementTypeRepo.DeleteEngagementType(engagementTypeToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> GetAllEngagementType()
        {
            var engagementType = await _engagementTypeRepo.GetEngagementTypes();
            if (engagementType == null)
            {
                return new ApiResponse(404);
            }
            var engagementTypeTransferDTO = _mapper.Map<IEnumerable<EngagementTypeTransferDTO>>(engagementType);
            return new ApiOkResponse(engagementTypeTransferDTO);
        }

        public  async Task<ApiResponse> UpdateEngagementType(HttpContext context, long id, EngagementTypeReceivingDTO engagementTypeReceivingDTO)
        {
            var engagementTypeToUpdate = await _engagementTypeRepo.FindEngagementTypeById(id);
            if (engagementTypeToUpdate == null)
            {
                return new ApiResponse(404);
            }
            
            var summary = $"Initial details before change, \n {engagementTypeToUpdate.ToString()} \n" ;

            engagementTypeToUpdate.Caption = engagementTypeReceivingDTO.Caption;
            engagementTypeToUpdate.Description = engagementTypeReceivingDTO.Description;
            var updatedEngagementType = await _engagementTypeRepo.UpdateEngagementType(engagementTypeToUpdate);

            summary += $"Details after change, \n {updatedEngagementType.ToString()} \n";

            if (updatedEngagementType == null)
            {
                return new ApiResponse(500);
            }
            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "EngagementType",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedEngagementType.Id
            };

            await _historyRepo.SaveHistory(history);

            var engagementTypeTransferDTOs = _mapper.Map<EngagementTypeTransferDTO>(updatedEngagementType);
            return new ApiOkResponse(engagementTypeTransferDTOs);
        }
    }
}