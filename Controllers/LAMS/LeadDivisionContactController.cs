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
        public async Task<ApiCommonResponse> GetLeadContacts()
        {
            return await _LeadDivisionContactService.GetAllLeadDivisionContact();
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _LeadDivisionContactService.GetLeadDivisionContactById(id);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewLeadDivisionContact(long leadDivisionId, LeadDivisionContactReceivingDTO leadDivisionContactReceiving)
        {
            return await _LeadDivisionContactService.AddLeadDivisionContact(HttpContext, leadDivisionId, leadDivisionContactReceiving);
            
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, LeadDivisionContactReceivingDTO LeadDivisionContactReceiving)
        {
            return await _LeadDivisionContactService.UpdateLeadDivisionContact(HttpContext, id, LeadDivisionContactReceiving);
            
        }
    }
}
