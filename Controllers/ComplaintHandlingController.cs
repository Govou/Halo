using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
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
        public async Task<ActionResult> GetComplaintHandlingStats()
        {
            var response = await _complaintHandlingService.GetComplaintHandlingStats(HttpContext);
            if (response.StatusCode != 200)
                return StatusCode(response.StatusCode, response);
            var returnData = ((ApiOkResponse)response).Result;
            return Ok(returnData);
        }

        [HttpGet]
        public async Task<ActionResult> GetComplaintsHandling()
        {
            var response = await _complaintHandlingService.GetComplaintsHandling(HttpContext);
            if (response.StatusCode != 200)
                return StatusCode(response.StatusCode, response);
            var returnData = ((ApiOkResponse)response).Result;
            return Ok(returnData);
        }

        [HttpPost]
        public async Task<ActionResult> PickComplaint(PickComplaintDTO model)
        {
            var response = await _complaintHandlingService.PickComplaint(HttpContext, model);
            if (response.StatusCode != 200)
                return StatusCode(response.StatusCode, response);
            var returnData = ((ApiOkResponse)response).Result;
            return Ok(returnData);
        }

        [HttpPost("MoveComplaintToNextStage")]
        public async Task<ActionResult> MoveComplaintToNextStage(MoveComplaintToNextStageDTO model)
        {
            model.applicationUrl = _applicationUrl;
            var response = await _complaintHandlingService.MoveComplaintToNextStage(HttpContext, model);
            if (response.StatusCode != 200)
                return StatusCode(response.StatusCode, response);
            var returnData = ((ApiOkResponse)response).Result;
            return Ok(returnData);
        }

        [HttpPost("TrackComplaint")]
        public async Task<ActionResult> TrackComplaint(ComplaintTrackingRecievingDTO model)
        {
            var response = await _complaintHandlingService.TrackComplaint(model);
            if (response.StatusCode != 200)
                return StatusCode(response.StatusCode, response);
            var returnData = ((ApiOkResponse)response).Result;
            return Ok(returnData);
        }

        [HttpGet("GetUserEscalationLevelDetails")]
        public async Task<ActionResult> GetUserEscalationLevelDetails()
        {
            var response = await _complaintHandlingService.GetUserEscalationLevelDetails(HttpContext);
            if (response.StatusCode != 200)
                return StatusCode(response.StatusCode, response);
            var returnData = ((ApiOkResponse)response).Result;
            return Ok(returnData);
        }

        [HttpGet("ConfirmComplaintResolved/{id}")]
        public async Task<ActionResult> ConfirmComplaintResolved(long id)
        {
            var response = await _complaintHandlingService.ConfirmComplaintResolved(id);
            if (response.StatusCode != 200)
                return StatusCode(response.StatusCode, response);
            var returnData = ((ApiOkResponse)response).Result;
            return Ok(returnData);
        }

        [HttpGet("RunComplaintConfirmationCronJob")]
        public async Task<ActionResult> RunComplaintConfirmationCronJob()
        {
            var response = await _complaintHandlingService.RunComplaintConfirmationCronJob();
            if (response.StatusCode != 200)
                return StatusCode(response.StatusCode, response);
            var returnData = ((ApiOkResponse)response).Result;
            return Ok(returnData);
        }

        [HttpPost("AssignComplaintToUser")]
        public async Task<ActionResult> AssignComplaintToUser(AssignComplaintReceivingDTO model)
        {
            var response = await _complaintHandlingService.AssignComplaintToUser(model);
            if (response.StatusCode != 200)
                return StatusCode(response.StatusCode, response);
            var returnData = ((ApiOkResponse)response).Result;
            return Ok(returnData);
        }

        [HttpGet("TrackComplaint/{complaintId}")]
        public async Task<ActionResult> TrackComplaint(long complaintId)
        {
            var response = await _complaintHandlingService.MiniTrackComplaint(complaintId);
            if (response.StatusCode != 200)
                return StatusCode(response.StatusCode, response);
            var returnData = ((ApiOkResponse)response).Result;
            return Ok(returnData);
        }

        [HttpPost("HandlersRating")]
        public async Task<ActionResult> HandlersRatings(HandlersRatingReceivingDTO model)
        {
            var response = await _complaintHandlingService.GetHandlersRatings(model);
            if (response.StatusCode != 200)
                return StatusCode(response.StatusCode, response);
            var returnData = ((ApiOkResponse)response).Result;
            return Ok(returnData);
        }
    }
}
