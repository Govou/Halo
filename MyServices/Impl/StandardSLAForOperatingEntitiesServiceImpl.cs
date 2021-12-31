using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Helpers;
using HalobizMigrations.Models;
using HaloBiz.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace HaloBiz.MyServices.Impl
{
    public class StandardSlaforOperatingEntityServiceImpl : IStandardSlaforOperatingEntityService
    {
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IStandardSlaforOperatingEntityRepository _standardSLAForOperatingEntitiesRepo;
        private readonly IMapper _mapper;

        public StandardSlaforOperatingEntityServiceImpl(IModificationHistoryRepository historyRepo, IStandardSlaforOperatingEntityRepository StandardSlaforOperatingEntityRepo, IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._standardSLAForOperatingEntitiesRepo = StandardSlaforOperatingEntityRepo;
        }

        public async Task<ApiResponse> AddStandardSlaforOperatingEntity(HttpContext context, StandardSlaforOperatingEntityReceivingDTO standardSLAForOperatingEntitiesReceivingDTO)
        {
            var standardSLAForOperatingEntities = _mapper.Map<StandardSlaforOperatingEntity>(standardSLAForOperatingEntitiesReceivingDTO);
            standardSLAForOperatingEntities.CreatedById = context.GetLoggedInUserId();
            var savedStandardSlaforOperatingEntity = await _standardSLAForOperatingEntitiesRepo.SaveStandardSlaforOperatingEntity(standardSLAForOperatingEntities);
            if (savedStandardSlaforOperatingEntity == null)
            {
                return new ApiResponse(500);
            }
            var standardSLAForOperatingEntitiesTransferDTO = _mapper.Map<StandardSlaforOperatingEntityTransferDTO>(standardSLAForOperatingEntities);
            return new ApiOkResponse(standardSLAForOperatingEntitiesTransferDTO);
        }

        public async Task<ApiResponse> GetAllStandardSlaforOperatingEntity()
        {
            var standardSLAForOperatingEntitiess = await _standardSLAForOperatingEntitiesRepo.FindAllStandardSlaforOperatingEntity();
            if (standardSLAForOperatingEntitiess == null)
            {
                return new ApiResponse(404);
            }
            var standardSLAForOperatingEntitiesTransferDTO = _mapper.Map<IEnumerable<StandardSlaforOperatingEntityTransferDTO>>(standardSLAForOperatingEntitiess);
            return new ApiOkResponse(standardSLAForOperatingEntitiesTransferDTO);
        }

        public async Task<ApiResponse> GetStandardSlaforOperatingEntityById(long id)
        {
            var standardSLAForOperatingEntities = await _standardSLAForOperatingEntitiesRepo.FindStandardSlaforOperatingEntityById(id);
            if (standardSLAForOperatingEntities == null)
            {
                return new ApiResponse(404);
            }
            var standardSLAForOperatingEntitiesTransferDTOs = _mapper.Map<StandardSlaforOperatingEntityTransferDTO>(standardSLAForOperatingEntities);
            return new ApiOkResponse(standardSLAForOperatingEntitiesTransferDTOs);
        }

        public async Task<ApiResponse> GetStandardSlaforOperatingEntityByName(string name)
        {
            var standardSLAForOperatingEntities = await _standardSLAForOperatingEntitiesRepo.FindStandardSlaforOperatingEntityByName(name);
            if (standardSLAForOperatingEntities == null)
            {
                return new ApiResponse(404);
            }
            var standardSLAForOperatingEntitiesTransferDTOs = _mapper.Map<StandardSlaforOperatingEntityTransferDTO>(standardSLAForOperatingEntities);
            return new ApiOkResponse(standardSLAForOperatingEntitiesTransferDTOs);
        }

        public async Task<ApiResponse> UpdateStandardSlaforOperatingEntity(HttpContext context, long id, StandardSlaforOperatingEntityReceivingDTO standardSLAForOperatingEntitiesReceivingDTO)
        {
            var standardSLAForOperatingEntitiesToUpdate = await _standardSLAForOperatingEntitiesRepo.FindStandardSlaforOperatingEntityById(id);
            if (standardSLAForOperatingEntitiesToUpdate == null)
            {
                return new ApiResponse(404);
            }
            
            var summary = $"Initial details before change, \n {standardSLAForOperatingEntitiesToUpdate.ToString()} \n" ;

            standardSLAForOperatingEntitiesToUpdate.Caption = standardSLAForOperatingEntitiesReceivingDTO.Caption;
            standardSLAForOperatingEntitiesToUpdate.Description = standardSLAForOperatingEntitiesReceivingDTO.Description;
            standardSLAForOperatingEntitiesToUpdate.OperatingEntityId = standardSLAForOperatingEntitiesReceivingDTO.OperatingEntityId;
            standardSLAForOperatingEntitiesToUpdate.DocumentUrl = standardSLAForOperatingEntitiesReceivingDTO.DocumentUrl;
            var updatedStandardSlaforOperatingEntity = await _standardSLAForOperatingEntitiesRepo.UpdateStandardSlaforOperatingEntity(standardSLAForOperatingEntitiesToUpdate);

            summary += $"Details after change, \n {updatedStandardSlaforOperatingEntity.ToString()} \n";

            if (updatedStandardSlaforOperatingEntity == null)
            {
                return new ApiResponse(500);
            }
            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "StandardSlaforOperatingEntity",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedStandardSlaforOperatingEntity.Id
            };

            await _historyRepo.SaveHistory(history);

            var standardSLAForOperatingEntitiesTransferDTOs = _mapper.Map<StandardSlaforOperatingEntityTransferDTO>(updatedStandardSlaforOperatingEntity);
            return new ApiOkResponse(standardSLAForOperatingEntitiesTransferDTOs);

        }

        public async Task<ApiResponse> DeleteStandardSlaforOperatingEntity(long id)
        {
            var standardSLAForOperatingEntitiesToDelete = await _standardSLAForOperatingEntitiesRepo.FindStandardSlaforOperatingEntityById(id);
            if (standardSLAForOperatingEntitiesToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _standardSLAForOperatingEntitiesRepo.DeleteStandardSlaforOperatingEntity(standardSLAForOperatingEntitiesToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }
    }
}