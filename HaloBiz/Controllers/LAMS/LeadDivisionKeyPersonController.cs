using System.Threading.Tasks;
using Halobiz.Common.Auths;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.ReceivingDTOs.LAMS;
using HaloBiz.MyServices.LAMS;
using Microsoft.AspNetCore.Mvc;

namespace HaloBiz.Controllers.LAMS
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ModuleName(HalobizModules.LeadAdministration,23)]

    public class LeadDivisionKeyPersonController : ControllerBase
    {
        private readonly ILeadDivisionKeyPersonService _leadDivisionKeyPersonService;

        public LeadDivisionKeyPersonController(ILeadDivisionKeyPersonService leadDivisionKeyPersonService)
        {
            this._leadDivisionKeyPersonService = leadDivisionKeyPersonService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetLeadKeyPeople()
        {
            return await _leadDivisionKeyPersonService.GetAllLeadDivisionKeyPerson();
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _leadDivisionKeyPersonService.GetLeadDivisionKeyPersonById(id);
        }

        [HttpGet("LeadDivision/{leadDivisionId}")]
        public async Task<ApiCommonResponse> GetByLeadDivisionId(long leadDivisionId)
        {
            return await _leadDivisionKeyPersonService.GetAllLeadDivisionKeyPersonsByLeadDivisionId(leadDivisionId);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, LeadDivisionKeyPersonReceivingDTO LeadDivisionKeyPersonReceiving)
        {
            return await _leadDivisionKeyPersonService.UpdateLeadDivisionKeyPerson(HttpContext, id, LeadDivisionKeyPersonReceiving);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewLeadDivisionKeyPerson(LeadDivisionKeyPersonReceivingDTO leadDivisionKeyPersonReceiving)
        {
            return await _leadDivisionKeyPersonService.AddLeadDivisionKeyPerson(HttpContext, leadDivisionKeyPersonReceiving);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _leadDivisionKeyPersonService.DeleteKeyPerson(id);
        }

  
    }
}



        