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
    public class LeadTypeServiceImpl : ILeadTypeService
    {
        private readonly ILogger<LeadTypeServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly ILeadTypeRepository _leadTypeRepo;
        private readonly IMapper _mapper;

        public LeadTypeServiceImpl(IModificationHistoryRepository historyRepo, ILeadTypeRepository leadTypeRepo, ILogger<LeadTypeServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._leadTypeRepo = leadTypeRepo;
            this._logger = logger;
        }

        public async Task<ApiResponse> AddLeadType(HttpContext context, LeadTypeReceivingDTO leadTypeReceivingDTO)
        {
            var leadType = _mapper.Map<LeadType>(leadTypeReceivingDTO);
            leadType.CreatedById = context.GetLoggedInUserId();
            var savedLeadType = await _leadTypeRepo.SaveLeadType(leadType);
            if (savedLeadType == null)
            {
                return new ApiResponse(500);
            }
            var leadTypeTransferDTO = _mapper.Map<LeadTypeTransferDTO>(leadType);
            return new ApiOkResponse(leadTypeTransferDTO);
        }

        public async Task<ApiResponse> GetAllLeadType()
        {
            var leadTypes = await _leadTypeRepo.FindAllLeadType();
            if (leadTypes == null)
            {
                return new ApiResponse(404);
            }
            var leadTypeTransferDTO = _mapper.Map<IEnumerable<LeadTypeTransferDTO>>(leadTypes);
            return new ApiOkResponse(leadTypeTransferDTO);
        }

        public async Task<ApiResponse> GetLeadTypeById(long id)
        {
            var leadType = await _leadTypeRepo.FindLeadTypeById(id);
            if (leadType == null)
            {
                return new ApiResponse(404);
            }
            var leadTypeTransferDTOs = _mapper.Map<LeadTypeTransferDTO>(leadType);
            return new ApiOkResponse(leadTypeTransferDTOs);
        }

        public async Task<ApiResponse> GetLeadTypeByName(string name)
        {
            var leadType = await _leadTypeRepo.FindLeadTypeByName(name);
            if (leadType == null)
            {
                return new ApiResponse(404);
            }
            var leadTypeTransferDTOs = _mapper.Map<LeadTypeTransferDTO>(leadType);
            return new ApiOkResponse(leadTypeTransferDTOs);
        }

        public async Task<ApiResponse> UpdateLeadType(HttpContext context, long id, LeadTypeReceivingDTO leadTypeReceivingDTO)
        {
            var leadTypeToUpdate = await _leadTypeRepo.FindLeadTypeById(id);
            if (leadTypeToUpdate == null)
            {
                return new ApiResponse(404);
            }
            
            var summary = $"Initial details before change, \n {leadTypeToUpdate.ToString()} \n" ;

            leadTypeToUpdate.Caption = leadTypeReceivingDTO.Caption;
            leadTypeToUpdate.Description = leadTypeReceivingDTO.Description;
            var updatedLeadType = await _leadTypeRepo.UpdateLeadType(leadTypeToUpdate);

            summary += $"Details after change, \n {updatedLeadType.ToString()} \n";

            if (updatedLeadType == null)
            {
                return new ApiResponse(500);
            }
            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "LeadType",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedLeadType.Id
            };

            await _historyRepo.SaveHistory(history);

            var leadTypeTransferDTOs = _mapper.Map<LeadTypeTransferDTO>(updatedLeadType);
            return new ApiOkResponse(leadTypeTransferDTOs);

        }

        public async Task<ApiResponse> DeleteLeadType(long id)
        {
            var leadTypeToDelete = await _leadTypeRepo.FindLeadTypeById(id);
            if (leadTypeToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _leadTypeRepo.DeleteLeadType(leadTypeToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }
    }
}