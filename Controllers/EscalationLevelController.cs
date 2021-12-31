using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.MyServices;
using HaloBiz.MyServices.LAMS;
using Microsoft.AspNetCore.Mvc;

namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EscalationLevelController : ControllerBase
    {
        private readonly IEscalationLevelService _EscalationLevelService;

        public EscalationLevelController(IEscalationLevelService serviceTypeService)
        {
            this._EscalationLevelService = serviceTypeService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetEscalationLevel()
        {
            var response = await _EscalationLevelService.GetAllEscalationLevel();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var EscalationLevel = ((ApiOkResponse)response).Result;
            return Ok(EscalationLevel);
        }
        [HttpGet("caption/{name}")]
        public async Task<ApiCommonResponse> GetByCaption(string name)
        {
            var response = await _EscalationLevelService.GetEscalationLevelByName(name);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var EscalationLevel = ((ApiOkResponse)response).Result;
            return Ok(EscalationLevel);
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            var response = await _EscalationLevelService.GetEscalationLevelById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var EscalationLevel = ((ApiOkResponse)response).Result;
            return Ok(EscalationLevel);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewEscalationLevel(EscalationLevelReceivingDTO EscalationLevelReceiving)
        {
            var response = await _EscalationLevelService.AddEscalationLevel(HttpContext, EscalationLevelReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var EscalationLevel = ((ApiOkResponse)response).Result;
            return Ok(EscalationLevel);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, EscalationLevelReceivingDTO EscalationLevelReceiving)
        {
            var response = await _EscalationLevelService.UpdateEscalationLevel(HttpContext, id, EscalationLevelReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var EscalationLevel = ((ApiOkResponse)response).Result;
            return Ok(EscalationLevel);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            var response = await _EscalationLevelService.DeleteEscalationLevel(id);
            return StatusCode(response.StatusCode);
        }
    }
}
