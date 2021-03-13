using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.ReceivingDTOs.LAMS;
using HaloBiz.MyServices.LAMS;
using Microsoft.AspNetCore.Mvc;

namespace HaloBiz.Controllers.LAMS
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LeadDivisionContactController : ControllerBase
    {
        private readonly ILeadDivisionContactService _LeadDivisionContactService;

        public LeadDivisionContactController(ILeadDivisionContactService LeadDivisionContactService)
        {
            this._LeadDivisionContactService = LeadDivisionContactService;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetLeadContacts()
        {
            var response = await _LeadDivisionContactService.GetAllLeadDivisionContact();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var LeadDivisionContact = ((ApiOkResponse)response).Result;
            return Ok(LeadDivisionContact);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var response = await _LeadDivisionContactService.GetLeadDivisionContactById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var LeadDivisionContact = ((ApiOkResponse)response).Result;
            return Ok(LeadDivisionContact);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNewLeadDivisionContact(long leadDivisionId, LeadDivisionContactReceivingDTO leadDivisionContactReceiving)
        {
            var response = await _LeadDivisionContactService.AddLeadDivisionContact(HttpContext, leadDivisionId, leadDivisionContactReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var leadContact = ((ApiOkResponse)response).Result;
            return Ok(leadContact);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, LeadDivisionContactReceivingDTO LeadDivisionContactReceiving)
        {
            var response = await _LeadDivisionContactService.UpdateLeadDivisionContact(HttpContext, id, LeadDivisionContactReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var LeadDivisionContact = ((ApiOkResponse)response).Result;
            return Ok(LeadDivisionContact);
        }
    }
}
