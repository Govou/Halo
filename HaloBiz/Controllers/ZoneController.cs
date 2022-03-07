using System.Threading.Tasks;
using Halobiz.Common.DTOs.ApiDTOs;
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
        public async Task<ApiCommonResponse> GetZone()
        {
            return await _zoneService.GetAllZones(); 
        }

        [HttpGet("name/{name}")]
        public async Task<ApiCommonResponse> GetByName(string name)
        {
            return await _zoneService.GetZoneByName(name); 
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _zoneService.GetZoneById(id); 
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewZone(ZoneReceivingDTO ZoneReceiving)
        {
            return await _zoneService.AddZone(HttpContext, ZoneReceiving); 
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, ZoneReceivingDTO ZoneReceiving)
        {
            return await _zoneService.UpdateZone(HttpContext, id, ZoneReceiving); 
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _zoneService.DeleteZone(id);
         }
    }
}