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

        public async Task<ApiResponse> GetUserEscalationLevelDetails(HttpContext context)
        {
            try
            {
                long userProfileID = context.GetLoggedInUserId();
                if (userProfileID <= 0) return new ApiResponse(500, "Unable to retrieve user's details");
                ProfileEscalationLevel profileEscalationLevel = await _context.ProfileEscalationLevels.FirstOrDefaultAsync(x => x.UserProfileId == userProfileID);
                if (profileEscalationLevel == null) return new ApiResponse(500, "User has no profile level escalation configured for user profile");
                List<EscalationMatrix> escalationMatrices = await _context.EscalationMatrices
                    .Include(x => x.ComplaintAttendants)
                    .Where(x => x.ComplaintAttendants
                          .Any(y => y.EscalationLevelId == profileEscalationLevel.EscalationLevelId))
                    .ToListAsync();


                return new ApiOkResponse(true);
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
                            Learnings = "None"
                        };
                        await _context.ComplaintResolutions.AddAsync(complaintResolved);
                        break;
                    case ComplaintStage.Resolution:
                        complaint.IsClosed = true;
                        complaint.ClosedById = userProfileID;
                        complaint.DateClosed = DateTime.Now;
                        //complaint.IsConfirmedResolved = true;     ~Will be updated either by the user clicking on confirmation link or by the cron job.
                        ComplaintResolution complaintClosed = await _context.ComplaintResolutions.FirstOrDefaultAsync(x => x.ComplaintId == complaint.Id);
                        complaintClosed.Learnings = model.findings;
                        _context.ComplaintResolutions.Update(complaintClosed);
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

        public async Task<ApiResponse> TrackComplaint(ComplaintTrackingRecievingDTO model)
        {
            try
            {
                Complaint complaint = await _context.Complaints
                    .Include(x => x.ComplaintOrigin)
                    .Include(x => x.ComplaintType)
                    .Include(x => x.ComplaintSource)
                    .Include(x => x.PickedBy)
                    .FirstOrDefaultAsync(x => x.TrackingId == model.TrackingNo);

                if(complaint == null) return new ApiResponse(500, "No Complaint with the passed tracking number exists.");

                var complaintTransferDTOs = _mapper.Map<ComplaintTransferDTO>(complaint);
                switch (complaintTransferDTOs.ComplaintOrigin.Caption.ToLower())
                {
                    case "supplier":
                        complaintTransferDTOs.Complainant = await _context.Suppliers.FindAsync(complaint.ComplainantId);
                        break;
                    case "staff":
                        complaintTransferDTOs.Complainant = await _context.UserProfiles.FindAsync(complaint.ComplainantId);
                        break;
                    case "client":
                        complaintTransferDTOs.Complainant = await _context.CustomerDivisions.FindAsync(complaint.ComplainantId);
                        break;
                }
                ComplaintAssesment complaintAssesment = await _context.ComplaintAssesments.FirstOrDefaultAsync(x => x.ComplaintId == complaint.Id);
                ComplaintInvestigation complaintInvestigation = await _context.ComplaintInvestigations.FirstOrDefaultAsync(x => x.ComplaintId == complaint.Id);
                ComplaintResolution complaintResolution = await _context.ComplaintResolutions.FirstOrDefaultAsync(x => x.ComplaintId == complaint.Id);
                List<string> registrationEvidences = await _context.Evidences.Where(x => x.ComplaintId == complaint.Id && x.ComplaintStage == ComplaintStage.Registration).Select(x => x.ImageUrl).ToListAsync();
                List<string> assessmentEvidences = await _context.Evidences.Where(x => x.ComplaintId == complaint.Id && x.ComplaintStage == ComplaintStage.Assesment).Select(x => x.ImageUrl).ToListAsync();
                List<string> investiagtionEvidences = await _context.Evidences.Where(x => x.ComplaintId == complaint.Id && x.ComplaintStage == ComplaintStage.Investigation).Select(x => x.ImageUrl).ToListAsync();
                List<string> resolutionEvidences = await _context.Evidences.Where(x => x.ComplaintId == complaint.Id && x.ComplaintStage == ComplaintStage.Resolution).Select(x => x.ImageUrl).ToListAsync();
                var totalHandlerCases = await _context.Complaints.Where(x => x.PickedById == complaint.PickedById && x.IsDeleted == false).ToListAsync();
                EscalationMatrix escalationMatrix = await _context.EscalationMatrices.FirstOrDefaultAsync(x => x.ComplaintTypeId == complaint.ComplaintTypeId && x.IsDeleted == false);
                decimal totalCasesResolvedPercentage = (Convert.ToDecimal(totalHandlerCases.Where(x => x.IsResolved == true).Count()) / Convert.ToDecimal(totalHandlerCases.Count())) * 100m;
                decimal totalCasesUnresolvedPercentage = (Convert.ToDecimal(totalHandlerCases.Where(x => x.IsResolved == null).Count()) / Convert.ToDecimal(totalHandlerCases.Count())) * 100m;

                var resultObject = new ComplaintTrackingTransferDTO()
                {
                    Complaint = complaintTransferDTOs,
                    Assessment = _mapper.Map<ComplaintAssessmentTransferDTO>(complaintAssesment),
                    Investigation = _mapper.Map<ComplaintInvestigationTransferDTO>(complaintInvestigation),
                    Resolution = _mapper.Map<ComplaintResolutionTransferDTO>(complaintResolution),
                    RegistrationEvidenceUrls = registrationEvidences,
                    AssessmentEvidenceUrls = assessmentEvidences,
                    InvestigationEvidenceUrls = investiagtionEvidences,
                    ResolutionEvidenceUrls = resolutionEvidences,
                    UserProfileImageUrl = complaint.PickedBy == null ? String.Empty : complaint.PickedBy.ImageUrl,
                    TotalHandlerCases = complaint.PickedById == null ? 0 : totalHandlerCases.Count(),
                    TotalHandlerCasesResolved = complaint.PickedById == null ? 0 : Math.Round(totalCasesResolvedPercentage, 2),
                    TotalHanlderCasesUnresolved = complaint.PickedById == null ? 0 : Math.Round(totalCasesUnresolvedPercentage, 2),
                    EstimatedDateResolved = escalationMatrix == null ? complaint.DateRegistered : complaint.DateRegistered.AddHours(escalationMatrix.Level1MaxResolutionTimeInHrs),
                    HandlerName = complaint.PickedBy == null ? String.Empty : complaint.PickedBy.LastName + " " + complaint.PickedBy.FirstName,
                };
                return new ApiOkResponse(resultObject);
            }
            catch(Exception error)
            {
                return new ApiResponse(500, error.Message);
            }
        }
    }
}
