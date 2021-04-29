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
using HalobizMigrations.Models.Complaints;
using HalobizMigrations.Data;
using Microsoft.EntityFrameworkCore;

namespace HaloBiz.MyServices.Impl
{
    public class ComplaintServiceImpl : IComplaintService
    {
        private readonly ILogger<ComplaintServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IComplaintRepository _complaintRepo;
        private readonly IEvidenceRepository _evidenceRepository;
        private readonly HalobizContext _context;
        private readonly IMapper _mapper;

        public ComplaintServiceImpl(IModificationHistoryRepository historyRepo,
            IComplaintRepository complaintRepo,
            IEvidenceRepository evidenceRepo,
            HalobizContext context,
            ILogger<ComplaintServiceImpl> logger, 
            IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._complaintRepo = complaintRepo;
            _evidenceRepository = evidenceRepo;
            _context = context;
            this._logger = logger;
        }

        public async Task<ApiResponse> AddComplaint(HttpContext context, ComplaintReceivingDTO complaintReceivingDTO)
        {         
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var loggedInUserId = context.GetLoggedInUserId();

                var complaint = _mapper.Map<Complaint>(complaintReceivingDTO);

                complaint.CreatedById = loggedInUserId;
                complaint.RegisteredById = loggedInUserId;

                complaint.DateRegistered = DateTime.Now;

                complaint.IsRegistered = true;

                var savedcomplaint = await _complaintRepo.SaveComplaint(complaint);
                if (savedcomplaint == null)
                {
                    return new ApiResponse(500);
                }

                var complaintType = await _context.ComplaintTypes.FindAsync(savedcomplaint.ComplaintTypeId);
                var complaintOrigin = await _context.ComplaintTypes.FindAsync(savedcomplaint.ComplaintOriginId);

                savedcomplaint.TrackingId = $"#COMPL{savedcomplaint.Id}-{complaintType.Code}-{complaintOrigin.Code}";
                _context.Complaints.Update(savedcomplaint);
                await _context.SaveChangesAsync();

                if (complaintReceivingDTO.Evidences.Count > 0)
                {
                    var evidences = _mapper.Map<IEnumerable<Evidence>>(complaintReceivingDTO.Evidences);

                    foreach (var evidence in evidences)
                    {
                        evidence.ComplaintId = savedcomplaint.Id;
                    }

                    await _context.Evidences.AddRangeAsync(evidences);
                    await _context.SaveChangesAsync();
                }             

                await transaction.CommitAsync();

                var complaintTransferDTO = _mapper.Map<ComplaintTransferDTO>(complaint);
                return new ApiOkResponse(complaintTransferDTO);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex.Message);
                return new ApiResponse(500, ex.Message);
            }           
        }

        public async Task<ApiResponse> GetComplaintsStats(HttpContext context)
        {
            var loggedInUserId = context.GetLoggedInUserId();

            var userComplaints = await _context.Complaints
                .Where(x => x.RegisteredById == loggedInUserId)
                .ToListAsync();

            var registeredOnlyComplaints = userComplaints
                .Where(x => x.IsRegistered != null && x.IsResolved == null && x.IsInvestigated == null 
                        && x.IsAssesed == null && x.IsClosed == null).ToList();
            
            var assesedOnlyComplaints = userComplaints
                .Where(x => x.IsRegistered != null && x.IsResolved == null && x.IsInvestigated == null
                        && x.IsAssesed != null && x.IsClosed == null).ToList();

            var investigatedOnlyComplaints = userComplaints
                .Where(x => x.IsRegistered != null && x.IsResolved == null && x.IsInvestigated != null
                        && x.IsAssesed != null && x.IsClosed == null).ToList();

            var resolvedOnlyComplaints = userComplaints
                .Where(x => x.IsRegistered != null && x.IsResolved != null && x.IsInvestigated != null
                        && x.IsAssesed != null && x.IsClosed == null).ToList();

            var closedOnlyComplaints = userComplaints
                .Where(x => x.IsRegistered != null && x.IsResolved != null && x.IsInvestigated != null
                        && x.IsAssesed != null && x.IsClosed != null).ToList();

            var registeredPercentage = registeredOnlyComplaints.Count / userComplaints.Count * 100;
            var assesedPercentage = assesedOnlyComplaints.Count / userComplaints.Count * 100;
            var investigatedPercentage = investigatedOnlyComplaints.Count / userComplaints.Count * 100;
            var resolvedPercentage = resolvedOnlyComplaints.Count / userComplaints.Count * 100;
            var closedPercentage = closedOnlyComplaints.Count / userComplaints.Count * 100;

            var response = new 
            {
                ResgisteredPercentage = registeredPercentage,
                AssesedPercentage = assesedPercentage,
                InvestigatedPercentage = investigatedPercentage,
                ResolvedPercentage = resolvedPercentage,
                ClosedPercentage = closedPercentage,
            };

            return new ApiOkResponse(response);
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

        /*public async Task<ApiResponse> GetComplaintByName(string name)
        {
            var complaint = await _complaintRepo.FindComplaintByName(name);
            if (complaint == null)
            {
                return new ApiResponse(404);
            }
            var complaintTransferDTOs = _mapper.Map<ComplaintTransferDTO>(complaint);
            return new ApiOkResponse(complaintTransferDTOs);
        }*/

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
            complaintToUpdate.DateCreated = complaintReceivingDTO.DateCreated;
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
