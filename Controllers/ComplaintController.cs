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
    public class ComplaintController : ControllerBase
    {
        private readonly IComplaintService _ComplaintService;

        public ComplaintController(IComplaintService serviceTypeService)
        {
            this._ComplaintService = serviceTypeService;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetComplaint()
        {
            var response = await _ComplaintService.GetAllComplaint();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Complaint = ((ApiOkResponse)response).Result;
            return Ok(Complaint);
        }
        /*[HttpGet("caption/{name}")]
        public async Task<ActionResult> GetByCaption(string name)
        {
            var response = await _ComplaintService.GetComplaintByName(name);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Complaint = ((ApiOkResponse)response).Result;
            return Ok(Complaint);
        }*/

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var response = await _ComplaintService.GetComplaintById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Complaint = ((ApiOkResponse)response).Result;
            return Ok(Complaint);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNewComplaint(ComplaintReceivingDTO ComplaintReceiving)
        {
            var response = await _ComplaintService.AddComplaint(HttpContext, ComplaintReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Complaint = ((ApiOkResponse)response).Result;
            return Ok(Complaint);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, ComplaintReceivingDTO ComplaintReceiving)
        {
            var response = await _ComplaintService.UpdateComplaint(HttpContext, id, ComplaintReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Complaint = ((ApiOkResponse)response).Result;
            return Ok(Complaint);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var response = await _ComplaintService.DeleteComplaint(id);
            return StatusCode(response.StatusCode);
        }
    }
}
