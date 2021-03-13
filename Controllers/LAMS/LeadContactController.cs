using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.ReceivingDTOs.LAMS;
using HaloBiz.MyServices.LAMS;
using Microsoft.AspNetCore.Mvc;

namespace HaloBiz.Controllers.LAMS
{
    [Route("api/v1/Lead/{leadId}/LeadContact")]
    [ApiController]
    public class LeadContactController : ControllerBase
    {
        private readonly ILeadContactService _leadContactService;

        public LeadContactController(ILeadContactService leadContactService)
        {
            this._leadContactService = leadContactService;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetLeadConatact()
        {
            var response = await _leadContactService.GetAllLeadContact();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var leadContact = ((ApiOkResponse)response).Result;
            return Ok(leadContact);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var response = await _leadContactService.GetLeadContactById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var leadContact = ((ApiOkResponse)response).Result;
            return Ok(leadContact);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNewLeadContact(long leadId, LeadContactReceivingDTO leadContactReceiving)
        {
            var response = await _leadContactService.AddLeadContact(HttpContext, leadId,  leadContactReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var leadContact = ((ApiOkResponse)response).Result;
            return Ok(leadContact);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, LeadContactReceivingDTO leadContactReceivingDTO)
        {
            var response = await _leadContactService.UpdateLeadContact(HttpContext, id, leadContactReceivingDTO);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var leadContact = ((ApiOkResponse)response).Result;
            return Ok(leadContact);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var response = await _leadContactService.DeleteLeadContact(id);
            return StatusCode(response.StatusCode);
        }


    }
}


    