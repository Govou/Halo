using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.MyServices;
using Microsoft.AspNetCore.Mvc;

namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SBUProportionController : ControllerBase
    {
        private readonly ISBUProportionService _sBUProportionService;

        public SBUProportionController(ISBUProportionService sBUProportionService)
        {
            this._sBUProportionService = sBUProportionService;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetSBUProportion()
        {
            var response = await _sBUProportionService.GetAllSBUProportions();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var sBUProportion = ((ApiOkResponse)response).Result;
            return Ok(sBUProportion);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var response = await _sBUProportionService.GetSBUProportionById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var sBUProportion = ((ApiOkResponse)response).Result;
            return Ok(sBUProportion);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNewSBUProportion(SBUProportionReceivingDTO sBUProportionReceiving)
        {
            var response = await _sBUProportionService.AddSBUProportion(HttpContext, sBUProportionReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var sBUProportion = ((ApiOkResponse)response).Result;
            return Ok(sBUProportion);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, SBUProportionReceivingDTO sBUProportionReceiving)
        {
            var response = await _sBUProportionService.UpdateSBUProportion(HttpContext, id, sBUProportionReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var sBUProportion = ((ApiOkResponse)response).Result;
            return Ok(sBUProportion);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var response = await _sBUProportionService.DeleteSBUProportion(id);
            return StatusCode(response.StatusCode);
        }
    }
}