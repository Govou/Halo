using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
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
        public async Task<ActionResult> GetRegion()
        {
            var response = await _regionService.GetAllRegions();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var region = ((ApiOkResponse)response).Result;
            return Ok(region);
        }

        [HttpGet("name/{name}")]
        public async Task<ActionResult> GetByName(string name)
        {
            var response = await _regionService.GetRegionByName(name);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Region = ((ApiOkResponse)response).Result;
            return Ok(Region);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var response = await _regionService.GetRegionById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Region = ((ApiOkResponse)response).Result;
            return Ok(Region);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNewRegion(RegionReceivingDTO RegionReceiving)
        {
            var response = await _regionService.AddRegion(HttpContext, RegionReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var region = ((ApiOkResponse)response).Result;
            return Ok(region);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, RegionReceivingDTO RegionReceiving)
        {
            var response = await _regionService.UpdateRegion(HttpContext, id, RegionReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var region = ((ApiOkResponse)response).Result;
            return Ok(region);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var response = await _regionService.DeleteRegion(id);
            return StatusCode(response.StatusCode);
        }
    }
}