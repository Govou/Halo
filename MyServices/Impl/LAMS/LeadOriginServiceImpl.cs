using System.Collections.Generic;
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
    public class LeadOriginServiceImpl : ILeadOriginService
    {
        
        private readonly ILogger<LeadOriginServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly ILeadOriginRepository _leadOriginRepo;
        private readonly IMapper _mapper;

        public LeadOriginServiceImpl(IModificationHistoryRepository historyRepo, 
        ILeadOriginRepository leadOriginRepo, ILogger<LeadOriginServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._leadOriginRepo = leadOriginRepo;
            this._logger = logger;
        }

        public async Task<ApiResponse> AddLeadOrigin(HttpContext context, LeadOriginReceivingDTO leadOriginReceivingDTO)
        {
            var leadOrigin = _mapper.Map<LeadOrigin>(leadOriginReceivingDTO);
            leadOrigin.CreatedById = context.GetLoggedInUserId();
            var savedLeadOrigin = await _leadOriginRepo.SaveLeadOrigin(leadOrigin);
            if (savedLeadOrigin == null)
            {
                return new ApiResponse(500);
            }
            var leadOriginTransferDTO = _mapper.Map<LeadOriginTransferDTO>(savedLeadOrigin);
            return new ApiOkResponse(leadOriginTransferDTO);
        }

        public async Task<ApiResponse> GetAllLeadOrigin()
        {
            var leadOrigins = await _leadOriginRepo.FindAllLeadOrigin();
            if (leadOrigins == null)
            {
                return new ApiResponse(404);
            }
            var leadOriginTransferDTO = _mapper.Map<IEnumerable<LeadOriginTransferDTO>>(leadOrigins);
            return new ApiOkResponse(leadOriginTransferDTO);
        }

        public async Task<ApiResponse> GetLeadOriginById(long id)
        {
            var leadOrigin = await _leadOriginRepo.FindLeadOriginById(id);
            if (leadOrigin == null)
            {
                return new ApiResponse(404);
            }
            var leadOriginTransferDTO = _mapper.Map<LeadOriginTransferDTO>(leadOrigin);
            return new ApiOkResponse(leadOriginTransferDTO);
        }

        public async Task<ApiResponse> GetLeadOriginByName(string name)
        {
            var leadOrigin = await _leadOriginRepo.FindLeadOriginByName(name);
            if (leadOrigin == null)
            {
                return new ApiResponse(404);
            }
            var leadOriginTransferDTOs = _mapper.Map<LeadOriginTransferDTO>(leadOrigin);
            return new ApiOkResponse(leadOriginTransferDTOs);
        }

        public async Task<ApiResponse> UpdateLeadOrigin(HttpContext context, long id, LeadOriginReceivingDTO LeadOriginReceivingDTO)
        {
            var leadOriginToUpdate = await _leadOriginRepo.FindLeadOriginById(id);
            if (leadOriginToUpdate == null)
            {
                return new ApiResponse(404);
            }
            
            var summary = $"Initial details before change, \n {leadOriginToUpdate.ToString()} \n" ;

            leadOriginToUpdate.Caption = LeadOriginReceivingDTO.Caption;
            leadOriginToUpdate.Description = LeadOriginReceivingDTO.Description;
            leadOriginToUpdate.LeadTypeId = LeadOriginReceivingDTO.LeadTypeId;
            var updatedLeadOrigin = await _leadOriginRepo.UpdateLeadOrigin(leadOriginToUpdate);

            summary += $"Details after change, \n {updatedLeadOrigin.ToString()} \n";

            if (updatedLeadOrigin == null)
            {
                return new ApiResponse(500);
            }
            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "LeadOrigin",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedLeadOrigin.Id
            };

            await _historyRepo.SaveHistory(history);

            var leadOriginTransferDTOs = _mapper.Map<LeadOriginTransferDTO>(updatedLeadOrigin);
            return new ApiOkResponse(leadOriginTransferDTOs);

        }

        public async Task<ApiResponse> DeleteLeadOrigin(long id)
        {
            var leadOriginToDelete = await _leadOriginRepo.FindLeadOriginById(id);
            if (leadOriginToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _leadOriginRepo.DeleteLeadOrigin(leadOriginToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }
    }
}