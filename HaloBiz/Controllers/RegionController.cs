using System.Threading.Tasks;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.MyServices;
using Microsoft.AspNetCore.Mvc;

namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RegionController : ControllerBase
    {
        private readonly IRegionService _regionService;

        public RegionController(IRegionService regionService)
        {
            this._regionService = regionService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetRegion()
        {
            return await _regionService.GetAllRegions();
        }

        [HttpGet("name/{name}")]
        public async Task<ApiCommonResponse> GetByName(string name)
        {
            return await _regionService.GetRegionByName(name);
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _regionService.GetRegionById(id);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewRegion(RegionReceivingDTO RegionReceiving)
        {
            return await _regionService.AddRegion(HttpContext, RegionReceiving);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, RegionReceivingDTO RegionReceiving)
        {
            return await _regionService.UpdateRegion(HttpContext, id, RegionReceiving);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _regionService.DeleteRegion(id);
        }
    }
}