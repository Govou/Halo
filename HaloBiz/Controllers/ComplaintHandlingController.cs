using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Halobiz.Common.Auths;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.MyServices;
using HaloBiz.MyServices.LAMS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ModuleName(HalobizModules.ComplaintManagement)]

    public class ComplaintHandlingController : ControllerBase
    {
        private readonly IComplaintHandlingService _complaintHandlingService;
        private readonly IConfiguration _configuration;
        private readonly string _applicationUrl;
        public ComplaintHandlingController(IComplaintHandlingService complaintHandlingService, IConfiguration configuration)
        {
            _complaintHandlingService = complaintHandlingService;
            _configuration = configuration;
            _applicationUrl = _configuration["ApplicationURL"] ?? _configuration.GetSection("AppSettings:ApplicationURL").Value;
        }

        [HttpGet("GetComplaintHandlingStats")]
        public async Task<ApiCommonResponse> GetComplaintHandlingStats()
        {
            return await _complaintHandlingService.GetComplaintHandlingStats(HttpContext);
        }

        [HttpGet]
        public async Task<ApiCommonResponse> GetComplaintsHandling()
        {
            return await _complaintHandlingService.GetComplaintsHandling(HttpContext);
        }

        [HttpPost]
        public async Task<ApiCommonResponse> PickComplaint(PickComplaintDTO model)
        {
            return await _complaintHandlingService.PickComplaint(HttpContext, model);
        }

        [HttpPost("MoveComplaintToNextStage")]
        public async Task<ApiCommonResponse> MoveComplaintToNextStage(MoveComplaintToNextStageDTO model)
        {
            model.applicationUrl = _applicationUrl;
            return await _complaintHandlingService.MoveComplaintToNextStage(HttpContext, model);
        }

        [HttpPost("TrackComplaint")]
        public async Task<ApiCommonResponse> TrackComplaint(ComplaintTrackingRecievingDTO model)
        {
            return await _complaintHandlingService.TrackComplaint(model);
        }

        [HttpGet("GetUserEscalationLevelDetails")]
        public async Task<ApiCommonResponse> GetUserEscalationLevelDetails()
        {
            return await _complaintHandlingService.GetUserEscalationLevelDetails(HttpContext);
        }

        [HttpGet("ConfirmComplaintResolved/{id}")]
        public async Task<ApiCommonResponse> ConfirmComplaintResolved(long id)
        {
            return await _complaintHandlingService.ConfirmComplaintResolved(id);
        }

        [HttpGet("RunComplaintConfirmationCronJob")]
        public async Task<ApiCommonResponse> RunComplaintConfirmationCronJob()
        {
            return await _complaintHandlingService.RunComplaintConfirmationCronJob();
        }

        [HttpPost("AssignComplaintToUser")]
        public async Task<ApiCommonResponse> AssignComplaintToUser(AssignComplaintReceivingDTO model)
        {
            return await _complaintHandlingService.AssignComplaintToUser(HttpContext, model);
        }

        [HttpGet("TrackComplaint/{complaintId}")]
        public async Task<ApiCommonResponse> TrackComplaint(long complaintId)
        {
            return await _complaintHandlingService.MiniTrackComplaint(complaintId);
        }

        [HttpPost("HandlersRating")]
        public async Task<ApiCommonResponse> HandlersRatings(HandlersRatingReceivingDTO model)
        {
            return await _complaintHandlingService.GetHandlersRatings(model);
        }
    }
}
