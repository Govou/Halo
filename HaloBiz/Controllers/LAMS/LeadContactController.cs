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
        public async Task<ApiCommonResponse> GetLeadConatact()
        {
            return await _leadContactService.GetAllLeadContact();
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _leadContactService.GetLeadContactById(id);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewLeadContact(long leadId, LeadContactReceivingDTO leadContactReceiving)
        {
            return await _leadContactService.AddLeadContact(HttpContext, leadId,  leadContactReceiving);
        }


        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, LeadContactReceivingDTO leadContactReceivingDTO)
        {
            return await _leadContactService.UpdateLeadContact(HttpContext, id, leadContactReceivingDTO);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _leadContactService.DeleteLeadContact(id);
        }


    }
}


    