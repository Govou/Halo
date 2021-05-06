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

        [HttpGet("GetComplaintHandlingStats/{userProfileid}")]
        public async Task<ActionResult> GetComplaintHandlingStats(long userProfileid)
        {
            var response = await _complaintHandlingService.GetComplaintHandlingStats(HttpContext, userProfileid);
            if (response.StatusCode != 200)
                return StatusCode(response.StatusCode, response);
            var Complaint = ((ApiOkResponse)response).Result;
            return Ok(Complaint);
        }

        [HttpGet("{userProfileid}")]
        public async Task<ActionResult> GetComplaintsHandling(long userProfileid)
        {
            var response = await _complaintHandlingService.GetComplaintsHandling(HttpContext, userProfileid);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Complaint = ((ApiOkResponse)response).Result;
            return Ok(Complaint);
        }
    }
}
