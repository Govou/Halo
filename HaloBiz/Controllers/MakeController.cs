
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.MyServices;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MakeController : Controller
    {
        private readonly IMakeService _makeService;

        public MakeController(IMakeService makeService)
        {
            this._makeService = makeService;
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddMake(MakeReceivingDTO makeReceiving)
        {
            return await _makeService.AddMake(HttpContext, makeReceiving);
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetAllMake()
        {
            return await _makeService.GetAllMake();
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetMakeById(long id)
        {
            return await _makeService.GetMakeById(id);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, MakeReceivingDTO makeReceiving)
        {
            return await _makeService.UpdateMake(HttpContext, id, makeReceiving);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteMake(long id)
        {
            return await _makeService.DeleteMake(id);
        }
    }
}
