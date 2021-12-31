using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.MyServices;
using Microsoft.AspNetCore.Mvc;

namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ZoneController : ControllerBase
    {
        private readonly IZoneService _zoneService;

        public ZoneController(IZoneService zoneService)
        {
            this._zoneService = zoneService;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetZone()
        {
            var response = await _zoneService.GetAllZones();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var zone = ((ApiOkResponse)response).Result;
            return Ok(zone);
        }

        [HttpGet("name/{name}")]
        public async Task<ActionResult> GetByName(string name)
        {
            var response = await _zoneService.GetZoneByName(name);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Zone = ((ApiOkResponse)response).Result;
            return Ok(Zone);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var response = await _zoneService.GetZoneById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Zone = ((ApiOkResponse)response).Result;
            return Ok(Zone);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNewZone(ZoneReceivingDTO ZoneReceiving)
        {
            var response = await _zoneService.AddZone(HttpContext, ZoneReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var zone = ((ApiOkResponse)response).Result;
            return Ok(zone);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, ZoneReceivingDTO ZoneReceiving)
        {
            var response = await _zoneService.UpdateZone(HttpContext, id, ZoneReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var zone = ((ApiOkResponse)response).Result;
            return Ok(zone);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var response = await _zoneService.DeleteZone(id);
            return StatusCode(response.StatusCode);
        }
    }
}