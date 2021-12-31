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
    public class LeadDivisionServiceImpl : ILeadDivisionService
    {
        private readonly ILogger<LeadDivisionServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly ILeadDivisionRepository _leadDivisionRepo;
        private readonly IMapper _mapper;

        public LeadDivisionServiceImpl(IModificationHistoryRepository historyRepo, ILeadDivisionRepository leadDivisionRepo, ILogger<LeadDivisionServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._leadDivisionRepo = leadDivisionRepo;
            this._logger = logger;
        }

        public async Task<ApiResponse> AddLeadDivision(HttpContext context, LeadDivisionReceivingDTO leadDivisionReceivingDTO)
        {
            var leadDivision = _mapper.Map<LeadDivision>(leadDivisionReceivingDTO);
            leadDivision.CreatedById = context.GetLoggedInUserId();
            var savedLeadDivision = await _leadDivisionRepo.SaveLeadDivision(leadDivision);
            if (savedLeadDivision == null)
            {
                return new ApiResponse(500);
            }
            var leadDivisionTransferDTO = _mapper.Map<LeadDivisionTransferDTO>(savedLeadDivision);
            return new ApiOkResponse(leadDivisionTransferDTO);
        }

        public async Task<ApiResponse> GetAllLeadDivision()
        {
            var leadDivisions = await _leadDivisionRepo.FindAllLeadDivision();
            if (leadDivisions == null)
            {
                return new ApiResponse(404);
            }
            var leadDivisionTransferDTO = _mapper.Map<IEnumerable<LeadDivisionTransferDTO>>(leadDivisions);
            return new ApiOkResponse(leadDivisionTransferDTO);
        }

        public async Task<ApiResponse> GetLeadDivisionById(long id)
        {
            var leadDivision = await _leadDivisionRepo.FindLeadDivisionById(id);
            if (leadDivision == null)
            {
                return new ApiResponse(404);
            }
            var leadDivisionTransferDTOs = _mapper.Map<LeadDivisionTransferDTO>(leadDivision);
            return new ApiOkResponse(leadDivisionTransferDTOs);
        }

        public async Task<ApiResponse> GetLeadDivisionByName(string name)
        {
            var leadDivision = await _leadDivisionRepo.FindLeadDivisionByName(name);
            if (leadDivision == null)
            {
                return new ApiResponse(404);
            }
            var leadDivisionTransferDTOs = _mapper.Map<LeadDivisionTransferDTO>(leadDivision);
            return new ApiOkResponse(leadDivisionTransferDTOs);
        }

        public async Task<ApiResponse> GetLeadDivisionByRCNumber(string rcNumber)
        {
            var leadDivision = await _leadDivisionRepo.FindLeadDivisionByRCNumber(rcNumber);
            if (leadDivision == null)
            {
                return new ApiResponse(404);
            }
            var leadDivisionTransferDTOs = _mapper.Map<LeadDivisionTransferDTO>(leadDivision);
            return new ApiOkResponse(leadDivisionTransferDTOs);
        }

        public async Task<ApiResponse> UpdateLeadDivision(HttpContext context, long id, LeadDivisionReceivingDTO leadDivisionReceivingDTO)
        {
            var leadDivisionToUpdate = await _leadDivisionRepo.FindLeadDivisionById(id);
            if (leadDivisionToUpdate == null)
            {
                return new ApiResponse(404);
            }
            
            var summary = $"Initial details before change, \n {leadDivisionToUpdate.ToString()} \n" ;

            leadDivisionToUpdate.LeadOriginId = leadDivisionReceivingDTO.LeadOriginId;
            leadDivisionToUpdate.Industry = leadDivisionReceivingDTO.Industry;
            leadDivisionToUpdate.Rcnumber = leadDivisionReceivingDTO.RCNumber;
            leadDivisionToUpdate.DivisionName = leadDivisionReceivingDTO.DivisionName;
            leadDivisionToUpdate.PhoneNumber = leadDivisionReceivingDTO.PhoneNumber;
            leadDivisionToUpdate.Email = leadDivisionReceivingDTO.Email;
            leadDivisionToUpdate.LogoUrl = leadDivisionReceivingDTO.LogoUrl;
            leadDivisionToUpdate.PrimaryContactId = (Int64) leadDivisionReceivingDTO.PrimaryContactId;
            leadDivisionToUpdate.SecondaryContactId = leadDivisionReceivingDTO.SecondaryContactId;
            leadDivisionToUpdate.BranchId = (Int64) leadDivisionReceivingDTO.BranchId;
            leadDivisionToUpdate.OfficeId = (Int64) leadDivisionReceivingDTO.OfficeId;
            leadDivisionToUpdate.LeadId = leadDivisionReceivingDTO.LeadId;
            leadDivisionToUpdate.StateId = (Int64)leadDivisionReceivingDTO.StateId;
            leadDivisionToUpdate.Lgaid = (Int64)leadDivisionReceivingDTO.LGAId;
            leadDivisionToUpdate.Street = leadDivisionReceivingDTO.Street;
            leadDivisionToUpdate.Address = leadDivisionReceivingDTO.Address;
            leadDivisionToUpdate.LeadTypeId = leadDivisionReceivingDTO.LeadTypeId;

            var updatedLeadDivision = await _leadDivisionRepo.UpdateLeadDivision(leadDivisionToUpdate);

            summary += $"Details after change, \n {updatedLeadDivision.ToString()} \n";

            if (updatedLeadDivision == null)
            {
                return new ApiResponse(500);
            }
            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "LeadDivision",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedLeadDivision.Id
            };

            await _historyRepo.SaveHistory(history);

            var leadDivisionTransferDTOs = _mapper.Map<LeadDivisionTransferDTO>(updatedLeadDivision);
            return new ApiOkResponse(leadDivisionTransferDTOs);

        }

        public async Task<ApiResponse> DeleteLeadDivision(long id)
        {
            var leadDivisionToDelete = await _leadDivisionRepo.FindLeadDivisionById(id);
            if (leadDivisionToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _leadDivisionRepo.DeleteLeadDivision(leadDivisionToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }
    }
}