using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.MyServices.LAMS;
using Microsoft.AspNetCore.Mvc;

namespace HaloBiz.Controllers.LAMS
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LeadOriginController : ControllerBase
    {
        private readonly ILeadOriginService _leadOriginService;

        public LeadOriginController(ILeadOriginService leadOriginService)
        {
            this._leadOriginService = leadOriginService;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetLeadOrigin()
        {
            var response = await _leadOriginService.GetAllLeadOrigin();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var leadOrigin = ((ApiOkResponse)response).Result;
            return Ok(leadOrigin);
        }
        [HttpGet("caption/{name}")]
        public async Task<ActionResult> GetByCaption(string name)
        {
            var response = await _leadOriginService.GetLeadOriginByName(name);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var leadOrigin = ((ApiOkResponse)response).Result;
            return Ok(leadOrigin);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var response = await _leadOriginService.GetLeadOriginById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var leadOrigin = ((ApiOkResponse)response).Result;
            return Ok(leadOrigin);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNewLeadOrigin(LeadOriginReceivingDTO leadOriginReceiving)
        {
            var response = await _leadOriginService.AddLeadOrigin(HttpContext, leadOriginReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var leadOrigin = ((ApiOkResponse)response).Result;
            return Ok(leadOrigin);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, LeadOriginReceivingDTO leadOriginReceiving)
        {
            var response = await _leadOriginService.UpdateLeadOrigin(HttpContext, id, leadOriginReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var leadOrigin = ((ApiOkResponse)response).Result;
            return Ok(leadOrigin);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var response = await _leadOriginService.DeleteLeadOrigin(id);
            return StatusCode(response.StatusCode);
        }
    }
}