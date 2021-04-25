using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.MyServices;
using Microsoft.AspNetCore.Mvc;

namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SbuproportionController : ControllerBase
    {
        private readonly ISbuproportionService _sBUProportionService;

        public SbuproportionController(ISbuproportionService sBUProportionService)
        {
            this._sBUProportionService = sBUProportionService;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetSbuproportion()
        {
            var response = await _sBUProportionService.GetAllSbuproportions();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var sBUProportion = ((ApiOkResponse)response).Result;
            return Ok(sBUProportion);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var response = await _sBUProportionService.GetSbuproportionById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var sBUProportion = ((ApiOkResponse)response).Result;
            return Ok(sBUProportion);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNewSbuproportion(SbuproportionReceivingDTO sBUProportionReceiving)
        {
            var response = await _sBUProportionService.AddSbuproportion(HttpContext, sBUProportionReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var sBUProportion = ((ApiOkResponse)response).Result;
            return Ok(sBUProportion);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, SbuproportionReceivingDTO sBUProportionReceiving)
        {
            var response = await _sBUProportionService.UpdateSbuproportion(HttpContext, id, sBUProportionReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var sBUProportion = ((ApiOkResponse)response).Result;
            return Ok(sBUProportion);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var response = await _sBUProportionService.DeleteSbuproportion(id);
            return StatusCode(response.StatusCode);
        }
    }
}