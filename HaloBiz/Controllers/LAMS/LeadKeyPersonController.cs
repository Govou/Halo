using System.Threading.Tasks;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.ReceivingDTOs.LAMS;
using HaloBiz.MyServices.LAMS;
using Microsoft.AspNetCore.Mvc;

namespace HaloBiz.Controllers.LAMS
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LeadKeyPersonController : ControllerBase
    {
        private readonly ILeadKeyPersonService _leadKeyPersonService;

        public LeadKeyPersonController(ILeadKeyPersonService leadKeyPersonService)
        {
            this._leadKeyPersonService = leadKeyPersonService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetLeadKeyPeople()
        {
            return await _leadKeyPersonService.GetAllLeadKeyPerson();
        }

        [HttpGet("Lead/{leadId}")]
        public async Task<ApiCommonResponse> GetLeadKeyPeopleByLeadId(long leadId)
        {
            return await _leadKeyPersonService.GetAllLeadKeyPersonsByLeadId(leadId);
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _leadKeyPersonService.GetLeadKeyPersonById(id);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewLeadKeyPerson(LeadKeyPersonReceivingDTO leadKeyPersonReceiving)
        {
            return await _leadKeyPersonService.AddLeadKeyPerson(HttpContext, leadKeyPersonReceiving);
        }
        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, LeadKeyPersonReceivingDTO leadKeyPersonReceivingDTO)
        {
            return await _leadKeyPersonService.UpdateLeadKeyPerson(HttpContext, id, leadKeyPersonReceivingDTO);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _leadKeyPersonService.DeleteKeyPerson(id);
        }

    }
}



        