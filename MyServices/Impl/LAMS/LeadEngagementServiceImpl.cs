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
using HalobizMigrations.Models;
using HaloBiz.MyServices.LAMS;
using HaloBiz.Repository;
using HaloBiz.Repository.LAMS;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using HalobizMigrations.Models.Halobiz;
using HalobizMigrations.Data;
using Microsoft.EntityFrameworkCore;

namespace HaloBiz.MyServices.Impl.LAMS
{
    public class LeadEngagementServiceImpl : ILeadEngagementService
    {
        private readonly ILogger<LeadEngagementServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly HalobizContext _context;
        private readonly ILeadEngagementRepository _leadEngagementRepo;
        private readonly IMapper _mapper;

        public LeadEngagementServiceImpl(IModificationHistoryRepository historyRepo, 
            HalobizContext context,
            ILeadEngagementRepository leadEngagementRepo, 
            ILogger<LeadEngagementServiceImpl> logger, 
            IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._leadEngagementRepo = leadEngagementRepo;
            _context = context;
            this._logger = logger;
        }

        public async Task<ApiCommonResponse> AddLeadEngagement(HttpContext context, LeadEngagementReceivingDTO leadEngagementReceivingDTO)
        {
            var leadEngagement = _mapper.Map<LeadEngagement>(leadEngagementReceivingDTO);

            leadEngagement.ContactsEngagedWith = await _context.LeadDivisionContacts.Where(x => !x.IsDeleted && leadEngagementReceivingDTO.ContactsEngagedIds.Contains(x.Id)).ToListAsync();
            leadEngagement.KeyPersonsEngagedWith = await _context.LeadDivisionKeyPeople.Where(x => !x.IsDeleted && leadEngagementReceivingDTO.KeyPersonsEngagedIds.Contains(x.Id)).ToListAsync();
            leadEngagement.UsersEngagedWith = await _context.UserProfiles.Where(x => !x.IsDeleted.Value && leadEngagementReceivingDTO.UsersEngagedIds.Contains(x.Id)).ToListAsync();
            
            leadEngagement.CreatedById = context.GetLoggedInUserId();
            var savedLeadEngagement = await _leadEngagementRepo.SaveLeadEngagement(leadEngagement);
            if (savedLeadEngagement == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var leadEngagementTransferDTO = _mapper.Map<LeadEngagementTransferDTO>(savedLeadEngagement);
            return new ApiOkResponse(leadEngagementTransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllLeadEngagement()
        {
            var leadEngagements = await _leadEngagementRepo.FindAllLeadEngagement();
            if (leadEngagements == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var leadEngagementTransferDTO = _mapper.Map<IEnumerable<LeadEngagementTransferDTO>>(leadEngagements);
            return new ApiOkResponse(leadEngagementTransferDTO);
        }

        public async Task<ApiCommonResponse> GetLeadEngagementById(long id)
        {
            var leadEngagement = await _leadEngagementRepo.FindLeadEngagementById(id);
            if (leadEngagement == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var leadEngagementTransferDTOs = _mapper.Map<LeadEngagementTransferDTO>(leadEngagement);
            return new ApiOkResponse(leadEngagementTransferDTOs);
        }

        public async Task<ApiCommonResponse> FindLeadEngagementsByLeadId(long leadId)
        {
            var leadEngagement = await _leadEngagementRepo.FindLeadEngagementsByLeadId(leadId);
            if (leadEngagement == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var leadEngagementTransferDTOs = _mapper.Map<List<LeadEngagementTransferDTO>>(leadEngagement);
            return new ApiOkResponse(leadEngagementTransferDTOs);
        }

        public async Task<ApiCommonResponse> GetLeadEngagementByName(string name)
        {
            var leadEngagement = await _leadEngagementRepo.FindLeadEngagementByName(name);
            if (leadEngagement == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var leadEngagementTransferDTOs = _mapper.Map<LeadEngagementTransferDTO>(leadEngagement);
            return new ApiOkResponse(leadEngagementTransferDTOs);
        }

        public async Task<ApiCommonResponse> UpdateLeadEngagement(HttpContext context, long id, LeadEngagementReceivingDTO leadEngagementReceivingDTO)
        {
            var leadEngagementToUpdate = await _leadEngagementRepo.FindLeadEngagementById(id);
            if (leadEngagementToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
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
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
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

        public async Task<ApiCommonResponse> DeleteLeadEngagement(long id)
        {
            var leadEngagementToDelete = await _leadEngagementRepo.FindLeadEngagementById(id);
            if (leadEngagementToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _leadEngagementRepo.DeleteLeadEngagement(leadEngagementToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }
    }
}