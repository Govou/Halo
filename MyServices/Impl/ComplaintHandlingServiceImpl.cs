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
using HaloBiz.DTOs.MailDTOs;

namespace HaloBiz.MyServices.Impl
{
    public class ComplaintHandlingServiceImpl : IComplaintHandlingService
    {
        private readonly HalobizContext _context;
        private readonly ILogger<ComplaintHandlingServiceImpl> _logger;
        private readonly IMapper _mapper;
        private readonly Adapters.IMailAdapter _mailAdapter;
        public ComplaintHandlingServiceImpl(HalobizContext context, ILogger<ComplaintHandlingServiceImpl> logger, IMapper mapper, Adapters.IMailAdapter mailAdapter)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _mailAdapter = mailAdapter;
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
                _logger.LogError("Exception occurred in  GetComplaintHandlingStats " + error);
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
                _logger.LogError("Exception occurred in  GetComplaintsHandling " + error);
                return new ApiResponse(500, error.Message);
            }
        }

        public async Task<ApiResponse> GetUserEscalationLevelDetails(HttpContext context)
        {
            try
            {
                long userProfileID = context.GetLoggedInUserId();
                if (userProfileID <= 0) return new ApiResponse(500, "Unable to retrieve user's details");
                ProfileEscalationLevel profileEscalationLevel = await _context.ProfileEscalationLevels.Include(x => x.EscalationLevel).FirstOrDefaultAsync(x => x.UserProfileId == userProfileID && x.IsDeleted == false);
                if (profileEscalationLevel == null) return new ApiResponse(500, "User has no profile level escalation configured for user profile");
                if (profileEscalationLevel.EscalationLevel.Caption.ToLower().Contains("handler")) return new ApiResponse(500, "Currently logged in user is an hanler. You need to either be a supervisor or manager.");
                List<EscalationMatrix> escalationMatrices = await _context.EscalationMatrices
                    .Include(x => x.ComplaintAttendants)
                    .Include(x => x.ComplaintType)
                    .Where(x => x.ComplaintAttendants.Any(y => y.EscalationLevelId == profileEscalationLevel.EscalationLevelId && y.UserProfileId == userProfileID) == true && x.IsDeleted == false)
                    .ToListAsync();

                long handlerEscalationLevelID = await GetHandlerEscalationLevelID();
                List<Complaint> assignedComplaints = new();
                List<Complaint> unassignedComplaints = new();
                List<Complaint> allComplaints = await _context.Complaints
                    .Include(x => x.PickedBy)
                    .Include(x => x.ComplaintOrigin)
                    .Include(x => x.ComplaintSource)
                    .Include(x => x.ComplaintType)
                    .Where(x => x.IsDeleted == false).ToListAsync();

                //Calculate Complaint Distributions
                List<ComplaintType> allComplaintTypes = await _context.ComplaintTypes.Where(x => x.IsDeleted == false).ToListAsync();
                List<decimal> complaintDistributionPercentages = new();
                List<decimal> complaintDistributionValues = new();
                double totalComplaints = 0;
                foreach(var complaintType in allComplaintTypes)
                {
                    int complaintCount = allComplaints.Where(x => x.ComplaintTypeId == complaintType.Id).Count();
                    complaintDistributionValues.Add(complaintCount);
                    totalComplaints += complaintCount;
                }
                foreach(var complaintDistribution in complaintDistributionValues)
                {
                    var resultValue = (Convert.ToDecimal(complaintDistribution) / Convert.ToDecimal(totalComplaints)) * 100m;
                    complaintDistributionPercentages.Add(Math.Round(resultValue, 2));
                }

                List<Complaint> complaints = allComplaints.Where(y => escalationMatrices.Select(x => x.ComplaintTypeId).ToList().Contains(y.ComplaintTypeId) && y.IsResolved == null).ToList();
                if(!profileEscalationLevel.EscalationLevel.Caption.ToLower().Contains("supervisor") && !profileEscalationLevel.EscalationLevel.Caption.ToLower().Contains("supervisor")) return new ApiResponse(500, "No configuration created for users escalation level " + profileEscalationLevel.EscalationLevel.Caption);
                foreach (var escalation in escalationMatrices)
                {
                    long compareTime = profileEscalationLevel.EscalationLevel.Caption.ToLower().Contains("supervisor") ? escalation.Level2MaxResolutionTimeInHrs : escalation.Level1MaxResolutionTimeInHrs;
                    foreach (var complaint in complaints.Where(x => x.ComplaintTypeId == escalation.ComplaintTypeId).ToList())
                    {
                        TimeSpan duration = DateTime.Now - complaint.DateRegistered;
                        if (duration.TotalHours >= compareTime)
                        {
                            if(complaint.PickedBy == null)
                            {
                                unassignedComplaints.Add(complaint);
                            }
                            else
                            {
                                assignedComplaints.Add(complaint);
                            }
                        }
                    }
                }

                EscalationManagementDTO returnObject = new()
                {
                    assignedComplaints = _mapper.Map<IEnumerable<ComplaintTransferDTO>>(assignedComplaints).ToList(),
                    unassignedComplaints = _mapper.Map<IEnumerable<ComplaintTransferDTO>>(unassignedComplaints).ToList(),
                    totalEscalatedComplaints = assignedComplaints.Count + unassignedComplaints.Count,
                    complaintTypes = _mapper.Map<IEnumerable<ComplaintTypeTransferDTO>>(allComplaintTypes).ToList(),
                    escalationLevelHandlers = _mapper.Map<IEnumerable<EscalationMatrixTransferDTO>>(escalationMatrices.Where(x => x.ComplaintAttendants.Any(x => x.EscalationLevelId == handlerEscalationLevelID))).ToList(),
                    complaintsDistribution = complaintDistributionPercentages,
                    handlerEscalationLevelID = handlerEscalationLevelID,
                };

                foreach (var complaint in returnObject.assignedComplaints)
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
                foreach (var complaint in returnObject.unassignedComplaints)
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
                return new ApiOkResponse(returnObject);
            }
            catch(Exception error)
            {
                _logger.LogError("Exception occurred in  GetUserEscalationLevelDetails " + error);
                return new ApiResponse(500, error.Message);
            }
        }

        public async Task<long> GetHandlerEscalationLevelID()
        {
            var data = await _context.EscalationLevels.FirstOrDefaultAsync(x => x.Caption.ToLower().Contains("handler") && x.IsDeleted == false);
            return data == null ? 0 : data.Id;
        }

        public async Task<ApiResponse> MoveComplaintToNextStage(HttpContext context, MoveComplaintToNextStageDTO model)
        {
            try
            {
                long userProfileID = context.GetLoggedInUserId();
                Complaint complaint = await _context.Complaints
                    .Include(x => x.PickedBy)
                    .Include(x => x.ComplaintOrigin)
                    .FirstOrDefaultAsync(x => x.Id == model.complaintID);
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
                        await SendComplaintConfirmationMail(complaint, model.applicationUrl);
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
                _logger.LogError("Exception occurred in  MoveComplaintToNextStage " + error);
                return new ApiResponse(500, error.Message);
            }
        }

        public async Task<bool> SendComplaintConfirmationMail(Complaint complaint, string applicationUrl)
        {
            bool result = false;

            try
            {
                if (complaint == null)
                {
                    _logger.LogError("Complaint data is null");
                    return result;
                }

                if (String.IsNullOrWhiteSpace(applicationUrl))
                {
                    applicationUrl = "http://localhost:4200";
                }

                Supplier complainantSupplier = null;
                UserProfile complainantStaff = null;
                CustomerDivision complainantClient = null;
                switch (complaint.ComplaintOrigin.Caption.ToLower())
                {
                    case "supplier":
                        complainantSupplier = await _context.Suppliers.FindAsync(complaint.ComplainantId);
                        break;
                    case "staff":
                        complainantStaff = await _context.UserProfiles.FindAsync(complaint.ComplainantId);
                        break;
                    case "client":
                        complainantClient = await _context.CustomerDivisions.FindAsync(complaint.ComplainantId);
                        break;
                }

                string receipentEmail = complainantSupplier != null ? complainantSupplier.SupplierEmail : complainantStaff != null ? complainantStaff.Email : complainantClient.Email;

                ConfirmComplaintResolutionMailDTO model = new ConfirmComplaintResolutionMailDTO()
                {
                    Username = complainantSupplier != null ? complainantSupplier.SupplierName : complainantStaff != null ? complainantStaff.LastName : complainantClient.DivisionName,
                    Subject = "Confirmation of complaint resolution",
                    ComplaintId = complaint.Id,
                    ConfirmationLink = applicationUrl + "/#/confirm-complaint/" + complaint.Id,
                    DateComplaintReported = complaint.DateComplaintReported.Value,
                    HandlerName = complaint.PickedBy.LastName,
                    ReceipentEmailAddress = new string[1],
                };
                model.ReceipentEmailAddress[0] = receipentEmail;
                var response = await _mailAdapter.SendComplaintResolutionConfirmationMail(model);
                if (response.StatusCode == 200) result = true;
            }
            catch (Exception err) 
            {
                _logger.LogError("Exception occurred in  SendComplaintConfirmationMail " + err);
                result = false; 
            }

            return result;
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
                _logger.LogError("Exception occurred in  PickComplaint " + error);
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
                    .FirstOrDefaultAsync(x => x.TrackingId == model.TrackingNo || x.Id == model.ComplaintId);

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
                _logger.LogError("Exception occurred in  TrackComplaint " + error);
                return new ApiResponse(500, error.Message);
            }
        }

        public async Task<ApiResponse> ConfirmComplaintResolved(long complaintId)
        {
            try
            {
                Complaint complaint = await _context.Complaints.FirstOrDefaultAsync(x => x.Id == complaintId);
                complaint.IsConfirmedResolved = true;
                _context.Complaints.Update(complaint);
                await _context.SaveChangesAsync();
                return new ApiOkResponse(true);
            }
            catch(Exception error)
            {
                _logger.LogError("Exception occurred in  ConfirmComplaintResolved " + error);
                return new ApiResponse(500, error.Message);
            }
        }

        public async Task<ApiResponse> RunComplaintConfirmationCronJob()
        {
            try
            {
                List<Complaint> allComplaints = await _context.Complaints
                    .Where(
                        x => x.IsConfirmedResolved == false && 
                        x.IsDeleted == false &&
                        x.IsClosed == true
                        )
                    .ToListAsync();

                foreach(var complaint in allComplaints)
                {
                    TimeSpan duration = DateTime.Now - complaint.DateClosed.Value;
                    if(duration.TotalHours > 48)
                    {
                        complaint.IsConfirmedResolved = true;
                        _context.Complaints.Update(complaint);
                    }
                }

                await _context.SaveChangesAsync();
                return new ApiOkResponse(true);
            }
            catch(Exception error)
            {
                _logger.LogError("Exception occurred in  ConfirmComplaintResolved " + error);
                return new ApiResponse(500, error.Message);
            }
        }

        public async Task<ApiResponse> AssignComplaintToUser(AssignComplaintReceivingDTO model)
        {
            try
            {
                UserProfile userProfile = await _context.UserProfiles.FirstOrDefaultAsync(x => x.Id == model.UserId && x.IsDeleted == false);
                if (userProfile == null) return new ApiResponse(500, "No user with the passed ID exists");
                Complaint complaint = await _context.Complaints.FirstOrDefaultAsync(x => x.Id == model.ComplaintId && x.IsDeleted == false);
                if (complaint == null) return new ApiResponse(500, "No complaint with the passed ID exists");
                complaint.IsPicked = true;
                complaint.PickedById = userProfile.Id;
                complaint.DatePicked = DateTime.Now;
                _context.Complaints.Update(complaint);
                await _context.SaveChangesAsync();
                return new ApiOkResponse(true);
            }
            catch(Exception error)
            {
                _logger.LogError("Exception occurred in  AssignComplaintToUser " + error);
                return new ApiResponse(500, error.Message);
            }
        }

        public async Task<ApiResponse> MiniTrackComplaint(long ComplaintId)
        {
            try
            {
                ComplaintAssesment complaintAssesment = await _context.ComplaintAssesments.FirstOrDefaultAsync(x => x.ComplaintId == ComplaintId);
                ComplaintInvestigation complaintInvestigation = await _context.ComplaintInvestigations.FirstOrDefaultAsync(x => x.ComplaintId == ComplaintId);
                ComplaintResolution complaintResolution = await _context.ComplaintResolutions.FirstOrDefaultAsync(x => x.ComplaintId == ComplaintId);
                List<string> registrationEvidences = await _context.Evidences.Where(x => x.ComplaintId == ComplaintId && x.ComplaintStage == ComplaintStage.Registration).Select(x => x.ImageUrl).ToListAsync();
                List<string> assessmentEvidences = await _context.Evidences.Where(x => x.ComplaintId == ComplaintId && x.ComplaintStage == ComplaintStage.Assesment).Select(x => x.ImageUrl).ToListAsync();
                List<string> investiagtionEvidences = await _context.Evidences.Where(x => x.ComplaintId == ComplaintId && x.ComplaintStage == ComplaintStage.Investigation).Select(x => x.ImageUrl).ToListAsync();
                List<string> resolutionEvidences = await _context.Evidences.Where(x => x.ComplaintId == ComplaintId && x.ComplaintStage == ComplaintStage.Resolution).Select(x => x.ImageUrl).ToListAsync();

                var resultObject = new ComplaintTrackingTransferDTO()
                {
                    Assessment = _mapper.Map<ComplaintAssessmentTransferDTO>(complaintAssesment),
                    Investigation = _mapper.Map<ComplaintInvestigationTransferDTO>(complaintInvestigation),
                    Resolution = _mapper.Map<ComplaintResolutionTransferDTO>(complaintResolution),
                    RegistrationEvidenceUrls = registrationEvidences,
                    AssessmentEvidenceUrls = assessmentEvidences,
                    InvestigationEvidenceUrls = investiagtionEvidences,
                    ResolutionEvidenceUrls = resolutionEvidences,
                };
                return new ApiOkResponse(resultObject);
            }
            catch(Exception error)
            {
                _logger.LogError("Exception occurred in  MiniTrackComplaint " + error);
                return new ApiResponse(500, error.Message);
            }
        }

        public async Task<ApiResponse> GetHandlersRatings(HandlersRatingReceivingDTO model)
        {
            try
            {
                List<HandlersRatingTransferDTO> returnObject = new();
                List<UserProfile> handlers = await _context.UserProfiles.Where(x => model.HandlersIDs.Any(y => y == x.Id) && x.IsDeleted == false).ToListAsync();
                List<Complaint> handlersComplaint = await _context.Complaints.Where(x => model.HandlersIDs.Any(y => y == x.PickedById.Value) && (x.IsClosed != null || x.IsResolved != null) && x.IsDeleted == false).ToListAsync();
                foreach(var handler in handlers)
                {
                    HandlersRatingTransferDTO handlerRating = new HandlersRatingTransferDTO()
                    {
                        Username = handler.LastName,
                        Score = handlersComplaint.Where(x => x.PickedById == handler.Id).ToList().Count
                    };
                    returnObject.Add(handlerRating);
                }

                return new ApiOkResponse(returnObject);
            }
            catch(Exception error)
            {
                _logger.LogError("Exception occurred in  GetHandlersRatings " + error);
                return new ApiResponse(500, error.Message);
            }
        }
    }
}
