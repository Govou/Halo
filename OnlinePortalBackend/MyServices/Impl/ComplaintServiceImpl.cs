using Halobiz.Common.DTOs.ApiDTOs;
using Halobiz.Common.DTOs.TransferDTOs;
using Microsoft.Extensions.Configuration;
using OnlinePortalBackend.Adapters;
using OnlinePortalBackend.Repository;
using System.Threading.Tasks;
using Flurl.Http;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Logging;
using OnlinePortalBackend.DTOs.TransferDTOs;
using HalobizMigrations.Models.Complaints;
using HalobizMigrations.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AutoMapper;

namespace OnlinePortalBackend.MyServices.Impl
{
    public class ComplaintServiceImpl : IComplaintService
    {
        IComplaintRepository _complaintRepository;
        private readonly IApiInterceptor _apiInterceptor;
        private readonly IConfiguration _configuration;
        private string _HalobizBaseUrl;
        private readonly ILogger<ComplaintServiceImpl> _logger;
        private readonly HalobizContext _context;
        private readonly IMapper _mapper;

        public ComplaintServiceImpl(IComplaintRepository complaintRepository, 
            IApiInterceptor apiInterceptor, 
            IConfiguration configuration, 
            ILogger<ComplaintServiceImpl> logger, 
            HalobizContext context,
            IMapper mapper)
        {
            _complaintRepository = complaintRepository;
            _apiInterceptor = apiInterceptor;
            _configuration = configuration;
            _HalobizBaseUrl = _configuration["HalobizBaseUrl"] ?? _configuration.GetSection("AppSettings:HalobizBaseUrl").Value;
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiCommonResponse> CreateComplaint(ComplaintDTO complaint)
        {
            ApiCommonResponse responseData = new ApiCommonResponse();
            try
            {
                responseData = await _complaintRepository.CreateComplaint(complaint);
                return responseData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ex.Message);
            }
        }

        public async Task<ApiCommonResponse> GetComplainType()
        {
            var complaints = _complaintRepository.GetComplainTypes();

            if (complaints == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }
            return CommonResponse.Send(ResponseCodes.SUCCESS, complaints);
        }

        public async Task<ApiCommonResponse> TrackComplaint(ComplaintTrackingDTO model)
        {
            try
            {
                Complaint complaint = await _context.Complaints
                    .Include(x => x.ComplaintOrigin)
                    .Include(x => x.ComplaintType)
                    .Include(x => x.ComplaintSource)
                    .Include(x => x.PickedBy)
                    .FirstOrDefaultAsync(x => x.TrackingId == model.TrackingNo || x.Id == model.ComplaintId);

                if (complaint == null) return CommonResponse.Send(ResponseCodes.FAILURE, null, "No Complaint with the passed tracking number exists.");

                var complaintTransferDTOs = _mapper.Map<ComplaintTransferDTO>(complaint);
                complaintTransferDTOs.Complainant = await _context.CustomerDivisions.FindAsync(complaint.ComplainantId);

                ComplaintAssesment complaintAssesment = await _context.ComplaintAssesments.FirstOrDefaultAsync(x => x.ComplaintId == complaint.Id);
                ComplaintInvestigation complaintInvestigation = await _context.ComplaintInvestigations.FirstOrDefaultAsync(x => x.ComplaintId == complaint.Id);
                ComplaintResolution complaintResolution = await _context.ComplaintResolutions.FirstOrDefaultAsync(x => x.ComplaintId == complaint.Id);
                List<Evidence> complaintEvidences = await _context.Evidences.Where(x => x.ComplaintId == complaint.Id).ToListAsync();
                List<string> registrationEvidences = complaintEvidences.Where(x => x.ComplaintStage == ComplaintStage.Registration).Select(x => x.ImageUrl).ToList();
                List<string> assessmentEvidences = complaintEvidences.Where(x => x.ComplaintStage == ComplaintStage.Assesment).Select(x => x.ImageUrl).ToList();
                List<string> investiagtionEvidences = complaintEvidences.Where(x => x.ComplaintStage == ComplaintStage.Investigation).Select(x => x.ImageUrl).ToList();
                List<string> resolutionEvidences = complaintEvidences.Where(x => x.ComplaintStage == ComplaintStage.Resolution).Select(x => x.ImageUrl).ToList();
                List<string> closureEvidences = complaintEvidences.Where(x => x.ComplaintStage == ComplaintStage.Closure).Select(x => x.ImageUrl).ToList();
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
                    ClosureEvidenceUrls = closureEvidences,
                    UserProfileImageUrl = complaint.PickedBy == null ? String.Empty : complaint.PickedBy.ImageUrl,
                    TotalHandlerCases = complaint.PickedById == null ? 0 : totalHandlerCases.Count(),
                    TotalHandlerCasesResolved = complaint.PickedById == null ? 0 : Math.Round(totalCasesResolvedPercentage, 2),
                    TotalHanlderCasesUnresolved = complaint.PickedById == null ? 0 : Math.Round(totalCasesUnresolvedPercentage, 2),
                    EstimatedDateResolved = escalationMatrix == null ? complaint.DateRegistered : complaint.DateRegistered.AddHours(escalationMatrix.Level1MaxResolutionTimeInHrs),
                    HandlerName = complaint.PickedBy == null ? String.Empty : complaint.PickedBy.LastName + " " + complaint.PickedBy.FirstName,
                };
                return CommonResponse.Send(ResponseCodes.SUCCESS, resultObject);
            }
            catch (Exception error)
            {
                _logger.LogError("Exception occurred in  TrackComplaint " + error);
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "System errors");
            }
        }

        public async Task<ApiCommonResponse> GetAllComplaints(int userId)
        {

            var complaints = _complaintRepository.GetAllComplaints(userId);

            if (complaints == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }
            return CommonResponse.Send(ResponseCodes.SUCCESS, complaints);
        }
    }
}
