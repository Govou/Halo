using Halobiz.Common.DTOs.TransferDTOs;
using Microsoft.AspNetCore.Mvc;
using OnlinePortalBackend.DTOs.ApiDTOs;
using OnlinePortalBackend.DTOs.ReceivingDTOs;
using OnlinePortalBackend.DTOs.TransferDTOs;
using OnlinePortalBackend.MyServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PortalComplaintController : ControllerBase
    {
        private readonly IComplaintService _portalComplaintService;

        public PortalComplaintController(IComplaintService portalComplaintService)
        {
            _portalComplaintService = portalComplaintService;
        }

        //[HttpGet("")]
        //public async Task<ActionResult> GetPortalComplaints()
        //{
        //    var response = await _portalComplaintService.FindAllPortalComplaints();
        //    if (response.StatusCode >= 400)
        //        return StatusCode(response.StatusCode, response);
        //    var portalComplaint = ((ApiOkResponse)response).Result;
        //    return Ok((IEnumerable<PortalComplaintTransferDTO>)portalComplaint);
        //}

        //[HttpGet("{id}")]
        //public async Task<ActionResult> GetById(long id)
        //{
        //    var response = await _portalComplaintService.FindPortalComplaintById(id);
        //    if (response.StatusCode >= 400)
        //        return StatusCode(response.StatusCode, response);
        //    var portalComplaint = ((ApiOkResponse)response).Result;
        //    return Ok((PortalComplaintTransferDTO)portalComplaint);
        //}

        [HttpPost("CreateComplaint")]
        public async Task<ActionResult> AddNewPortalComplaint(ComplaintDTO complaint)
        {
            var response = await _portalComplaintService.CreateComplaint(complaint);
            return Ok(response);
        }

        [HttpGet("GetComplaintType")]
        public async Task<ActionResult> GetComplainType()
        {
            var response = await _portalComplaintService.GetComplainType();
            return Ok(response);
        }

        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateById(long id, PortalComplaintReceivingDTO portalComplaintReceiving)
        //{
        //    var response = await _portalComplaintService.UpdatePortalComplaint(HttpContext, id, portalComplaintReceiving);
        //    if (response.StatusCode >= 400)
        //        return StatusCode(response.StatusCode, response);
        //    var portalComplaint = ((ApiOkResponse)response).Result;
        //    return Ok((PortalComplaintTransferDTO)portalComplaint);
        //}

        //[HttpDelete("{id}")]
        //public async Task<ActionResult> DeleteById(int id)
        //{
        //    var response = await _portalComplaintService.DeletePortalComplaint(id);
        //    return StatusCode(response.StatusCode);
        //}
    }
}