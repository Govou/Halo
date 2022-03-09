using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HalobizMigrations.Models.OnlinePortal;
using Microsoft.AspNetCore.Http;
using OnlinePortalBackend.DTOs.ApiDTOs;
using OnlinePortalBackend.DTOs.ReceivingDTOs;
using OnlinePortalBackend.DTOs.TransferDTOs;
using OnlinePortalBackend.Helpers;
using OnlinePortalBackend.Repository;

namespace OnlinePortalBackend.MyServices.Impl
{
    public class PortalComplaintServiceImpl : IPortalComplaintService
    {
        private readonly IPortalComplaintRepository _portalComplaintRepo;
        private readonly IMapper _mapper;
        private readonly IModificationHistoryRepository _historyRepo ;
        public PortalComplaintServiceImpl(IPortalComplaintRepository portalComplaintRepo, 
            IMapper mapper, 
            IModificationHistoryRepository historyRepo)
        {
            _mapper = mapper;
            _portalComplaintRepo = portalComplaintRepo;
            _historyRepo = historyRepo;
        }

        public async Task<ApiResponse> AddPortalComplaint(HttpContext context, PortalComplaintReceivingDTO portalComplaintReceivingDTO)
        {
            var portalComplaint = _mapper.Map<PortalComplaint>(portalComplaintReceivingDTO);
            portalComplaint.CreatedById = context.GetLoggedInUserId();
            var savedPortalComplaint = await _portalComplaintRepo.SavePortalComplaint(portalComplaint);
            if(savedPortalComplaint == null)
            {
                return new ApiResponse(500);
            }
            var portalComplaintTransferDto = _mapper.Map<PortalComplaintTransferDTO>(portalComplaint);
            return new ApiOkResponse(portalComplaintTransferDto);
        }

        public async Task<ApiResponse> FindPortalComplaintById(long id)
        {
            var portalComplaint = await _portalComplaintRepo.FindPortalComplaintById(id);
            if(portalComplaint == null)
            {
                return new ApiResponse(404);
            }
            var portalComplaintTransferDto = _mapper.Map<PortalComplaintTransferDTO>(portalComplaint);
            return new ApiOkResponse(portalComplaintTransferDto);
        }        

        public async Task<ApiResponse> FindAllPortalComplaints()
        {
            var portalComplaints = await _portalComplaintRepo.FindAllPortalComplaints();
            if(portalComplaints == null )
            {
                return new ApiResponse(404);
            }
            var portalComplaintsTransferDto = _mapper.Map<IEnumerable<PortalComplaintTransferDTO>>(portalComplaints);
            return new ApiOkResponse(portalComplaintsTransferDto);
        }

        public async Task<ApiResponse> UpdatePortalComplaint(HttpContext context, long portalComplaintId, PortalComplaintReceivingDTO portalComplaintReceivingDTO)
        {
            var portalComplaintToUpdate = await _portalComplaintRepo.FindPortalComplaintById(portalComplaintId);
            if(portalComplaintToUpdate == null)
            {
                return new ApiResponse(404);
            }
            var summary = $"Initial details before change, \n {portalComplaintToUpdate.ToString()} \n" ;
            portalComplaintToUpdate.AdminComments = portalComplaintReceivingDTO.AdminComments;
            portalComplaintToUpdate.ComplaintDescription = portalComplaintReceivingDTO.ComplaintDescription;
            portalComplaintToUpdate.CustomerDivisionId = portalComplaintReceivingDTO.CustomerDivisionId;
            portalComplaintToUpdate.DateResolved = portalComplaintReceivingDTO.DateResolved;
            portalComplaintToUpdate.IsResolved = portalComplaintReceivingDTO.IsResolved;
            portalComplaintToUpdate.ProspectId = portalComplaintReceivingDTO.ProspectId;
            portalComplaintToUpdate.ResolutionComments = portalComplaintReceivingDTO.ResolutionComments;
            portalComplaintToUpdate.ServiceId = portalComplaintReceivingDTO.ServiceId;

            summary += $"Details after change, \n {portalComplaintToUpdate} \n";

            var updatedPortalComplaint = await _portalComplaintRepo.UpdatePortalComplaint(portalComplaintToUpdate);

            if(updatedPortalComplaint == null)
            {
                return new ApiResponse(500);
            }      

            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "PortalComplaint",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedPortalComplaint.Id
            };

            await _historyRepo.SaveHistory(history);

            var portalComplaintTransferDto = _mapper.Map<PortalComplaintTransferDTO>(updatedPortalComplaint);
            return new ApiOkResponse(portalComplaintTransferDto);
        }

        public async Task<ApiResponse> DeletePortalComplaint(long portalComplaintId)
        {
            var portalComplaintToDelete = await _portalComplaintRepo.FindPortalComplaintById(portalComplaintId);
            if(portalComplaintToDelete == null)
            {
                return new ApiResponse(404);
            }

            if(!await _portalComplaintRepo.RemovePortalComplaint(portalComplaintToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }
    }
}