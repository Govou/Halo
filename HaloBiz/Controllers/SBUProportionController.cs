using System.Threading.Tasks;
using Halobiz.Common.Auths;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.MyServices;
using Microsoft.AspNetCore.Mvc;

namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ModuleName(HalobizModules.LeadAdministration,88)]

    public class SbuproportionController : ControllerBase
    {
        private readonly ISbuproportionService _sBUProportionService;

        public SbuproportionController(ISbuproportionService sBUProportionService)
        {
            this._sBUProportionService = sBUProportionService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetSbuproportion()
        {
            return await _sBUProportionService.GetAllSbuproportions();
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _sBUProportionService.GetSbuproportionById(id);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewSbuproportion(SbuproportionReceivingDTO sBUProportionReceiving)
        {
            return await _sBUProportionService.AddSbuproportion(HttpContext, sBUProportionReceiving);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, SbuproportionReceivingDTO sBUProportionReceiving)
        {
            return await _sBUProportionService.UpdateSbuproportion(HttpContext, id, sBUProportionReceiving);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _sBUProportionService.DeleteSbuproportion(id);
        }
    }
}