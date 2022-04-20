using System.Threading.Tasks;
using Halobiz.Common.Auths;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.MyServices.LAMS;
using Microsoft.AspNetCore.Mvc;

namespace HaloBiz.Controllers.LAMS
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ModuleName(HalobizModules.Setups,122)]

    public class LeadOriginController : ControllerBase
    {
        private readonly ILeadOriginService _leadOriginService;

        public LeadOriginController(ILeadOriginService leadOriginService)
        {
            this._leadOriginService = leadOriginService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetLeadOrigin()
        {
            return await _leadOriginService.GetAllLeadOrigin();
        }
        [HttpGet("caption/{name}")]
        public async Task<ApiCommonResponse> GetByCaption(string name)
        {
            return await _leadOriginService.GetLeadOriginByName(name);
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _leadOriginService.GetLeadOriginById(id);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewLeadOrigin(LeadOriginReceivingDTO leadOriginReceiving)
        {
            return await _leadOriginService.AddLeadOrigin(HttpContext, leadOriginReceiving);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, LeadOriginReceivingDTO leadOriginReceiving)
        {
            return await _leadOriginService.UpdateLeadOrigin(HttpContext, id, leadOriginReceiving);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _leadOriginService.DeleteLeadOrigin(id);
        }
    }
}