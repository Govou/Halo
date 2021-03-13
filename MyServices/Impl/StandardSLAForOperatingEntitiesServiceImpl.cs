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
    public class StandardSLAForOperatingEntitiesServiceImpl : IStandardSLAForOperatingEntitiesService
    {
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IStandardSLAForOperatingEntitiesRepository _standardSLAForOperatingEntitiesRepo;
        private readonly IMapper _mapper;

        public StandardSLAForOperatingEntitiesServiceImpl(IModificationHistoryRepository historyRepo, IStandardSLAForOperatingEntitiesRepository StandardSLAForOperatingEntitiesRepo, IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._standardSLAForOperatingEntitiesRepo = StandardSLAForOperatingEntitiesRepo;
        }

        public async Task<ApiResponse> AddStandardSLAForOperatingEntities(HttpContext context, StandardSLAForOperatingEntitiesReceivingDTO standardSLAForOperatingEntitiesReceivingDTO)
        {
            var standardSLAForOperatingEntities = _mapper.Map<StandardSLAForOperatingEntities>(standardSLAForOperatingEntitiesReceivingDTO);
            standardSLAForOperatingEntities.CreatedById = context.GetLoggedInUserId();
            var savedStandardSLAForOperatingEntities = await _standardSLAForOperatingEntitiesRepo.SaveStandardSLAForOperatingEntities(standardSLAForOperatingEntities);
            if (savedStandardSLAForOperatingEntities == null)
            {
                return new ApiResponse(500);
            }
            var standardSLAForOperatingEntitiesTransferDTO = _mapper.Map<StandardSLAForOperatingEntitiesTransferDTO>(standardSLAForOperatingEntities);
            return new ApiOkResponse(standardSLAForOperatingEntitiesTransferDTO);
        }

        public async Task<ApiResponse> GetAllStandardSLAForOperatingEntities()
        {
            var standardSLAForOperatingEntitiess = await _standardSLAForOperatingEntitiesRepo.FindAllStandardSLAForOperatingEntities();
            if (standardSLAForOperatingEntitiess == null)
            {
                return new ApiResponse(404);
            }
            var standardSLAForOperatingEntitiesTransferDTO = _mapper.Map<IEnumerable<StandardSLAForOperatingEntitiesTransferDTO>>(standardSLAForOperatingEntitiess);
            return new ApiOkResponse(standardSLAForOperatingEntitiesTransferDTO);
        }

        public async Task<ApiResponse> GetStandardSLAForOperatingEntitiesById(long id)
        {
            var standardSLAForOperatingEntities = await _standardSLAForOperatingEntitiesRepo.FindStandardSLAForOperatingEntitiesById(id);
            if (standardSLAForOperatingEntities == null)
            {
                return new ApiResponse(404);
            }
            var standardSLAForOperatingEntitiesTransferDTOs = _mapper.Map<StandardSLAForOperatingEntitiesTransferDTO>(standardSLAForOperatingEntities);
            return new ApiOkResponse(standardSLAForOperatingEntitiesTransferDTOs);
        }

        public async Task<ApiResponse> GetStandardSLAForOperatingEntitiesByName(string name)
        {
            var standardSLAForOperatingEntities = await _standardSLAForOperatingEntitiesRepo.FindStandardSLAForOperatingEntitiesByName(name);
            if (standardSLAForOperatingEntities == null)
            {
                return new ApiResponse(404);
            }
            var standardSLAForOperatingEntitiesTransferDTOs = _mapper.Map<StandardSLAForOperatingEntitiesTransferDTO>(standardSLAForOperatingEntities);
            return new ApiOkResponse(standardSLAForOperatingEntitiesTransferDTOs);
        }

        public async Task<ApiResponse> UpdateStandardSLAForOperatingEntities(HttpContext context, long id, StandardSLAForOperatingEntitiesReceivingDTO standardSLAForOperatingEntitiesReceivingDTO)
        {
            var standardSLAForOperatingEntitiesToUpdate = await _standardSLAForOperatingEntitiesRepo.FindStandardSLAForOperatingEntitiesById(id);
            if (standardSLAForOperatingEntitiesToUpdate == null)
            {
                return new ApiResponse(404);
            }
            
            var summary = $"Initial details before change, \n {standardSLAForOperatingEntitiesToUpdate.ToString()} \n" ;

            standardSLAForOperatingEntitiesToUpdate.Caption = standardSLAForOperatingEntitiesReceivingDTO.Caption;
            standardSLAForOperatingEntitiesToUpdate.Description = standardSLAForOperatingEntitiesReceivingDTO.Description;
            standardSLAForOperatingEntitiesToUpdate.OperatingEntityId = standardSLAForOperatingEntitiesReceivingDTO.OperatingEntityId;
            standardSLAForOperatingEntitiesToUpdate.DocumentUrl = standardSLAForOperatingEntitiesReceivingDTO.DocumentUrl;
            var updatedStandardSLAForOperatingEntities = await _standardSLAForOperatingEntitiesRepo.UpdateStandardSLAForOperatingEntities(standardSLAForOperatingEntitiesToUpdate);

            summary += $"Details after change, \n {updatedStandardSLAForOperatingEntities.ToString()} \n";

            if (updatedStandardSLAForOperatingEntities == null)
            {
                return new ApiResponse(500);
            }
            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "StandardSLAForOperatingEntities",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedStandardSLAForOperatingEntities.Id
            };

            await _historyRepo.SaveHistory(history);

            var standardSLAForOperatingEntitiesTransferDTOs = _mapper.Map<StandardSLAForOperatingEntitiesTransferDTO>(updatedStandardSLAForOperatingEntities);
            return new ApiOkResponse(standardSLAForOperatingEntitiesTransferDTOs);

        }

        public async Task<ApiResponse> DeleteStandardSLAForOperatingEntities(long id)
        {
            var standardSLAForOperatingEntitiesToDelete = await _standardSLAForOperatingEntitiesRepo.FindStandardSLAForOperatingEntitiesById(id);
            if (standardSLAForOperatingEntitiesToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _standardSLAForOperatingEntitiesRepo.DeleteStandardSLAForOperatingEntities(standardSLAForOperatingEntitiesToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }
    }
}