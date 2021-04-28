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
    public class ComplaintOriginController : ControllerBase
    {
        private readonly IComplaintOriginService _ComplaintOriginService;

        public ComplaintOriginController(IComplaintOriginService serviceTypeService)
        {
            this._ComplaintOriginService = serviceTypeService;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetComplaintOrigin()
        {
            var response = await _ComplaintOriginService.GetAllComplaintOrigin();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var ComplaintOrigin = ((ApiOkResponse)response).Result;
            return Ok(ComplaintOrigin);
        }
        [HttpGet("caption/{name}")]
        public async Task<ActionResult> GetByCaption(string name)
        {
            var response = await _ComplaintOriginService.GetComplaintOriginByName(name);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var ComplaintOrigin = ((ApiOkResponse)response).Result;
            return Ok(ComplaintOrigin);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var response = await _ComplaintOriginService.GetComplaintOriginById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var ComplaintOrigin = ((ApiOkResponse)response).Result;
            return Ok(ComplaintOrigin);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNewComplaintOrigin(ComplaintOriginReceivingDTO ComplaintOriginReceiving)
        {
            var response = await _ComplaintOriginService.AddComplaintOrigin(HttpContext, ComplaintOriginReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var ComplaintOrigin = ((ApiOkResponse)response).Result;
            return Ok(ComplaintOrigin);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, ComplaintOriginReceivingDTO ComplaintOriginReceiving)
        {
            var response = await _ComplaintOriginService.UpdateComplaintOrigin(HttpContext, id, ComplaintOriginReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var ComplaintOrigin = ((ApiOkResponse)response).Result;
            return Ok(ComplaintOrigin);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var response = await _ComplaintOriginService.DeleteComplaintOrigin(id);
            return StatusCode(response.StatusCode);
        }
    }
}
