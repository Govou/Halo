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
    public class EndorsementTypeServiceImpl : IEndorsementTypeService
    {
        private readonly ILogger<EndorsementTypeServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IEndorsementTypeRepository _endorsementTypeRepo;
        private readonly IMapper _mapper;

        public EndorsementTypeServiceImpl(IModificationHistoryRepository historyRepo, IEndorsementTypeRepository endorsementTypeRepo, ILogger<EndorsementTypeServiceImpl> logger, IMapper mapper)
        {
            _mapper = mapper;
            _historyRepo = historyRepo;
            _endorsementTypeRepo = endorsementTypeRepo;
            _logger = logger;
        }

        public async Task<ApiResponse> AddEndorsementType(HttpContext context, EndorsementTypeReceivingDTO endorsementTypeReceivingDTO)
        {
            var endorsementType = _mapper.Map<EndorsementType>(endorsementTypeReceivingDTO);
            endorsementType.CreatedById = context.GetLoggedInUserId();
            var savedEndorsementType = await _endorsementTypeRepo.SaveEndorsementType(endorsementType);
            if (savedEndorsementType == null)
            {
                return new ApiResponse(500);
            }
            var endorsementTypeTransferDTO = _mapper.Map<EndorsementTypeTransferDTO>(savedEndorsementType);
            return new ApiOkResponse(endorsementTypeTransferDTO);
        }

        public async Task<ApiResponse> GetAllEndorsementType()
        {
            var endorsementTypes = await _endorsementTypeRepo.FindAllEndorsementType();
            if (endorsementTypes == null)
            {
                return new ApiResponse(404);
            }
            var endorsementTypeTransferDTO = _mapper.Map<IEnumerable<EndorsementTypeTransferDTO>>(endorsementTypes);
            return new ApiOkResponse(endorsementTypeTransferDTO);
        }

        public async Task<ApiResponse> GetEndorsementTypeById(long id)
        {
            var endorsementType = await _endorsementTypeRepo.FindEndorsementTypeById(id);
            if (endorsementType == null)
            {
                return new ApiResponse(404);
            }
            var endorsementTypeTransferDTOs = _mapper.Map<EndorsementTypeTransferDTO>(endorsementType);
            return new ApiOkResponse(endorsementTypeTransferDTOs);
        }

        public async Task<ApiResponse> GetEndorsementTypeByName(string name)
        {
            var endorsementType = await _endorsementTypeRepo.FindEndorsementTypeByName(name);
            if (endorsementType == null)
            {
                return new ApiResponse(404);
            }
            var endorsementTypeTransferDTOs = _mapper.Map<EndorsementTypeTransferDTO>(endorsementType);
            return new ApiOkResponse(endorsementTypeTransferDTOs);
        }

        public async Task<ApiResponse> UpdateEndorsementType(HttpContext context, long id, EndorsementTypeReceivingDTO endorsementTypeReceivingDTO)
        {
            var endorsementTypeToUpdate = await _endorsementTypeRepo.FindEndorsementTypeById(id);
            if (endorsementTypeToUpdate == null)
            {
                return new ApiResponse(404);
            }
            
            var summary = $"Initial details before change, \n {endorsementTypeToUpdate} \n" ;

            endorsementTypeToUpdate.Caption = endorsementTypeReceivingDTO.Caption;
            endorsementTypeToUpdate.Description = endorsementTypeReceivingDTO.Description;
            var updatedEndorsementType = await _endorsementTypeRepo.UpdateEndorsementType(endorsementTypeToUpdate);

            summary += $"Details after change, \n {updatedEndorsementType} \n";

            if (updatedEndorsementType == null)
            {
                return new ApiResponse(500);
            }
            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "EndorsementType",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedEndorsementType.Id
            };

            await _historyRepo.SaveHistory(history);

            var endorsementTypeTransferDTOs = _mapper.Map<EndorsementTypeTransferDTO>(updatedEndorsementType);
            return new ApiOkResponse(endorsementTypeTransferDTOs);

        }

        public async Task<ApiResponse> DeleteEndorsementType(long id)
        {
            var endorsementTypeToDelete = await _endorsementTypeRepo.FindEndorsementTypeById(id);
            if (endorsementTypeToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _endorsementTypeRepo.DeleteEndorsementType(endorsementTypeToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }
    }
}