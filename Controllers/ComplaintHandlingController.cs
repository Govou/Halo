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

namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ComplaintHandlingController : ControllerBase
    {
        private readonly IComplaintHandlingService _complaintHandlingService;
        public ComplaintHandlingController(IComplaintHandlingService complaintHandlingService)
        {
            _complaintHandlingService = complaintHandlingService;
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
    }
}
