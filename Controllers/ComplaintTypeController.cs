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
    public class ComplaintTypeController : ControllerBase
    {
        private readonly IComplaintTypeService _ComplaintTypeService;

        public ComplaintTypeController(IComplaintTypeService serviceTypeService)
        {
            this._ComplaintTypeService = serviceTypeService;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetComplaintType()
        {
            var response = await _ComplaintTypeService.GetAllComplaintType();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var ComplaintType = ((ApiOkResponse)response).Result;
            return Ok(ComplaintType);
        }
        [HttpGet("caption/{name}")]
        public async Task<ActionResult> GetByCaption(string name)
        {
            var response = await _ComplaintTypeService.GetComplaintTypeByName(name);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var ComplaintType = ((ApiOkResponse)response).Result;
            return Ok(ComplaintType);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var response = await _ComplaintTypeService.GetComplaintTypeById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var ComplaintType = ((ApiOkResponse)response).Result;
            return Ok(ComplaintType);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNewComplaintType(ComplaintTypeReceivingDTO ComplaintTypeReceiving)
        {
            var response = await _ComplaintTypeService.AddComplaintType(HttpContext, ComplaintTypeReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var ComplaintType = ((ApiOkResponse)response).Result;
            return Ok(ComplaintType);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, ComplaintTypeReceivingDTO ComplaintTypeReceiving)
        {
            var response = await _ComplaintTypeService.UpdateComplaintType(HttpContext, id, ComplaintTypeReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var ComplaintType = ((ApiOkResponse)response).Result;
            return Ok(ComplaintType);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var response = await _ComplaintTypeService.DeleteComplaintType(id);
            return StatusCode(response.StatusCode);
        }
    }
}
