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
    public class ComplaintHandlingServiceImpl : IComplaintHandlingService
    {
        private readonly HalobizContext _context;
        private readonly ILogger<ComplaintHandlingServiceImpl> _logger;
        private readonly IMapper _mapper;
        public ComplaintHandlingServiceImpl(HalobizContext context, ILogger<ComplaintHandlingServiceImpl> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ApiResponse> GetComplaintHandlingStats(HttpContext context)
        {
            try
            {
                long userProfileID = context.GetLoggedInUserId();
                List<EscalationMatrixUserProfile> allEscalationMatrixTiedToUser = await _context.EscalationMatrixUserProfiles
                    .Include(x => x.EscalationMatrix)
                    .Where(x => x.UserProfileId == userProfileID && x.EscalationMatrix.IsDeleted == false)
                    .ToListAsync();

                List<Complaint> allCoplaintsAssigned = await _context.Complaints
                    .Where(x => allEscalationMatrixTiedToUser
                                .Select(y => y.EscalationMatrix.ComplaintTypeId)
                                .Contains(x.ComplaintTypeId) && x.IsDeleted == false)
                    .ToListAsync();

                var resultObject = new ComplaintHandlingStatsDTO()
                {
                    TotalComplaintsAssigned = allCoplaintsAssigned.Count(),
                    TotalComplaintsClosed = allCoplaintsAssigned.Where(x => x.IsClosed != null).ToList().Count(),
                    TotalComplaintsBeingHandled = allCoplaintsAssigned.Where(x => (x.IsAssesed != null || x.IsInvestigated != null || x.IsResolved != null) && x.IsClosed == null).ToList().Count(),
                    TotalComplaintsInWorkbench = allCoplaintsAssigned.Where(x => x.PickedById == userProfileID && !x.IsClosed.HasValue).Count()
                };
                return new ApiOkResponse(resultObject);
            }
            catch(Exception error)
            {
                return new ApiResponse(500, error.Message);
            }
        }

        public async Task<ApiResponse> GetComplaintsHandling(HttpContext context)
        {
            try
            {
                long userProfileID = context.GetLoggedInUserId();
                List<EscalationMatrixUserProfile> allEscalationMatrixTiedToUser = await _context.EscalationMatrixUserProfiles
                    .Include(x => x.EscalationMatrix)
                    .Where(x => x.UserProfileId == userProfileID && x.EscalationMatrix.IsDeleted == false)
                    .ToListAsync();

                List<Complaint> allComplaintsAssigned = await _context.Complaints
                    .Include(x => x.ComplaintOrigin)
                    .Include(x => x.ComplaintType)
                    .Include(x => x.ComplaintSource)
                    .Where(x => allEscalationMatrixTiedToUser
                                .Select(y => y.EscalationMatrix.ComplaintTypeId)
                                .Contains(x.ComplaintTypeId) && x.IsDeleted == false)
                    .ToListAsync();
                var complaintTransferDTOs = _mapper.Map<IEnumerable<ComplaintTransferDTO>>(allComplaintsAssigned);

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
                }

                var resultObject = new ComplaintHandlingDTO
                {
                    TotalComplaintsAssigned = complaintTransferDTOs.Count(),
                    TotalComplaintsClosed = complaintTransferDTOs.Where(x => x.IsClosed != null).ToList().Count(),
                    TotalComplaintsBeingHandled = complaintTransferDTOs.Where(x => (x.IsAssesed != null || x.IsInvestigated != null || x.IsResolved != null) && x.IsClosed == null).ToList().Count(),
                    TotalComplaintsInWorkbench = complaintTransferDTOs.Where(x => x.PickedById == userProfileID && !x.IsClosed.HasValue).ToList().Count(),
                    assignedComplaints = complaintTransferDTOs.Where(x => x.PickedById == null).ToList(),
                    workbenchComplaints = complaintTransferDTOs.Where(x => x.PickedById == userProfileID && !x.IsClosed.HasValue).ToList()
                };
                return new ApiOkResponse(resultObject);
            }
            catch(Exception error)
            {
                return new ApiResponse(500, error.Message);
            }
        }

        public async Task<ApiResponse> MoveComplaintToNextStage(HttpContext context, MoveComplaintToNextStageDTO model)
        {
            try
            {
                long userProfileID = context.GetLoggedInUserId();
                Complaint complaint = await _context.Complaints.FirstOrDefaultAsync(x => x.Id == model.complaintID);
                ComplaintStage currentComplaintStage = (ComplaintStage)model.currentStage;

                switch (currentComplaintStage)
                {
                    case ComplaintStage.Registration:
                        complaint.IsAssesed = true;
                        complaint.AssesedLedById = userProfileID;
                        complaint.DateAssesmentsConcluded = DateTime.Now;
                        ComplaintAssesment complaintAssesment = new ComplaintAssesment()
                        {
                            AssesmentDetails = model.details,
                            Findings = model.findings,
                            Complaint = complaint,
                            ComplaintId = complaint.Id,
                            CapturedDateTime = DateTime.Now,
                            Caption = "Complaint Assessment has been done",
                            CapturedById = userProfileID,
                            CommencementDate = complaint.DatePicked.Value,
                            CreatedById = userProfileID,
                            ConclusionDate = DateTime.Now,
                            CreatedAt = DateTime.Now,
                        };
                        await _context.ComplaintAssesments.AddAsync(complaintAssesment);
                        break;
                    case ComplaintStage.Assesment:
                        complaint.IsInvestigated = true;
                        complaint.InvestigationsLedById = userProfileID;
                        complaint.DateInvestigationsCompleted = DateTime.Now;
                        ComplaintInvestigation complaintInvestiagtion = new ComplaintInvestigation()
                        {
                            InvestigationDetails = model.details,
                            Findings = model.findings,
                            Complaint = complaint,
                            ComplaintId = complaint.Id,
                            CapturedDateTime = DateTime.Now,
                            Caption = "Complaint Investigation has been done",
                            CapturedById = userProfileID,
                            CommencementDate = complaint.DateAssesmentsConcluded.Value,
                            CreatedById = userProfileID,
                            ConclusionDate = DateTime.Now,
                            CreatedAt = DateTime.Now,
                        };
                        await _context.ComplaintInvestigations.AddAsync(complaintInvestiagtion);
                        break;
                    case ComplaintStage.Investigation:
                        complaint.IsResolved = true;
                        complaint.ResolvedById = userProfileID;
                        complaint.DateResolved = DateTime.Now;
                        ComplaintResolution complaintResolved = new ComplaintResolution()
                        {
                            ResolutionDetails = model.details,
                            RootCause = model.findings,
                            Complaint = complaint,
                            ComplaintId = complaint.Id,
                            CapturedDateTime = DateTime.Now,
                            Caption = "Complaint Resolution has been done",
                            CapturedById = userProfileID,
                            CreatedById = userProfileID,
                            CreatedAt = DateTime.Now,
                            Learnings = String.IsNullOrEmpty(model.findings) ? "None" : model.findings
                        };
                        await _context.ComplaintResolutions.AddAsync(complaintResolved);
                        break;
                    case ComplaintStage.Resolution:
                        complaint.IsClosed = true;
                        complaint.ClosedById = userProfileID;
                        complaint.DateClosed = DateTime.Now;
                        complaint.IsConfirmedResolved = true;
                        ComplaintResolution complaintClosed = new ComplaintResolution()
                        {
                            ResolutionDetails = model.details,
                            RootCause = model.findings,
                            Complaint = complaint,
                            ComplaintId = complaint.Id,
                            CapturedDateTime = DateTime.Now,
                            Caption = "Complaint Closure has been done",
                            CapturedById = userProfileID,
                            CreatedById = userProfileID,
                            CreatedAt = DateTime.Now,
                            Learnings = String.IsNullOrEmpty(model.findings) ? "None" : model.findings
                        };
                        await _context.ComplaintResolutions.AddAsync(complaintClosed);
                        break;
                    default:
                        return new ApiResponse(500, "Current Stage Passed is invalid");
                }

                if(model.evidences.Length > 0)
                {
                    foreach(var evidence in model.evidences)
                    {
                        Evidence evidenceToAdd = new Evidence()
                        {
                            DateCaptured = DateTime.Now,
                            ComplaintStage = (ComplaintStage)model.currentStage + 1,
                            CreatedAt = DateTime.Now,
                            Caption = "Evidences uplaoded for current complaint stage",
                            Complaint = complaint,
                            ComplaintId = complaint.Id,
                            CreatedById = userProfileID,
                            EvidenceCaptureById = userProfileID,
                            ImageUrl = evidence,
                        };
                        await _context.Evidences.AddAsync(evidenceToAdd);
                    }
                }

                _context.Complaints.Update(complaint);
                await _context.SaveChangesAsync();
                return new ApiOkResponse(true);
            }
            catch(Exception error)
            {
                return new ApiResponse(500, error.Message);
            }
        }

        public async Task<ApiResponse> PickComplaint(HttpContext context, PickComplaintDTO model)
        {
            try
            {
                long userProfileID = context.GetLoggedInUserId();
                Complaint complaint = await _context.Complaints.FirstOrDefaultAsync(x => x.Id == model.complaintId && x.IsDeleted == false);
                if(complaint == null) return new ApiResponse(500, "Error, No Complaint with the passed ID exists");
                if (complaint.IsPicked.HasValue) return new ApiResponse(500, "Error, Complaint is already picked");
                complaint.IsPicked = true;
                complaint.PickedById = userProfileID;
                complaint.DatePicked = DateTime.Now;
                _context.Complaints.Update(complaint);
                await _context.SaveChangesAsync();
                return new ApiOkResponse(true);
            }
            catch(Exception error)
            {
                return new ApiResponse(500, error.Message);
            }
        }
    }
}
