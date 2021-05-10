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
            var Complaint = ((ApiOkResponse)response).Result;
            return Ok(Complaint);
        }

        [HttpGet]
        public async Task<ActionResult> GetComplaintsHandling()
        {
            var response = await _complaintHandlingService.GetComplaintsHandling(HttpContext);
            if (response.StatusCode != 200)
                return StatusCode(response.StatusCode, response);
            var Complaint = ((ApiOkResponse)response).Result;
            return Ok(Complaint);
        }

        [HttpPost]
        public async Task<ActionResult> PickComplaint(PickComplaintDTO model)
        {
            var response = await _complaintHandlingService.PickComplaint(HttpContext, model);
            if (response.StatusCode != 200)
                return StatusCode(response.StatusCode, response);
            var Complaint = ((ApiOkResponse)response).Result;
            return Ok(Complaint);
        }

        [HttpPost("MoveComplaintToNextStage")]
        public async Task<ActionResult> MoveComplaintToNextStage(MoveComplaintToNextStageDTO model)
        {
            var response = await _complaintHandlingService.MoveComplaintToNextStage(HttpContext, model);
            if (response.StatusCode != 200)
                return StatusCode(response.StatusCode, response);
            var Complaint = ((ApiOkResponse)response).Result;
            return Ok(Complaint);
        }

        [HttpPost("TrackComplaint")]
        public async Task<ActionResult> TrackComplaint(ComplaintTrackingRecievingDTO model)
        {
            var response = await _complaintHandlingService.TrackComplaint(model);
            if (response.StatusCode != 200)
                return StatusCode(response.StatusCode, response);
            var Complaint = ((ApiOkResponse)response).Result;
            return Ok(Complaint);
        }
    }
}
