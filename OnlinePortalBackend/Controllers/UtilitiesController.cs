using Halobiz.Common.DTOs.ApiDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlinePortalBackend.MyServices;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilitiesController : ControllerBase
    {
        private readonly IUtilityService _utilityService;

        public UtilitiesController(IUtilityService utilityService)
        {
            _utilityService = utilityService;
        }

        [HttpGet("GetStates")]
        public async Task<ApiCommonResponse> GetStates()
        {
            return await _utilityService.GetStates();
        }

        [HttpGet("GetLocalGovtAreas")]
        public async Task<ApiCommonResponse> GetLocalGovtAreas(int stateId)
        {
            return await _utilityService.GetLocalGovtAreas(stateId);
        }

        [HttpGet("GetStateById")]
        public async Task<ApiCommonResponse> GetStateById(int id)
        {
            return await _utilityService.GetStateById(id);
        }

        [HttpGet("GetLocalGovtAreaById")]
        public async Task<ApiCommonResponse> GetLocalGovtAreaById(int id)
        {
            return await _utilityService.GetLocalGovtAreaById(id);
        }


        [HttpGet("GetBusinessTypes")]
        public async Task<ApiCommonResponse> GetBusinessTypes()
        {
            return await _utilityService.GetBusinessTypes();
        }
    }
}
