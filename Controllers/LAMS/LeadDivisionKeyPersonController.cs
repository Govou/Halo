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
    public class LeadDivisionKeyPersonController : ControllerBase
    {
        private readonly ILeadDivisionKeyPersonService _leadDivisionKeyPersonService;

        public LeadDivisionKeyPersonController(ILeadDivisionKeyPersonService leadDivisionKeyPersonService)
        {
            this._leadDivisionKeyPersonService = leadDivisionKeyPersonService;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetLeadKeyPeople()
        {
            var response = await _leadDivisionKeyPersonService.GetAllLeadDivisionKeyPerson();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var LeadDivisionKeyPerson = ((ApiOkResponse)response).Result;
            return Ok(LeadDivisionKeyPerson);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var response = await _leadDivisionKeyPersonService.GetLeadDivisionKeyPersonById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var LeadDivisionKeyPerson = ((ApiOkResponse)response).Result;
            return Ok(LeadDivisionKeyPerson);
        }

        [HttpGet("LeadDivision/{leadDivisionId}")]
        public async Task<ActionResult> GetByLeadDivisionId(long leadDivisionId)
        {
            var response = await _leadDivisionKeyPersonService.GetAllLeadDivisionKeyPersonsByLeadDivisionId(leadDivisionId);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var LeadDivisionKeyPerson = ((ApiOkResponse)response).Result;
            return Ok(LeadDivisionKeyPerson);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, LeadDivisionKeyPersonReceivingDTO LeadDivisionKeyPersonReceiving)
        {
            var response = await _leadDivisionKeyPersonService.UpdateLeadDivisionKeyPerson(HttpContext, id, LeadDivisionKeyPersonReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var LeadDivisionKeyPerson = ((ApiOkResponse)response).Result;
            return Ok(LeadDivisionKeyPerson);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNewLeadDivisionKeyPerson(LeadDivisionKeyPersonReceivingDTO leadDivisionKeyPersonReceiving)
        {
            var response = await _leadDivisionKeyPersonService.AddLeadDivisionKeyPerson(HttpContext, leadDivisionKeyPersonReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var leadDivision = ((ApiOkResponse)response).Result;
            return Ok(leadDivision);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var response = await _leadDivisionKeyPersonService.DeleteKeyPerson(id);
            return StatusCode(response.StatusCode);
        }

  
    }
}



        