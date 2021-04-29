using AutoMapper;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Helpers;
using HalobizMigrations.Models;
using HaloBiz.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices.Impl
{
    public class ComplaintServiceImpl : IComplaintService
    {
        private readonly ILogger<ComplaintServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IComplaintRepository _complaintRepo;
        private readonly IMapper _mapper;

        public ComplaintServiceImpl(IModificationHistoryRepository historyRepo, IComplaintRepository ComplaintRepo, ILogger<ComplaintServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._complaintRepo = ComplaintRepo;
            this._logger = logger;
        }

        public async Task<ApiResponse> AddComplaint(HttpContext context, ComplaintReceivingDTO complaintReceivingDTO)
        {

            var complaint = _mapper.Map<Complaint>(complaintReceivingDTO);
            complaint.CreatedById = context.GetLoggedInUserId();
            var savedcomplaint = await _complaintRepo.SaveComplaint(complaint);
            if (savedcomplaint == null)
            {
                return new ApiResponse(500);
            }
            var complaintTransferDTO = _mapper.Map<ComplaintTransferDTO>(complaint);
            return new ApiOkResponse(complaintTransferDTO);
        }

        public async Task<ApiResponse> DeleteComplaint(long id)
        {
            var complaintToDelete = await _complaintRepo.FindComplaintById(id);
            if (complaintToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _complaintRepo.DeleteComplaint(complaintToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> GetAllComplaint()
        {
            var complaints = await _complaintRepo.FindAllComplaints();
            if (complaints == null)
            {
                return new ApiResponse(404);
            }
            var complaintTransferDTO = _mapper.Map<IEnumerable<ComplaintTransferDTO>>(complaints);
            return new ApiOkResponse(complaintTransferDTO);
        }

        public async Task<ApiResponse> GetComplaintById(long id)
        {
            var complaint = await _complaintRepo.FindComplaintById(id);
            if (complaint == null)
            {
                return new ApiResponse(404);
            }
            var complaintTransferDTOs = _mapper.Map<ComplaintTransferDTO>(complaint);
            return new ApiOkResponse(complaintTransferDTOs);
        }

        public async Task<ApiResponse> GetComplaintByName(string name)
        {
            var complaint = await _complaintRepo.FindComplaintByName(name);
            if (complaint == null)
            {
                return new ApiResponse(404);
            }
            var complaintTransferDTOs = _mapper.Map<ComplaintTransferDTO>(complaint);
            return new ApiOkResponse(complaintTransferDTOs);
        }

        public async Task<ApiResponse> UpdateComplaint(HttpContext context, long id, ComplaintReceivingDTO complaintReceivingDTO)
        {
            var complaintToUpdate = await _complaintRepo.FindComplaintById(id);
            if (complaintToUpdate == null)
            {
                return new ApiResponse(404);
            }

            var summary = $"Initial details before change, \n {complaintToUpdate.ToString()} \n";

            complaintToUpdate.ComplainantId = complaintReceivingDTO.ComplainantId;
            complaintToUpdate.ComplaintDescription = complaintReceivingDTO.ComplaintDescription;
            complaintToUpdate.ComplaintOriginId = complaintReceivingDTO.ComplaintOriginId;
            complaintToUpdate.ComplaintSourceId = complaintReceivingDTO.ComplaintSourceId;
            complaintToUpdate.ComplaintTypeId = complaintReceivingDTO.ComplaintTypeId;
            var updatedcomplaint = await _complaintRepo.UpdateComplaint(complaintToUpdate);

            summary += $"Details after change, \n {updatedcomplaint.ToString()} \n";

            if (updatedcomplaint == null)
            {
                return new ApiResponse(500);
            }
            ModificationHistory history = new ModificationHistory()
            {
                ModelChanged = "complaint",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedcomplaint.Id
            };
            await _historyRepo.SaveHistory(history);

            var complaintTransferDTOs = _mapper.Map<ComplaintTransferDTO>(updatedcomplaint);
            return new ApiOkResponse(complaintTransferDTOs);
        }
    }
}
