using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Halobiz.Common.DTOs.ApiDTOs;
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

        public async Task<ApiCommonResponse> AddLeadOrigin(HttpContext context, LeadOriginReceivingDTO leadOriginReceivingDTO)
        {
            var leadOrigin = _mapper.Map<LeadOrigin>(leadOriginReceivingDTO);
            leadOrigin.CreatedById = context.GetLoggedInUserId();
            var savedLeadOrigin = await _leadOriginRepo.SaveLeadOrigin(leadOrigin);
            if (savedLeadOrigin == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var leadOriginTransferDTO = _mapper.Map<LeadOriginTransferDTO>(savedLeadOrigin);
            return CommonResponse.Send(ResponseCodes.SUCCESS,leadOriginTransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllLeadOrigin()
        {
            var leadOrigins = await _leadOriginRepo.FindAllLeadOrigin();
            if (leadOrigins == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var leadOriginTransferDTO = _mapper.Map<IEnumerable<LeadOriginTransferDTO>>(leadOrigins);
            return CommonResponse.Send(ResponseCodes.SUCCESS,leadOriginTransferDTO);
        }

        public async Task<ApiCommonResponse> GetLeadOriginById(long id)
        {
            var leadOrigin = await _leadOriginRepo.FindLeadOriginById(id);
            if (leadOrigin == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var leadOriginTransferDTO = _mapper.Map<LeadOriginTransferDTO>(leadOrigin);
            return CommonResponse.Send(ResponseCodes.SUCCESS,leadOriginTransferDTO);
        }

        public async Task<ApiCommonResponse> GetLeadOriginByName(string name)
        {
            var leadOrigin = await _leadOriginRepo.FindLeadOriginByName(name);
            if (leadOrigin == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var leadOriginTransferDTOs = _mapper.Map<LeadOriginTransferDTO>(leadOrigin);
            return CommonResponse.Send(ResponseCodes.SUCCESS,leadOriginTransferDTOs);
        }

        public async Task<ApiCommonResponse> UpdateLeadOrigin(HttpContext context, long id, LeadOriginReceivingDTO LeadOriginReceivingDTO)
        {
            var leadOriginToUpdate = await _leadOriginRepo.FindLeadOriginById(id);
            if (leadOriginToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            
            var summary = $"Initial details before change, \n {leadOriginToUpdate.ToString()} \n" ;

            leadOriginToUpdate.Caption = LeadOriginReceivingDTO.Caption;
            leadOriginToUpdate.Description = LeadOriginReceivingDTO.Description;
            leadOriginToUpdate.LeadTypeId = LeadOriginReceivingDTO.LeadTypeId;
            var updatedLeadOrigin = await _leadOriginRepo.UpdateLeadOrigin(leadOriginToUpdate);

            summary += $"Details after change, \n {updatedLeadOrigin.ToString()} \n";

            if (updatedLeadOrigin == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "LeadOrigin",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedLeadOrigin.Id
            };

            await _historyRepo.SaveHistory(history);

            var leadOriginTransferDTOs = _mapper.Map<LeadOriginTransferDTO>(updatedLeadOrigin);
            return CommonResponse.Send(ResponseCodes.SUCCESS,leadOriginTransferDTOs);

        }

        public async Task<ApiCommonResponse> DeleteLeadOrigin(long id)
        {
            var leadOriginToDelete = await _leadOriginRepo.FindLeadOriginById(id);
            if (leadOriginToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _leadOriginRepo.DeleteLeadOrigin(leadOriginToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }
    }
}