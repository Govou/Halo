using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.Helpers;
using HaloBiz.Model;
using HaloBiz.Model.LAMS;
using HaloBiz.Model.ManyToManyRelationship;
using HaloBiz.MyServices.LAMS;
using HaloBiz.Repository;
using HaloBiz.Repository.LAMS;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace HaloBiz.MyServices.Impl.LAMS
{
    public class LeadEngagementServiceImpl : ILeadEngagementService
    {
        private readonly ILogger<LeadEngagementServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly ILeadEngagementRepository _leadEngagementRepo;
        private readonly IMapper _mapper;

        public LeadEngagementServiceImpl(IModificationHistoryRepository historyRepo, ILeadEngagementRepository leadEngagementRepo, ILogger<LeadEngagementServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._leadEngagementRepo = leadEngagementRepo;
            this._logger = logger;
        }

        public async Task<ApiResponse> AddLeadEngagement(HttpContext context, LeadEngagementReceivingDTO leadEngagementReceivingDTO)
        {
            var leadEngagement = _mapper.Map<LeadEngagement>(leadEngagementReceivingDTO);

            leadEngagement.LeadDivisionContactLeadEngagements = leadEngagementReceivingDTO.ContactsEngagedIds
                .Select(x => new LeadDivisionContactLeadEngagement { ContactsEngagedWithId = x }).ToList();

            leadEngagement.LeadDivisionKeyPersonLeadEngagements = leadEngagementReceivingDTO.KeyPersonsEngagedIds
                .Select(x => new LeadDivisionKeyPersonLeadEngagement { KeyPersonsEngagedWithId = x }).ToList();

            leadEngagement.LeadEngagementUserProfiles = leadEngagementReceivingDTO.UsersEngagedIds
                .Select(x => new LeadEngagementUserProfile { UsersEngagedWithId = x }).ToList();

            leadEngagement.CreatedById = context.GetLoggedInUserId();
            var savedLeadEngagement = await _leadEngagementRepo.SaveLeadEngagement(leadEngagement);
            if (savedLeadEngagement == null)
            {
                return new ApiResponse(500);
            }
            var leadEngagementTransferDTO = _mapper.Map<LeadEngagementTransferDTO>(savedLeadEngagement);
            return new ApiOkResponse(leadEngagementTransferDTO);
        }

        public async Task<ApiResponse> GetAllLeadEngagement()
        {
            var leadEngagements = await _leadEngagementRepo.FindAllLeadEngagement();
            if (leadEngagements == null)
            {
                return new ApiResponse(404);
            }
            var leadEngagementTransferDTO = _mapper.Map<IEnumerable<LeadEngagementTransferDTO>>(leadEngagements);
            return new ApiOkResponse(leadEngagementTransferDTO);
        }

        public async Task<ApiResponse> GetLeadEngagementById(long id)
        {
            var leadEngagement = await _leadEngagementRepo.FindLeadEngagementById(id);
            if (leadEngagement == null)
            {
                return new ApiResponse(404);
            }
            var leadEngagementTransferDTOs = _mapper.Map<LeadEngagementTransferDTO>(leadEngagement);
            return new ApiOkResponse(leadEngagementTransferDTOs);
        }

        public async Task<ApiResponse> FindLeadEngagementsByLeadId(long leadId)
        {
            var leadEngagement = await _leadEngagementRepo.FindLeadEngagementsByLeadId(leadId);
            if (leadEngagement == null)
            {
                return new ApiResponse(404);
            }
            var leadEngagementTransferDTOs = _mapper.Map<List<LeadEngagementTransferDTO>>(leadEngagement);
            return new ApiOkResponse(leadEngagementTransferDTOs);
        }

        public async Task<ApiResponse> GetLeadEngagementByName(string name)
        {
            var leadEngagement = await _leadEngagementRepo.FindLeadEngagementByName(name);
            if (leadEngagement == null)
            {
                return new ApiResponse(404);
            }
            var leadEngagementTransferDTOs = _mapper.Map<LeadEngagementTransferDTO>(leadEngagement);
            return new ApiOkResponse(leadEngagementTransferDTOs);
        }

        public async Task<ApiResponse> UpdateLeadEngagement(HttpContext context, long id, LeadEngagementReceivingDTO leadEngagementReceivingDTO)
        {
            var leadEngagementToUpdate = await _leadEngagementRepo.FindLeadEngagementById(id);
            if (leadEngagementToUpdate == null)
            {
                return new ApiResponse(404);
            }
            
            var summary = $"Initial details before change, \n {leadEngagementToUpdate.ToString()} \n" ;

            leadEngagementToUpdate.Caption = leadEngagementReceivingDTO.Caption;
            leadEngagementToUpdate.Date = leadEngagementReceivingDTO.Date;
            leadEngagementToUpdate.EngagementDiscussion = leadEngagementReceivingDTO.EngagementDiscussion;
            leadEngagementToUpdate.EngagementOutcome = leadEngagementReceivingDTO.EngagementOutcome;
            leadEngagementToUpdate.EngagementTypeId = leadEngagementReceivingDTO.EngagementTypeId;
            leadEngagementToUpdate.LeadCaptureStage = leadEngagementReceivingDTO.LeadCaptureStage;
            leadEngagementToUpdate.EngagementReasonId = leadEngagementReceivingDTO.EngagementReasonId;
            leadEngagementToUpdate.LeadId = leadEngagementReceivingDTO.LeadId;
            var updatedLeadEngagement = await _leadEngagementRepo.UpdateLeadEngagement(leadEngagementToUpdate);

            summary += $"Details after change, \n {updatedLeadEngagement.ToString()} \n";

            if (updatedLeadEngagement == null)
            {
                return new ApiResponse(500);
            }
            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "LeadEngagement",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedLeadEngagement.Id
            };

            await _historyRepo.SaveHistory(history);

            var leadEngagementTransferDTOs = _mapper.Map<LeadEngagementTransferDTO>(updatedLeadEngagement);
            return new ApiOkResponse(leadEngagementTransferDTOs);

        }

        public async Task<ApiResponse> DeleteLeadEngagement(long id)
        {
            var leadEngagementToDelete = await _leadEngagementRepo.FindLeadEngagementById(id);
            if (leadEngagementToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _leadEngagementRepo.DeleteLeadEngagement(leadEngagementToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }
    }
}