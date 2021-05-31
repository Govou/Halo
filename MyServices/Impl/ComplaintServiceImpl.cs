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
                complaint.DateComplaintReported = complaintReceivingDTO.DateCreated;
                complaint.DateCreated = DateTime.Now;
                complaint.DateRegistered = DateTime.Now;

                complaint.IsRegistered = true;

                var savedcomplaint = await _complaintRepo.SaveComplaint(complaint);
                if (savedcomplaint == null)
                {
                    return new ApiResponse(500);
                }

                var complaintType = await _context.ComplaintTypes.FindAsync(savedcomplaint.ComplaintTypeId);
                var complaintOrigin = await _context.ComplaintOrigins.FindAsync(savedcomplaint.ComplaintOriginId);

                savedcomplaint.TrackingId = $"#COMPL{savedcomplaint.Id}-{complaintType.Code}-{complaintOrigin.Code}";
                _context.Complaints.Update(savedcomplaint);
                await _context.SaveChangesAsync();

                if (complaintReceivingDTO.Evidences.Count > 0)
                {
                    var evidences = _mapper.Map<IEnumerable<Evidence>>(complaintReceivingDTO.Evidences);

                    foreach (var evidence in evidences)
                    {
                        evidence.ComplaintId = savedcomplaint.Id;
                        evidence.CreatedById = loggedInUserId;
                        evidence.EvidenceCaptureById = loggedInUserId;
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
                .Where(x => x.RegisteredById == loggedInUserId && x.IsDeleted == false)
                .ToListAsync();

            if(userComplaints.Count > 0)
            {
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

                var registeredPercentage = (Convert.ToDecimal(registeredOnlyComplaints.Count) / Convert.ToDecimal(userComplaints.Count)) * 100m;
                var assesedPercentage = (Convert.ToDecimal(assesedOnlyComplaints.Count) / Convert.ToDecimal(userComplaints.Count)) * 100m;
                var investigatedPercentage = (Convert.ToDecimal(investigatedOnlyComplaints.Count) / Convert.ToDecimal(userComplaints.Count)) * 100m;
                var resolvedPercentage = (Convert.ToDecimal(resolvedOnlyComplaints.Count) / Convert.ToDecimal(userComplaints.Count)) * 100m;
                var closedPercentage = (Convert.ToDecimal(closedOnlyComplaints.Count) / Convert.ToDecimal(userComplaints.Count)) * 100m;

                var response = new
                {
                    ResgisteredPercentage = Math.Round(registeredPercentage, 2),
                    AssesedPercentage = Math.Round(assesedPercentage, 2),
                    InvestigatedPercentage = Math.Round(investigatedPercentage, 2),
                    ResolvedPercentage = Math.Round(resolvedPercentage, 2),
                    ClosedPercentage = Math.Round(closedPercentage, 2),
                };

                return new ApiOkResponse(response);
            }
            else
            {
                var response = new
                {
                    ResgisteredPercentage = 0,
                    AssesedPercentage = 0,
                    InvestigatedPercentage = 0,
                    ResolvedPercentage = 0,
                    ClosedPercentage = 0,
                };

                return new ApiOkResponse(response);
            }
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
            var complaintTransferDTOs = _mapper.Map<IEnumerable<ComplaintTransferDTO>>(complaints);

            foreach (var complaint in complaintTransferDTOs)
            {
                switch (complaint.ComplaintOrigin.Caption.ToLower())
                {
                    case "supplier":
                        complaint.Complainant = await _context.Suppliers.FindAsync(complaint.ComplainantId);
                        break;
                    case "staff":
                        complaint.Complainant = await _context.UserProfiles.FindAsync(complaint.ComplainantId);
                        break;
                    case "client":
                        complaint.Complainant = await _context.CustomerDivisions.FindAsync(complaint.ComplainantId);
                        break;
                }

                complaint.EvidenceUrls = await _context.Evidences.Where(x => x.ComplaintId == complaint.Id).Select(x => x.ImageUrl).ToListAsync();
            }

            return new ApiOkResponse(complaintTransferDTOs);
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
